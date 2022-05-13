using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float defaultGravity;
    public float quickFallGravity;

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;

    [Header("Combat")]
    public GameObject swingBox;
    public bool canSwing;
    public float activeTime;
    public float reloadTime;

    [Header("Art")]
    public SpriteRenderer sr;
    public Animator anim;

    [HideInInspector]
    public bool facingRight;

    public AudioSource swingingAudio;
    public AudioSource swingCollisionAudio;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        swingBox.SetActive(false);
        canSwing = true;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Grounded() && Time.timeScale != 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (canSwing && Time.timeScale != 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
                FaceRight();
            else if (Input.GetAxis("Horizontal") < 0)
                FaceLeft();

            if (Input.GetMouseButtonDown(0))
                StartCoroutine(Swing());
        }

        if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.gravityScale = quickFallGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), 0);

        transform.Translate(direction * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator Swing()
    {
        canSwing = false;

        swingingAudio.Play();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x >= transform.position.x)
            FaceRight();
        else
            FaceLeft();

        float offset = 1.5f;
        if(!facingRight)
            offset = -1.5f;

        swingBox.transform.position = (Vector2)transform.position + Vector2.right * offset;

        swingBox.SetActive(true);
        swingBox.GetComponent<SwingBox>().hitBall = false;
        anim.SetTrigger("Wind Up");
        yield return new WaitForSeconds(activeTime);
        anim.SetTrigger("Swing");
        if (!swingBox.GetComponent<SwingBox>().hitBall)
            swingBox.SetActive(false);
        else
            swingCollisionAudio.Play();
        yield return new WaitForSeconds(reloadTime);
        canSwing = true;
    }

    private void HealDamage(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    private bool Grounded()
    {
        int layerMask = 1 << 6;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, layerMask);

        if (hit.collider != null)
            return true;
        else
            return false;
    }

    private void FaceRight()
    {
        facingRight = true;
        sr.flipX = false;
    }

    private void FaceLeft()
    {
        facingRight = false;
        sr.flipX = true;
    }
}

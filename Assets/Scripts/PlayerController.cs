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
    private bool jumping;
    private bool attacking;
    private Vector2 walkDirection;

    private Coroutine currentSwing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        swingBox.SetActive(false);
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Grounded())
            anim.SetBool("Grounded", true);
        else
            anim.SetBool("Grounded", false);

        if(Input.GetKeyDown(KeyCode.Space) && Grounded() && Time.timeScale != 0 && !attacking)
        {
            StartCoroutine(jump());
        }

        if (!attacking && Time.timeScale != 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
                FaceRight();
            else if (Input.GetAxis("Horizontal") < 0)
                FaceLeft();

            if (Input.GetMouseButtonDown(0))
                currentSwing = StartCoroutine(Swing());
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
        if (!jumping)
        {
            Walk();
        }
    }

    private IEnumerator jump()
    {
        anim.SetBool("Jumping", true);
        jumping = true;
        yield return new WaitForSeconds(0.05f);
        jumping = false;
        anim.SetBool("Jumping", false);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private IEnumerator Swing()
    {
        attacking = true;

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
        anim.SetBool("Jumping", false);
        anim.SetTrigger("Wind Up");
        anim.SetBool("Attacking", true);
        yield return new WaitForSeconds(activeTime);
        if (!swingBox.GetComponent<SwingBox>().hitBall)
        {
            swingBox.SetActive(false);
            anim.SetTrigger("Swing");
            swingingAudio.Play();
        }

        yield return new WaitForSeconds(reloadTime);
        attacking = false;
        anim.SetBool("Attacking", false);
    }

    private void StopSwing()
    {
        StartCoroutine(StopSwingEnum());
    }

    private IEnumerator StopSwingEnum()
    {
        attacking = true;
        if (currentSwing != null)
            StopCoroutine(currentSwing);
        swingBox.GetComponent<SwingBox>().StopSwing();
        swingBox.SetActive(false);
        anim.SetBool("Attacking", false);

        yield return new WaitForSeconds(.25f);
        attacking = false;
    }

    public void HealDamage(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        StopSwing();
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

    private void Walk()
    {
        if(!attacking)
            walkDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        else if(Grounded())
            walkDirection = Vector2.zero;

        transform.Translate(walkDirection * moveSpeed * Time.fixedDeltaTime);
    }
}

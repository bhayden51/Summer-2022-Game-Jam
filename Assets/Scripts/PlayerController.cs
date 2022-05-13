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

    [Header("Combat")]
    public GameObject swingBox;
    public bool canSwing;

    [HideInInspector]
    public bool facingRight;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        swingBox.SetActive(false);
        canSwing = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Grounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }


        if (canSwing)
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
        yield return new WaitForSeconds(.02f);
        if(!swingBox.GetComponent<SwingBox>().hitBall)
            swingBox.SetActive(false);
        yield return new WaitForSeconds(.25f);
        canSwing = true;
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
    }

    private void FaceLeft()
    {
        facingRight = false;
    }
}

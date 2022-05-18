using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;

    public bool movingRight;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Grounded();

        if (movingRight)
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        else
            transform.Translate(-Vector2.right * speed * Time.fixedDeltaTime);
    }

    private void Grounded()
    {
        if (movingRight)
        {
            int layerMask = 1 << 6;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down * .9f + (Vector2.right * .6f), Vector2.down, 0.2f, layerMask);
            int layerMask2 = 1 << 9;
            RaycastHit2D hit2 = Physics2D.Raycast((Vector2)transform.position + Vector2.down * .9f + (Vector2.right * .6f), Vector2.down, 0.2f, layerMask2);

            int layerMask3 = 1 << 6;
            RaycastHit2D hit3 = Physics2D.Raycast((Vector2)transform.position + (Vector2.right * .5f), Vector2.right, 0.6f, layerMask3);
            int layerMask4 = 1 << 9;
            RaycastHit2D hit4 = Physics2D.Raycast((Vector2)transform.position + (Vector2.right * .5f), Vector2.right, 0.6f, layerMask4);

            if ((hit.collider == null && hit2.collider == null) || (hit3.collider != null || hit4.collider != null))
            {
                movingRight = false;
            }
        }
        else
        {
            int layerMask = 1 << 6;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down * .9f + (Vector2.left * .5f), Vector2.down, 0.2f, layerMask);
            int layerMask2 = 1 << 9;
            RaycastHit2D hit2 = Physics2D.Raycast((Vector2)transform.position + Vector2.down * .9f + (Vector2.left * .5f), Vector2.down, 0.2f, layerMask2);

            int layerMask3 = 1 << 6;
            RaycastHit2D hit3 = Physics2D.Raycast((Vector2)transform.position - (Vector2.right * .5f), -Vector2.right, 0.5f, layerMask3);
            int layerMask4 = 1 << 9;
            RaycastHit2D hit4 = Physics2D.Raycast((Vector2)transform.position - (Vector2.right * .5f), -Vector2.right, 0.5f, layerMask4);

            if ((hit.collider == null && hit2.collider == null) || (hit3.collider != null || hit4.collider != null))
            {
                movingRight = true;
            }
        }
    }
}

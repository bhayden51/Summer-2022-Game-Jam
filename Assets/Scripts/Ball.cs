using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject arrowPivot;
    public GameObject sprite;
    public SpriteRenderer sr;
    public int bouncesToLoseSpeed;

    [HideInInspector]
    public int speedLevel;
    [HideInInspector]
    public Vector2 aimDirection;
    [HideInInspector]
    public int bounces;
    [HideInInspector]
    public int target;
    [HideInInspector]
    public Rigidbody2D rb;

    private GameObject player;
    private PlayerController playerCon;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        playerCon = FindObjectOfType<PlayerController>();
        arrowPivot.SetActive(false);
    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (target == 0)
        {
            aimDirection = mousePos - (Vector2)transform.position;

            if (playerCon.facingRight)
            {
                if (aimDirection.x < 0)
                {
                    if (aimDirection.y >= 0)
                        aimDirection = new Vector2(.2f, 1);
                    else
                        aimDirection = new Vector2(.2f, -1);
                }
            }
            else
            {
                if (aimDirection.x > 0)
                {
                    if (aimDirection.y >= 0)
                        aimDirection = new Vector2(-.2f, 1);
                    else
                        aimDirection = new Vector2(-.2f, -1);
                }
            }
        }
        else if (target == 1)
            aimDirection = (player.transform.position - transform.position).normalized;

        arrowPivot.transform.position = transform.position;
        arrowPivot.transform.right = aimDirection;

        transform.right = rb.velocity;
    }

    void FixedUpdate()
    {
        float velocityMag = Mathf.Round(rb.velocity.magnitude);
        if (velocityMag > 50)
            velocityMag = 50;


        speedLevel = (int)Mathf.Floor(velocityMag / 10);
        velocityMag = speedLevel * 10;

        if (speedLevel == 5)
        {
            sprite.transform.localScale = new Vector3(1.25f, .5f, 1f);
            sr.color = new Color(1, 0, 0, 1);
        }
        else if (speedLevel == 4)
        {
            sprite.transform.localScale = new Vector3(1f, .75f, 1f);
            sr.color = new Color(1, .5f, 0, 1);
        }
        else if (speedLevel == 3)
        {
            sprite.transform.localScale = new Vector3(1f, .9f, 1f);
            sr.color = new Color(.5f, .5f, 1, 1);
        }
        else if (speedLevel == 2)
        {
            sprite.transform.localScale = new Vector3(1f, 1f, 1f);
            sr.color = new Color(.5f, 1, .5f, 1);
        }
        else if (speedLevel == 1)
        {
            sprite.transform.localScale = new Vector3(1f, 1f, 1f);
            sr.color = new Color(.5f, .5f, .5f, 1);
        }
        else
        {
            sprite.transform.localScale = new Vector3(1f, 1f, 1f);
            sr.color = new Color(.5f, .5f, .5f, 1);
        }

        rb.velocity = rb.velocity.normalized * velocityMag;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            bounces++;
            if(bounces >= bouncesToLoseSpeed)
            {
                bounces = 0;
                float newSpeed = ((speedLevel * 10) - 10);
                if (newSpeed < 10)
                    newSpeed = 10;
                rb.velocity = rb.velocity.normalized * newSpeed;
            }
        }
    }
}

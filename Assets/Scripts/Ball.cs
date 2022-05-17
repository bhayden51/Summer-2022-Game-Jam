using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject arrowPivot;
    public GameObject arrowScalePivot;
    public GameObject sprite;
    public SpriteRenderer colorSR;
    public SpriteRenderer glassSR;
    public int bouncesToLoseSpeed;
    public float playerInvinAfterHitting;

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
    [HideInInspector]
    public bool canHurtPlayer;

    private GameObject player;
    private PlayerController playerCon;
    private TrailRenderer trail;

    void Start()
    {
        arrowPivot.transform.SetParent(null);
        canHurtPlayer = false;
        player = FindObjectOfType<PlayerController>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        playerCon = FindObjectOfType<PlayerController>();
        arrowPivot.SetActive(false);
        trail = GetComponent<TrailRenderer>();
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
        {
            if(player != null)
                aimDirection = (player.transform.position - transform.position).normalized;
            else
                aimDirection = (Vector3.zero - transform.position).normalized;
        }

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
            ChangeBallAppearence(new Vector3(1.25f, .5f, 1f), new Color(1, 0, 0, 1));
        }
        else if (speedLevel == 4)
        {
            ChangeBallAppearence(new Vector3(1.1f, .8f, 1f), new Color(1, .5f, 0, 1));
        }
        else if (speedLevel == 3)
        {
            ChangeBallAppearence(new Vector3(1.05f, .9f, 1f), new Color(0f, 0f, 1, 1));
        }
        else if (speedLevel == 2)
        {
            ChangeBallAppearence(new Vector3(1.025f, .95f, 1f), new Color(.5f, 1, .5f, 1));
        }
        else if (speedLevel == 1)
        {
            ChangeBallAppearence(new Vector3(1f, 1f, 1f), new Color(.5f, .5f, .5f, 1));
        }
        else
        {
            ChangeBallAppearence(new Vector3(1f, 1f, 1f), new Color(.5f, .5f, .5f, 1));
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

    public void CantHurtPlayer()
    {
        StartCoroutine(CantHurtPlayerEnum());
    }

    private IEnumerator CantHurtPlayerEnum()
    {
        canHurtPlayer = false;
        yield return new WaitForSeconds(playerInvinAfterHitting);
        canHurtPlayer = true;
    }

    private void ChangeBallAppearence(Vector3 newScale, Color newColor)
    {
        sprite.transform.localScale = newScale;
        colorSR.color = newColor;
        glassSR.color = newColor;
        trail.startColor = newColor;
        trail.endColor = newColor;
        trail.startWidth = newScale.y / 2;
    }
}

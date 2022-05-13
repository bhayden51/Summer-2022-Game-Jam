using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public GameObject ball;
    public Ball ballScript;

    private void Start()
    {
        ballScript = ball.GetComponent<Ball>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && ballScript.canHurtPlayer)
        {
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<PlayerController>().TakeDamage(1);
            ballScript.canHurtPlayer = false;
        }
        if (collision.tag == "Enemy")
        {
            Enemy en = collision.gameObject.GetComponent<Enemy>();
            if (ballScript.speedLevel >= en.requiredSpeedToKill)
            {
                Destroy(en.gameObject);

                ballScript.bounces = 0;
                float newSpeed = ((ballScript.speedLevel * 10) - 20);
                if (newSpeed < 10)
                    newSpeed = 10;
                ballScript.rb.velocity = ballScript.rb.velocity.normalized * newSpeed;
            }
            else
            {
                en.Redirect(ball);
            }
        }
    }
}

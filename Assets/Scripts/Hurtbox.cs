using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public GameObject ball;
    public GameObject deathParticle;
    public Ball ballScript;

    private void Start()
    {
        ballScript = ball.GetComponent<Ball>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && ballScript.canHurtPlayer && ballScript.speedLevel > 1)
        {
            ballScript.bounces = 0;
            float newSpeed = ((ballScript.speedLevel * 10) - 10);
            if (newSpeed < 10)
                newSpeed = 10;
            ballScript.rb.velocity = ballScript.rb.velocity.normalized * newSpeed;
            collision.GetComponent<PlayerController>().TakeDamage(1);
            ballScript.canHurtPlayer = false;
            gameObject.SetActive(false);
        }
        if (collision.tag == "Enemy")
        {
            Enemy en = collision.gameObject.GetComponent<Enemy>();
            if (ballScript.speedLevel >= en.requiredSpeedToKill)
            {
                Instantiate(deathParticle, collision.gameObject.transform.position, Quaternion.identity);
                Destroy(en.gameObject);
                ballScript.bounces = 0;
                float newSpeed = ((ballScript.speedLevel * 10) - 10);
                if (newSpeed < 10)
                    newSpeed = 10;
                ballScript.rb.velocity = ballScript.rb.velocity.normalized * newSpeed;
                if (FindObjectsOfType<Enemy>().Length <= 1)
                {
                    FindObjectOfType<GameManager>().NextLevel();
                    gameObject.SetActive(false);
                }
            }
            else
            {
                en.Redirect(ball);
            }
        }
    }
}

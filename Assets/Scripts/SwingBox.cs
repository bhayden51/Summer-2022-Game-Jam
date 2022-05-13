using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingBox : MonoBehaviour
{
    public float hitPower;
    public float aimTime;

    [HideInInspector]
    public bool hitBall;

    private GameObject player;
    private Vector2 hitDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ball")
        {
            StartCoroutine(HitBall(collision.gameObject));
        }
    }
    private IEnumerator HitBall(GameObject ball)
    {
        hitBall = true;
        Ball ballScript = ball.GetComponent<Ball>();
        Time.timeScale = 0;
        ballScript.target = 0;
        ballScript.arrowPivot.SetActive(true);
        yield return new WaitForSecondsRealtime(aimTime - 0.05f);
        FindObjectOfType<PlayerController>().anim.SetTrigger("Swing");
        yield return new WaitForSecondsRealtime(0.05f);
        ballScript.arrowPivot.SetActive(false);
        hitDirection = ballScript.aimDirection.normalized;
        Time.timeScale = 1;
        LaunchBall(ball);
    }

    private void LaunchBall(GameObject ball)
    {
        Ball ballScript = ball.GetComponent<Ball>();
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        float ballSpeed = ballRb.velocity.magnitude;
        ballRb.velocity = Vector2.zero;
        ballRb.AddForce(hitDirection * (ballSpeed + hitPower), ForceMode2D.Impulse);
        ballScript.bounces = 0;
        gameObject.SetActive(false);
    }
}

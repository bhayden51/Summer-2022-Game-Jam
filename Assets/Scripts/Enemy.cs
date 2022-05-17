using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int requiredSpeedToKill;
    public float redirectTime;
    public float redirectPower;

    private GameObject player;
    private Vector2 hitDirection;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public void Redirect(GameObject ball)
    {
        StartCoroutine(HitBall(ball));
    }

    private IEnumerator HitBall(GameObject ball)
    {
        Ball ballScript = ball.GetComponent<Ball>();
        Time.timeScale = 0;
        ballScript.target = 1;
        ballScript.arrowPivot.SetActive(true);
        yield return new WaitForSecondsRealtime(redirectTime);
        ballScript.arrowPivot.SetActive(false);
        hitDirection = (player.transform.position - ball.transform.position).normalized;
        Time.timeScale = 1;
        LaunchBall(ball);
    }

    private void LaunchBall(GameObject ball)
    {
        FindObjectOfType<CameraController>().Shake();
        Ball ballScript = ball.GetComponent<Ball>();
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        float ballSpeed = ballRb.velocity.magnitude;
        ballRb.velocity = Vector2.zero;
        ballRb.AddForce(hitDirection * (ballSpeed + redirectPower), ForceMode2D.Impulse);
        ballScript.bounces = 0;
    }
}

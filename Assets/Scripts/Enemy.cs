using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int requiredSpeedToKill;
    public float redirectTime;
    public float redirectPower;
    public AudioSource collisionSound;
    public AudioSource chargeUpSound;

    private GameObject player;
    private Vector2 hitDirection;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public void Redirect(GameObject ball)
    {
        StartCoroutine(HitBall(ball));
    }

    private IEnumerator HitBall(GameObject ball)
    {
        anim.SetTrigger("Wind Up");
        Ball ballScript = ball.GetComponent<Ball>();
        chargeUpSound.pitch = ((float)ballScript.speedLevel / 10f) + .5f;
        chargeUpSound.Play();
        Time.timeScale = 0;
        ballScript.target = 1;
        ballScript.arrowPivot.SetActive(true);
        ballScript.arrowScalePivot.transform.localScale = new Vector3(.5f, 1, 1);
        yield return new WaitForSecondsRealtime(redirectTime);
        ballScript.arrowPivot.SetActive(false);
        if(player != null)
            hitDirection = (player.transform.position - ball.transform.position).normalized;
        else
            hitDirection = (Vector3.zero - ball.transform.position).normalized;
        Time.timeScale = 1;
        LaunchBall(ball);
    }

    private void LaunchBall(GameObject ball)
    {
        collisionSound.Play();
        anim.SetTrigger("Swing");
        FindObjectOfType<CameraController>().Shake();
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.canHurtPlayer = true;
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        float ballSpeed = ballRb.velocity.magnitude;
        ballRb.velocity = Vector2.zero;
        if(ballScript.speedLevel != 0)
            ballRb.AddForce(hitDirection * (ballSpeed + redirectPower), ForceMode2D.Impulse);
        else
            ballRb.AddForce(hitDirection * (ballSpeed + redirectPower * 2), ForceMode2D.Impulse);
        ballScript.bounces = 0;
    }
}

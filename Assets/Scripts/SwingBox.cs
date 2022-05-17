using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingBox : MonoBehaviour
{
    public float hitPower;
    public float aimTime;
    public AudioSource chargeUpSound;

    [HideInInspector]
    public bool hitBall;

    private PlayerController playerCon;
    private Vector2 hitDirection;
    private Ball ballScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCon = FindObjectOfType<PlayerController>();
        if (collision.tag == "Ball" && gameObject.activeInHierarchy)
        {
            StartCoroutine(HitBall(collision.gameObject));
        }
    }
    private IEnumerator HitBall(GameObject ball)
    {
        hitBall = true;
        ballScript = ball.GetComponent<Ball>();
        chargeUpSound.pitch = ((float)ballScript.speedLevel / 10f) + .5f;
        chargeUpSound.Play();
        Time.timeScale = 0;
        ballScript.target = 0;
        ballScript.arrowPivot.SetActive(true);
        ballScript.arrowScalePivot.transform.localScale = new Vector3(.5f, 1, 1);
        yield return new WaitForSecondsRealtime(aimTime - 0.05f);
        playerCon.anim.SetTrigger("Swing");
        playerCon.swingingAudio.Play();
        yield return new WaitForSecondsRealtime(0.05f);
        ballScript.arrowPivot.SetActive(false);
        hitDirection = ballScript.aimDirection.normalized;
        Time.timeScale = 1;
        LaunchBall(ball);
    }

    private void LaunchBall(GameObject ball)
    {
        FindObjectOfType<CameraController>().Shake();
        playerCon.swingCollisionAudio.Play();
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.CantHurtPlayer();
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        float ballSpeed = ballRb.velocity.magnitude;
        ballRb.velocity = Vector2.zero;
        ballRb.AddForce(hitDirection * (ballSpeed + hitPower), ForceMode2D.Impulse);
        ballScript.bounces = 0;
        gameObject.SetActive(false);
    }

    public void StopSwing()
    {
        StopAllCoroutines();
        hitBall = false;
        ballScript.arrowPivot.SetActive(false);
        Time.timeScale = 1;
    }
}

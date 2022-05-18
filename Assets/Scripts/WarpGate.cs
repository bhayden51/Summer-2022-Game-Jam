using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpGate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Ball ballScript = collision.gameObject.GetComponent<Ball>();
            ballScript.bounces = 0;
            float newSpeed = ((ballScript.speedLevel * 10) + 10);
            if (newSpeed > 50)
                newSpeed = 50;
            ballScript.rb.velocity = ballScript.rb.velocity.normalized * newSpeed;
        }
    }
}

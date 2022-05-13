using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public GameObject ball;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Debug.Log("Hurt");
        }
    }
}

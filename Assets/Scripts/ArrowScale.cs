using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScale : MonoBehaviour
{
    public float growSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x < 1)
        {
            transform.localScale = new Vector3(transform.localScale.x + growSize * Time.unscaledDeltaTime, 1, 1);
        }
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
}

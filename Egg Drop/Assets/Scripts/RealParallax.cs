using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealParallax : MonoBehaviour
{
    private float length;
    private float StartPos;
    public GameObject Camera;
    public float speed;

    void Start()
    {
        StartPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        
    }

    void FixedUpdate()
    {
        float temp = (Camera.transform.position.x * (1 - speed));
        float distance = (Camera.transform.position.x * speed);
        transform.position = new Vector3(StartPos + distance, transform.position.y, transform.position.z);

        if (temp > StartPos + length) StartPos += length;
        else if (temp < StartPos - length) StartPos -= length;
    }
}

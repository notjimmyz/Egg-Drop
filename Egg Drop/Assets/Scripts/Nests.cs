using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nests : MonoBehaviour
{
    public float speed = 5f;
    private float leftEdge;
    private bool hasEgg = false;
    private bool isMoving = true;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position += Vector3.left * (speed * Time.deltaTime);
            if (transform.position.x < leftEdge)
            {
                Destroy(gameObject);
            }
        }
    }

    public bool HasEgg()
    {
        return hasEgg;
    }

    public void SetEgg()
    {
        hasEgg = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }
}
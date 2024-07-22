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

    public void AttachEgg(GameObject egg, Vector2 offset)
    {
        egg.transform.SetParent(transform);
        egg.transform.localPosition = new Vector3(offset.x, offset.y, 0); // Position egg with offset
        Egg eggScript = egg.GetComponent<Egg>();
        if (eggScript != null)
        {
            eggScript.StopEgg(); // Ensure the egg stops moving independently
            eggScript.SetSortingLayerInFront(); // Ensure the egg is in front
        }
    }
}

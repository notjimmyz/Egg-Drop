using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffect = 0.6f;
    public float leftBound;
    public float rightBound;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffect, 0, 0);
        lastCameraPosition = cameraTransform.position;

        if (cameraTransform.position.x - transform.position.x >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x - cameraTransform.position.x >= textureUnitSizeX)
        {
            float offsetPositionX = (transform.position.x - cameraTransform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x - offsetPositionX, transform.position.y, transform.position.z);
        }

        // Clamp the background position between left and right bounds
        float clampedX = Mathf.Clamp(transform.position.x, leftBound, rightBound - textureUnitSizeX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
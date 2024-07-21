using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundLayer
    {
        public GameObject backgroundPrefab; // Prefab of the background
        public float verticalOffset;
        [HideInInspector] public GameObject instance; // Instance of the prefab
    }

    public BackgroundLayer[] backgrounds; // Array of background layers with vertical offsets
    public float parallaxSpeed = 0.5f; // Speed of the parallax effect
    private float backgroundWidth; // Width of each background layer

    void Start()
    {
        if (backgrounds.Length > 0)
        {
            // Instantiate the first prefab to get its width
            GameObject tempInstance = Instantiate(backgrounds[0].backgroundPrefab, Vector3.zero, Quaternion.identity);
            backgroundWidth = tempInstance.GetComponent<SpriteRenderer>().bounds.size.x;
            Destroy(tempInstance);

            // Instantiate all prefabs and set their initial positions
            for (int i = 0; i < backgrounds.Length; i++)
            {
                Vector3 position = new Vector3(i * backgroundWidth, backgrounds[i].verticalOffset, 0);
                backgrounds[i].instance = Instantiate(backgrounds[i].backgroundPrefab, position, Quaternion.identity);
            }
        }
    }

    void Update()
    {
        foreach (BackgroundLayer layer in backgrounds)
        {
            // Move the background
            layer.instance.transform.position += Vector3.left * parallaxSpeed * Time.deltaTime;

            // If the background is out of view, reposition it
            if (layer.instance.transform.position.x <= -backgroundWidth)
            {
                RepositionBackground(layer);
            }
        }
    }

    private void RepositionBackground(BackgroundLayer layer)
    {
        Vector3 offset = new Vector3(backgroundWidth * backgrounds.Length, 0, 0);
        layer.instance.transform.position += offset;
        layer.instance.transform.position = new Vector3(layer.instance.transform.position.x, layer.verticalOffset, layer.instance.transform.position.z);
    }
}

using UnityEngine;

public class ParalexBack : MonoBehaviour
{
    public Transform[] layers; // Array to hold the layers
    public float[] parallaxScales; // Array to hold the parallax scales for each layer
    public float smoothing = 1f; // Smoothing factor for the parallax effect

    private Transform cam;
    private Vector3 previousCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;

        // Ensure the arrays are the same length
        if (layers.Length != parallaxScales.Length)
        {
            Debug.LogError("Layers and parallaxScales arrays must be the same length.");
            this.enabled = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = layers[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, layers[i].position.y, layers[i].position.z);

            layers[i].position = Vector3.Lerp(layers[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}

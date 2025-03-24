using UnityEngine;
public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation
    public bool clockwise = true; // Determines rotation direction

    void Update()
    {
        float direction = clockwise ? 1f : -1f;
        // Change Vector3.right to the appropriate axis:
        // Vector3.up for Y-axis rotation
        // Vector3.forward for Z-axis rotation
        // Vector3.right for X-axis rotation
        transform.Rotate(Vector3.up * rotationSpeed * direction * Time.deltaTime);
    }
}
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation
    public bool clockwise = true; // Determines rotation direction

    public enum RotationAxis { X, Y, Z } // Enum for axis selection
    public RotationAxis rotationAxis = RotationAxis.Y; // Default to Y-axis

    void Update()
    {
        float direction = clockwise ? 1f : -1f;
        Vector3 axis = Vector3.zero;

        // Set rotation axis based on selection
        switch (rotationAxis)
        {
            case RotationAxis.X:
                axis = Vector3.right;
                break;
            case RotationAxis.Y:
                axis = Vector3.up;
                break;
            case RotationAxis.Z:
                axis = Vector3.forward;
                break;
        }

        transform.Rotate(axis * rotationSpeed * direction * Time.deltaTime);
    }
}

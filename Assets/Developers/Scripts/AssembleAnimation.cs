using UnityEngine;
using System.Collections;

public class AssembleAnimation : MonoBehaviour
{
    public Transform[] parts; // Array to hold all the parts of the asset
    public float duration = 0.5f; // Reduced duration for faster animation
    public float delay = 0.05f; // Reduced delay between batches
    public Vector3 offScreenPosition = new Vector3(-5, 0, 0); // Closer off-screen position
    public AnimationCurve easeCurve; // Easing curve for smooth animation
    public int partsPerBatch = 5; // Number of parts to animate simultaneously

    private Vector3[] targetPositions; // Array to hold the target positions for each part
    private Vector3[] startPositions; // Array to hold the start positions for each part

    private void Start()
    {
        // Initialize the target positions array
        targetPositions = new Vector3[parts.Length];
        startPositions = new Vector3[parts.Length];

        // Move all parts off-screen initially and store their target and start positions
        for (int i = 0; i < parts.Length; i++)
        {
            startPositions[i] = parts[i].position; // Store the initial position
            parts[i].position = offScreenPosition; // Move to off-screen position
            targetPositions[i] = parts[i].parent.TransformPoint(parts[i].localPosition); // Calculate world position
            parts[i].gameObject.SetActive(false); // Hide the part initially
        }

        // Start the assembly animation
        StartCoroutine(AnimateAssembly());
    }

    private IEnumerator AnimateAssembly()
    {
        for (int i = 0; i < parts.Length; i += partsPerBatch)
        {
            int batchSize = Mathf.Min(partsPerBatch, parts.Length - i);
            for (int j = 0; j < batchSize; j++)
            {
                StartCoroutine(MoveToPosition(parts[i + j], startPositions[i + j], targetPositions[i + j], duration, 0));
            }
            yield return new WaitForSeconds(delay); // Wait before starting the next batch
        }
    }

    private IEnumerator MoveToPosition(Transform part, Vector3 startPosition, Vector3 targetPosition, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        part.gameObject.SetActive(true); // Make the part visible

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = easeCurve.Evaluate(elapsedTime / duration); // Use the easing curve for smooth movement
            part.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        part.position = targetPosition; // Ensure the part reaches the exact target position
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ViolentVibration : MonoBehaviour
{
    public RectTransform target;
    public float intensity = 10f; // Adjust for more or less violent shaking
    public float speed = 50f;

    private Vector3 originalPosition;

    void Start()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        originalPosition = target.anchoredPosition;
    }

    void Update()
    {
        float xOffset = Random.Range(-intensity, intensity);
        float yOffset = Random.Range(-intensity, intensity);

        target.anchoredPosition = originalPosition + new Vector3(xOffset, yOffset, 0);
    }
}

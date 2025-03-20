using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    private Camera mainCamera;
    private float screenHalfHeightInWorldUnits;

    void Start()
    {
        mainCamera = Camera.main;
        screenHalfHeightInWorldUnits = mainCamera.orthographicSize;
    }

    void Update()
    {
        Vector3 playerPosition = transform.position;
        //float playerHalfHeight = GetComponent<Renderer>().bounds.extents.y;

        // Calculate the screen bounds
        float topBound = screenHalfHeightInWorldUnits;
        float bottomBound = -screenHalfHeightInWorldUnits;

        // Clamp the player's position within the screen bounds
        playerPosition.y = Mathf.Clamp(playerPosition.y, bottomBound, topBound);
        transform.position = playerPosition;
    }
}

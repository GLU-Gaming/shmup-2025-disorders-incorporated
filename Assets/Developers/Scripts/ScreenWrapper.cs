using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    private Camera mainCamera;
    private float screenHalfHeightInWorldUnits;
    public float BottomoffSet;
    public float TopoffSet;

    void Start()
    {
        mainCamera = Camera.main;
        screenHalfHeightInWorldUnits = mainCamera.orthographicSize;
    }

    void Update()
    {
        Vector3 playerPosition = transform.position;

        // Calculate the screen bounds
        float topBound = screenHalfHeightInWorldUnits;
        float bottomBound = -screenHalfHeightInWorldUnits;

        // Clamp the player's position within the screen bounds
        playerPosition.y = Mathf.Clamp(playerPosition.y, bottomBound - BottomoffSet, topBound + TopoffSet);
        transform.position = playerPosition;
    }

    void OnDrawGizmos()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Calculate the screen bounds
        float screenHalfWidthInWorldUnits = mainCamera.aspect * mainCamera.orthographicSize;
        float screenHalfHeightInWorldUnits = mainCamera.orthographicSize;

        // Define the start and end points of the line relative to the camera's position
        Vector3 start = mainCamera.transform.position + new Vector3(-screenHalfWidthInWorldUnits, 0, 0);
        Vector3 end = mainCamera.transform.position + new Vector3(screenHalfWidthInWorldUnits, 0, 0);

        // Draw the blue line in the middle of the screen
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(start, end);
    }
}

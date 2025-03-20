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
}

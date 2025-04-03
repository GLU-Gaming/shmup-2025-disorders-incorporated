using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Gamemanager gamemanager;
    private PlayerMovement playerMovement;

    private void Start()
    {
        // Find the GameManager object in the scene
        gamemanager = FindAnyObjectByType<Gamemanager>();
        if (gamemanager == null)
        {
            Debug.LogError("Gamemanager component not found in the scene.");
        }

        // Find the Player object in the scene
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerMovement = playerObject.GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement component not found on the Player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerMovement.ForceFieldCharge = gamemanager.maxForceCharge; // Correctly set the ForceFieldCharge
            gamemanager.UpdateForceChargeUI(playerMovement.ForceFieldCharge); // Update the UI
            gameObject.SetActive(false); // Deactivate the pickup object
        }
    }
}

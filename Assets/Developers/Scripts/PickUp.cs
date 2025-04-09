using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Gamemanager gamemanager;
    private PlayerMovement playerMovement;

    public enum RotationAxis { Hp, Shield, Fire } // Enum for axis selection
    public RotationAxis rotationAxis = RotationAxis.Hp; // Default to Hp mode

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
            switch (rotationAxis)
            {
                case RotationAxis.Hp:
                    Health health = other.GetComponent<Health>();
                    if (health != null)
                    {
                        health.RestoreHealth(100f);
                    }
                    break;
                case RotationAxis.Shield:
                    if (playerMovement != null)
                    {
                        playerMovement.ForceFieldCharge = gamemanager.maxForceCharge; // Fully charge the shield
                        gamemanager.UpdateForceChargeUI(playerMovement.ForceFieldCharge); // Update the UI
                    }
                    break;
                case RotationAxis.Fire:
                    if (playerMovement != null)
                    {
                        playerMovement.FlameThrowerCharge = gamemanager.maxFlameThrowerCharge;
                        gamemanager.UpdateFlameThrowerChargeUI(playerMovement.FlameThrowerCharge);
                    }
                    break;
            }
            gameObject.SetActive(false); // Deactivate the pickup object
        }
    }
}

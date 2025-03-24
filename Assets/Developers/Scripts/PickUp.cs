using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Gamemanager gamemanager;
    public GameObject Manager;

    private PlayerMovement playerMovement;
    public GameObject Player;

    private void Start()
    {
        gamemanager = Manager.GetComponent<Gamemanager>();
        if (gamemanager == null)
        {
            Debug.LogError("Gamemanager component not found");
        }

        playerMovement = Player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on the player.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerMovement.ForceFieldCharge = gamemanager.maxForceCharge; // Correctly set the ForceFieldCharge
            gamemanager.UpdateForceChargeUI(playerMovement.ForceFieldCharge); // Update the UI
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public GameObject Player;

    public float maxForceCharge = 300;
    public Image ForceFill;

    private void Start()
    {
        playerMovement = Player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on the player.");
        }

        UpdateForceChargeUI(maxForceCharge); // Initialize the UI bar to full charge
    }

    public void UpdateForceChargeUI(float currentCharge)
    {
        if (ForceFill != null)
        {
            ForceFill.fillAmount = currentCharge / maxForceCharge;
            Debug.Log("Force Charge Updated: " + ForceFill.fillAmount);
        }
    }
}

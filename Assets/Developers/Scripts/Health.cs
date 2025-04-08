using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public AudioSource AudioDamageSource;
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthbarFill;
    private Gamemanager gamemanager;
    public float lowHealthThreshold = 200f; // Threshold for low health

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        gamemanager = FindAnyObjectByType<Gamemanager>();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("TakeDamage called with amount: " + amount);
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        AudioDamageSource.Play();
    }

    public void RestoreHealth(float amount)
    {
        Debug.Log("RestoreHealth called with amount: " + amount);
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthbarFill != null)
        {
            healthbarFill.fillAmount = currentHealth / maxHealth;
            Debug.Log("Health Bar Updated: " + healthbarFill.fillAmount);
            if (currentHealth == 0)
            {
                Death();
            }
            else if (currentHealth <= lowHealthThreshold)
            {
                if (gamemanager != null)
                {
                    gamemanager.ActivateVignette(true);
                }
            }
            else
            {
                if (gamemanager != null)
                {
                    gamemanager.ActivateVignette(false);
                }
            }
        }
    }

    // Death function to destroy the GameObject and its parent objects
    public void Death()
    {
        Debug.Log(gameObject.name + " has died!");

        // Destroy the current GameObject
        Destroy(gameObject);

        // Destroy all parent objects
        Transform parent = transform.parent;
        while (parent != null)
        {
            Destroy(parent.gameObject);
            parent = parent.parent;
        }

        if (gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Gameover");
        }

        if (gameObject.CompareTag("Enemy"))
        {
            if (gamemanager != null)
            {
                gamemanager.IncreaseScore(15);
            }
        }
    }
}

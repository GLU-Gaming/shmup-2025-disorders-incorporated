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

        if (gameObject.CompareTag("Player"))
        {
            AudioDamageSource.Play();
        }
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

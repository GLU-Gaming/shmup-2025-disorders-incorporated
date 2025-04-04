using UnityEngine;

public class EyeProjectile : MonoBehaviour
{
    public float speed = 15f; // Speed of the projectile
    public float homingDuration = 1f; // Duration for which the projectile homes in on the player
    public float returnDuration = 2f; // Duration for which the projectile returns to its original position
    private Transform player; // Reference to the player's transform
    private Vector3 originalPosition; // Original position of the projectile
    private float homingTimer; // Timer for the homing phase
    private bool isReturning = false; // Flag to indicate if the projectile is returning
    private bool isActive = false; // Flag to indicate if the projectile is active
    public float damageCount = 5f;



    private void Start()
    {
        originalPosition = transform.position;
        homingTimer = homingDuration;
        FindPlayer();
        Deactivate(); // Deactivate the projectile initially
    }

    private void Update()
    {
        if (isActive)
        {
            if (!isReturning)
            {
                Homing();
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player GameObject is tagged with 'Player'.");
        }
    }

    private void Homing()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            homingTimer -= Time.deltaTime;

            if (homingTimer <= 0)
            {
                isReturning = true;
            }
        }
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = Vector3.Lerp(transform.position, originalPosition, returnDuration * Time.deltaTime);

        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            Deactivate(); // Deactivate the projectile instead of destroying it
        }
    }

    public void Activate()
    {
        isActive = true;
        isReturning = false;
        homingTimer = homingDuration;
        transform.position = originalPosition; // Reset position
    }

    public void Deactivate()
    {
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damageCount);
        }
    }
}

using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile
    public float homingDuration = 2f; // Duration for which the projectile homes in on the player
    public float returnDuration = 2f; // Duration for which the projectile returns to its original position
    private Transform player; // Reference to the player's transform
    private Vector3 originalPosition; // Original position of the projectile
    private float homingTimer; // Timer for the homing phase
    private bool isReturning = false; // Flag to indicate if the projectile is returning

    private void Start()
    {
        originalPosition = transform.position;
        homingTimer = homingDuration;
        FindPlayer();
    }

    private void Update()
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
            transform.rotation = Quaternion.LookRotation(direction); // Rotate towards the player
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
            Destroy(gameObject);
        }
    }
}

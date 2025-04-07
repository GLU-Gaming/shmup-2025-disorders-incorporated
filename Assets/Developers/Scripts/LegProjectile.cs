using UnityEngine;

public class LegProjectile : MonoBehaviour
{
    public float rotationSpeed = 200f; // Speed at which the leg rotates
    public float moveSpeed = 20f; // Speed at which the leg moves forward
    public float rotationDuration = 0.45f; // Duration for which the leg rotates
    public float homingDuration = 3f; // Duration for which the leg homes in on the player
    public float moveDuration = 10f; // Duration for which the leg moves forward
    public bool isLeftLeg = true; // Boolean to identify if the leg is a left leg

    private float rotationTimer;
    private float homingTimer;
    private float moveTimer;
    private bool isRotating = true;
    private bool isHoming = false;
    private bool isMovingForward = false;
    private Transform player;

    public void Initialize()
    {
        rotationTimer = rotationDuration;
        homingTimer = homingDuration;
        moveTimer = moveDuration;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isRotating)
        {
            RotateLeg();
        }
        else if (isHoming)
        {
            HomeInOnPlayer();
        }
        else if (isMovingForward)
        {
            MoveLegForward();
        }
    }

    private void RotateLeg()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Rotate around the negative X-axis for left leg and positive X-axis for right leg
            if (isLeftLeg)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation * Quaternion.Euler(-200, 0, 0), rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation * Quaternion.Euler(-150, 0, 100), rotationSpeed * Time.deltaTime);
            }

            rotationTimer -= Time.deltaTime;

            if (rotationTimer <= 0)
            {
                isRotating = false;
                isHoming = true;
            }
        }
    }

    private void HomeInOnPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            homingTimer -= Time.deltaTime;

            if (homingTimer <= 0)
            {
                isHoming = false;
                isMovingForward = true;
            }
        }
    }

    private void MoveLegForward()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    
}

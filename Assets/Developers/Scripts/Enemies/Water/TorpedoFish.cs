using UnityEngine;
using System.Collections;

public class TorpedoFish : FlyingEnemy
{
    public bool isMoving = true; // Flag to control movement
    public float lifetime = 4f;
    public float attackInterval = 2f; // Interval between attacks in seconds
    public GameObject Projectile;
    public Transform ProjectilePosistioning;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifetime);
        StartCoroutine(AttackRoutine()); // Start the attack routine
    }

    void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    public override void Attack()
    {
        // Implement the attack logic here
        if (Projectile != null)
        {
            Instantiate(Projectile, transform.position, ProjectilePosistioning.rotation);
            Debug.Log("TorpedoFish attacked!");
        }
    }

    public override void Move()
    {
        // Move in the direction the fish is facing
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void PushBack(Vector3 direction, float force)
    {
        StartCoroutine(PushBackRoutine(direction, force));
    }

    private IEnumerator PushBackRoutine(Vector3 direction, float force)
    {
        isMoving = false; // Stop moving
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f); // Wait for a short duration

            // Gradually reduce velocity over 1 second
            float dampingTime = 1f;
            float elapsedTime = 0f;
            Vector3 initialVelocity = rb.linearVelocity;

            while (elapsedTime < dampingTime)
            {
                rb.linearVelocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsedTime / dampingTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rb.linearVelocity = Vector3.zero; // Ensure velocity is zero
            rb.angularVelocity = Vector3.zero; // Reset angular velocity
        }
        isMoving = true; // Resume moving
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval); // Wait for the attack interval
            Attack(); // Perform the attack
        }
    }
}

using UnityEngine;
using System.Collections;

public class Drone : FlyingEnemy
{
    public float bobFrequency = 1f; // Frequency of the bobbing motion
    public float bobAmplitude = 0.5f; // Amplitude of the bobbing motion
    private bool isMoving = true; // Flag to control movement

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            Move();
        }
        // Bob up and down
        float bobbing = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + bobbing, transform.position.z);
    }

    public override void Attack()
    {
        // Implement Drone attack logic if needed
    }

    public override void Move()
    {
        // Move to the left (negative X direction)
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
}

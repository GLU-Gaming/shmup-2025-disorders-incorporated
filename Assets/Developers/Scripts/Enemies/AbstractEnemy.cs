using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : MonoBehaviour
{
    public int health;
    public float speed;
    public float bobFrequency = 1f; // Frequency of the bobbing motion
    public float bobAmplitude = 0.5f; // Amplitude of the bobbing motion
    protected bool isMoving = true; // Flag to control movement
    protected Vector3 startPosition;

    private GameObject Player;
    public float resetDistance = 50f; // Distance at which the enemy should reset
    public float resetOffset = 70f; // Offset to reset the enemy's position

    protected virtual void Start()
    {
        startPosition = transform.position;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        if (isMoving)
        {
            Move();
        }
        // Bob up and down
        float bobbing = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + bobbing, transform.position.z);
        CheckAndResetPosition();
    }

    public abstract void Attack();
    public abstract void Move();

    protected virtual void CheckAndResetPosition()
    {
        if (Player != null && transform.position.x < Player.transform.position.x - resetDistance)
        {
            ResetPosition();
        }
    }

    protected virtual void ResetPosition()
    {
        transform.position = new Vector3(Player.transform.position.x + resetOffset, startPosition.y, transform.position.z);
    }

    public virtual void PushBack(Vector3 direction, float force)
    {
        StartCoroutine(PushBackRoutine(direction, force));
    }

    protected virtual IEnumerator PushBackRoutine(Vector3 direction, float force)
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

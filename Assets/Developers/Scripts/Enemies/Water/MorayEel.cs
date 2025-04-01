using UnityEngine;
using TMPro;

public class MorayEel : MonoBehaviour
{
    public float detectionRange = 3f;
    public float attackRange = 1f;
    public int attackDamage = 30;
    public float emergeSpeed = 1f;
    public float retreatSpeed = 2f;
    public float attackDistance = 2f; // Customizable attack distance
    public Vector3 detectionBoxSize = new Vector3(1f, 0.1f, 1f); // Size of the detection box
    public float attackDelay = 1f; // Editable attack delay
    public float vibrationAmplitude = 0.1f; // Amplitude of the vibration
    public float vibrationFrequency = 10f; // Frequency of the vibration
    public TMP_Text warningText; // Reference to the TextMeshPro component

    private enum State { Hidden, PreparingToAttack, Emerging, Attacking, Retreating, Vanished }
    private State currentState = State.Hidden;
    private float emergeTime;
    private float retreatTime;
    private float attackTime;
    private Vector3 initialPosition;
    private Vector3 attackPosition;

    private void Start()
    {
        initialPosition = transform.position;
        attackPosition = initialPosition + Vector3.up * attackDistance; // Use the customizable attack distance
        warningText.gameObject.SetActive(false); // Ensure the warning text is initially hidden
    }

    private void Update()
    {
        if (currentState == State.Vanished)
        {
            // Do nothing if the eel has vanished
            return;
        }

        bool playerDetected = IsPlayerBeneath();

        switch (currentState)
        {
            case State.Hidden:
                if (playerDetected)
                {
                    currentState = State.PreparingToAttack;
                    attackTime = Time.time + attackDelay;
                    warningText.gameObject.SetActive(true); // Show the warning text
                }
                break;

            case State.PreparingToAttack:
                if (Time.time >= attackTime)
                {
                    currentState = State.Emerging;
                    emergeTime = Time.time + emergeSpeed;
                    warningText.gameObject.SetActive(false); // Hide the warning text
                }
                else
                {
                    // Vibrate the eel
                    transform.position = initialPosition + new Vector3(
                        Mathf.Sin(Time.time * vibrationFrequency) * vibrationAmplitude,
                        0,
                        Mathf.Cos(Time.time * vibrationFrequency) * vibrationAmplitude
                    );
                }
                break;

            case State.Emerging:
                if (Time.time >= emergeTime)
                {
                    transform.position = attackPosition;
                    currentState = State.Attacking;
                    retreatTime = Time.time + retreatSpeed;
                    AttackPlayer();
                }
                else
                {
                    transform.position = Vector3.Lerp(initialPosition, attackPosition, (Time.time - (emergeTime - emergeSpeed)) / emergeSpeed);
                }
                break;

            case State.Attacking:
                if (Time.time >= retreatTime)
                {
                    currentState = State.Retreating;
                    emergeTime = Time.time + emergeSpeed;
                }
                break;

            case State.Retreating:
                if (Time.time >= emergeTime)
                {
                    transform.position = initialPosition;
                    currentState = State.Vanished;
                    Destroy(gameObject);
                }
                else
                {
                    transform.position = Vector3.Lerp(attackPosition, initialPosition, (Time.time - (emergeTime - emergeSpeed)) / emergeSpeed);
                }
                break;
        }
    }

    private bool IsPlayerBeneath()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, detectionBoxSize / 2, Quaternion.identity);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void AttackPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<Health>().TakeDamage(attackDamage);
            Debug.Log("Maw Ray attacks the player!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection box
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw attack distance
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * attackDistance);
    }
}

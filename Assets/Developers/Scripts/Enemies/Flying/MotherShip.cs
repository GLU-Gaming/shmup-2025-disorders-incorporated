using UnityEngine;
using System.Collections;

public class MotherShip : FlyingEnemy
{
    public Animator animator; // Reference to the Animator component
    public GameObject enemyPrefab; // Prefab of the enemy to summon
    public float summonInterval = 5f; // Interval between summoning enemies
    public int numberOfEnemiesToSummon = 3; // Number of enemies to summon each time
    public float summonRadius = 2f; // Radius within which enemies will be summoned
    public float spawnOffsetZ = 0f; // Z position to keep constant
    public float minSpawnY = -5f; // Minimum Y value for spawning
    public float maxSpawnY = -1f; // Maximum Y value for spawning
    private bool isMoving = true; // Flag to control movement
    public float bobFrequency = 1f; // Frequency of the bobbing motion
    public float bobAmplitude = 0.5f; // Amplitude of the bobbing motion

    private bool isAttacking = false;
    private Vector3 startPosition;


    public void Start()
    {
        StartCoroutine(RepeatAttackRoutine());
        startPosition = transform.position;

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
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackRoutine());
        }
    }

    public override void Move()
    {
        // Move backward (negative Z direction)
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        float bobbing = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + bobbing, transform.position.z);
    }

    private IEnumerator RepeatAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(summonInterval);
            Attack();
        }
    }

    private IEnumerator AttackRoutine()
    {
        // Play the attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // Assuming the animation trigger is named "Attack"
        }

        yield return new WaitForSeconds(2f); // Adjust this duration based on your animation length

        // Summon enemies
        for (int i = 0; i < numberOfEnemiesToSummon; i++)
        {
            Vector3 summonPosition = transform.position;
            summonPosition.y = Random.Range(minSpawnY, maxSpawnY); // Random Y position within the range
            summonPosition.z = spawnOffsetZ; // Keep Z position constant
            summonPosition.x += (i - (numberOfEnemiesToSummon - 1) / 2f) * summonRadius; // Align horizontally

            Instantiate(enemyPrefab, summonPosition, Quaternion.identity);
        }

        isAttacking = false; // Allow the attack to be triggered again
    }
}

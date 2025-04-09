using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings:")]
    public float moveSpeed = 4f;
    private Health healthScript;
    private Gamemanager gamemanager;
    public GameObject Manager;
    private bool Moving;
    private float moveX = 1f; // Default X movement value

    [Header("ForceField:")]
    [SerializeField] private GameObject ForceField;
    public AudioSource AudioShieldSource;
    public float ForceFieldCharge;
    private bool ForceFieldActive;
    public float shockwaveRadius = 5f; // Radius of the shockwave
    public float shockwaveForce = 10f; // Force applied to objects

    [Header("Base Laser Settings:")]
    public AudioSource AudioLaserSource;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    [Header("Torpedo Setting:")]
    public GameObject torpedoPrefab;
    public Transform torpedoPosition;
    public Transform torpedoFirePoint;
    private float nextTorpedoFireTime = 0f;
    public float torpedoFireRate = 2f;
    public bool canShootTorpedo = true;

    [Header("FlameThrower:")]
    public AudioSource AudioSource;
    public GameObject FlamethrowerPrefab;
    public float flameThrowerFireRate = 3;
    private float nextFlameThrowerFireTime = 0f;
    public bool canShootFlames = true;
    public float FlameThrowerCharge;
    private bool FlameThrowerActive;
    public float flameThrowerDrainRate = 1f; // Rate at which the flamethrower charge drains

    [Header("Animation:")]
    public Transform childWithAnimator; // Reference to the child object with the Animator component
    private Animator animator;
    public GameObject spawnParticlePrefab; // Prefab of the particle system to instantiate
    public GameObject splashParticlePrefab; // Prefab of the splash particle system to instantiate
    public AudioSource splashAudioSource; // Audio source for the splash sound

    void Start()
    {
        InitializeComponents();
        InitializeCharges();
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleShielding();
        HandleFlameThrower();

        // Check if the player is at y = -2.0000 and spawn the splash particle effect
        if (Mathf.Approximately(transform.position.y, -2f) && splashParticlePrefab != null)
        {
            SpawnSplashParticles();
        }
    }

    void InitializeComponents()
    {
        healthScript = GetComponent<Health>();
        if (healthScript == null)
        {
            Debug.LogError("Health component not found on the player.");
        }

        gamemanager = Manager.GetComponent<Gamemanager>();
        if (gamemanager == null)
        {
            Debug.LogError("Gamemanager component not found");
        }

        if (childWithAnimator != null)
        {
            animator = childWithAnimator.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the child object.");
            }
        }
        else
        {
            Debug.LogError("Child object with Animator not assigned.");
        }
    }

    void InitializeCharges()
    {
        if (gamemanager != null)
        {
            ForceFieldCharge = gamemanager.maxForceCharge;
            FlameThrowerCharge = gamemanager.maxFlameThrowerCharge;
        }
    }

    void HandleMovement()
    {
        float moveY = Input.GetAxis("Vertical"); // W/S movement for up and down
        Vector2 movement = new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime;
        transform.Translate(movement.x, movement.y, 0, Space.World);
    }

    public void StopMovementOnXAxis()
    {
        moveX = 0; // Set the X movement to 0
    }

    void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetButton("Fire2") && Time.time >= nextTorpedoFireTime && canShootTorpedo)
        {
            StartCoroutine(DelayedShootTorpedo());
            nextTorpedoFireTime = Time.time + torpedoFireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, bulletPosition.rotation); // Bullet is shot at set bullet point
        AudioLaserSource.Play();
    }

    IEnumerator DelayedShootTorpedo()
    {
        canShootTorpedo = false;
        ControlTorpedoAnimation(true);

        yield return new WaitForSeconds(1.45f); // Wait for 1.45 seconds
        SpawnTorpedoParticles();
        Instantiate(torpedoPrefab, torpedoFirePoint.position, torpedoPosition.rotation); // Torpedo is shot at set torpedo point

        ControlTorpedoAnimation(false);
        yield return new WaitForSeconds(1.45f); // Wait for 1.45 seconds

        canShootTorpedo = true;
    }

    void ControlTorpedoAnimation(bool isShooting)
    {
        if (animator != null)
        {
            animator.SetBool("Shoot_Torpedo", isShooting);
        }
    }

    void SpawnTorpedoParticles()
    {
        if (spawnParticlePrefab != null)
        {
            Instantiate(spawnParticlePrefab, torpedoFirePoint.position, torpedoPosition.rotation);
        }
    }

    void SpawnSplashParticles()
    {
        if (splashParticlePrefab != null)
        {
            Instantiate(splashParticlePrefab, transform.position, Quaternion.identity);
            if (splashAudioSource != null)
            {
                splashAudioSource.Play();
            }
        }
    }

    void HandleFlameThrower()
    {
        if (Input.GetButton("Fire3") && FlameThrowerCharge > 0)
        {
            ActivateFlameThrower();
        }
        else
        {
            DeactivateFlameThrower();
        }
    }

    void ActivateFlameThrower()
    {
        if (!FlameThrowerActive)
        {
            FlamethrowerPrefab.SetActive(true);
            FlameThrowerActive = true;
            AudioSource.Play();
        }

        FlameThrowerCharge -= flameThrowerDrainRate * Time.deltaTime;
        if (gamemanager != null)
        {
            gamemanager.UpdateFlameThrowerChargeUI(FlameThrowerCharge);
        }

        if (FlameThrowerCharge <= 0)
        {
            DeactivateFlameThrower();
        }
    }

    void DeactivateFlameThrower()
    {
        FlamethrowerPrefab.SetActive(false);
        FlameThrowerActive = false;
        AudioSource.Stop();
    }

    void HandleShielding()
    {
        if (Input.GetButton("Shield") && ForceFieldCharge > 0)
        {
            ActivateShield();
        }
        else
        {
            DeactivateShield();
        }
    }

    void ActivateShield()
    {
        if (ForceField != null)
        {
            ForceField.gameObject.SetActive(true);
            ForceFieldActive = true;
            AudioShieldSource.Play();
        }

        if (ForceFieldCharge > 0)
        {
            if (ForceFieldCharge == 1)
            {
                CreateShockwave();
            }
            ForceFieldCharge--;
            if (gamemanager != null)
            {
                gamemanager.UpdateForceChargeUI(ForceFieldCharge);
            }
        }
    }

    void DeactivateShield()
    {
        ForceField.gameObject.SetActive(false);
        ForceFieldActive = false;
        AudioShieldSource.Stop();
    }

    void CreateShockwave()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Drone drone = nearbyObject.GetComponent<Drone>();
            if (drone != null)
            {
                Vector3 direction = nearbyObject.transform.position - transform.position;
                drone.PushBack(direction, shockwaveForce);
            }
            else
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = nearbyObject.transform.position - transform.position;
                    rb.AddForce(direction.normalized * shockwaveForce, ForceMode.Impulse);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleEnemyCollision(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleEnemyTrigger(other);
    }

    void HandleEnemyCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!ForceFieldActive)
            {
                healthScript.TakeDamage(25f); // 25 dmg
                Destroy(collision.gameObject);
            }
            else
            {
                StopEnemyMovement(collision.gameObject);
            }
        }
    }

    void HandleEnemyTrigger(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !ForceFieldActive)
        {
            healthScript.TakeDamage(25f); // 25 dmg
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Legs") && !ForceFieldActive)
        {
            healthScript.TakeDamage(15f); // 15 dmg
            Destroy(other.gameObject);
        }
    }

    void StopEnemyMovement(GameObject enemy)
    {
        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
        if (enemyRb != null)
        {
            enemyRb.linearVelocity = Vector3.zero;
            enemyRb.angularVelocity = Vector3.zero;
        }
    }
}

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

    [Header("ForceField:")]
    [SerializeField] private GameObject ForceField;
    public float ForceFieldCharge;
    private bool ForceFieldActive;
    public float shockwaveRadius = 5f; // Radius of the shockwave
    public float shockwaveForce = 10f; // Force applied to objects

    [Header("Base Laser Settings:")]
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
    public GameObject FlamethrowerPrefab;
    public float flameThrowerFireRate = 3;
    private float nextFlameThrowerFireTime = 0f;
    public bool canShootFlames = true;

    [Header("Animation:")]
    public Transform childWithAnimator; // Reference to the child object with the Animator component
    private Animator animator;

    void Start()
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

        ForceFieldCharge = gamemanager.maxForceCharge;
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleShielding();
    }

    void HandleMovement()
    {
        float moveX = 1; // X movement is constant to match the auto scrolling effect
        float moveY = Input.GetAxis("Vertical"); // W/S movement for up and down

        Vector2 movement = new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime;
        transform.Translate(movement.x, movement.y, 0, Space.World);
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

        if (Input.GetButton("Fire3") && Time.time >= nextFlameThrowerFireTime && canShootFlames)
        {
            Debug.Log("Flames");
            StartCoroutine(FlameThrowerDeploy());
            nextFlameThrowerFireTime = Time.time + flameThrowerFireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, bulletPosition.rotation); // Bullet is shot at set bullet point
    }

    IEnumerator DelayedShootTorpedo()
    {
        canShootTorpedo = false;
        // Control the swimming animation
        if (animator != null)
        {
            animator.SetBool("Shoot_Torpedo", true);
        }

        yield return new WaitForSeconds(1.45f); // Wait for 1.45 seconds
        Instantiate(torpedoPrefab, torpedoFirePoint.position, torpedoPosition.rotation); // Torpedo is shot at set torpedo point

        // Control the swimming animation
        if (animator != null)
        {
            animator.SetBool("Shoot_Torpedo", false);
        }
        yield return new WaitForSeconds(1.45f); // Wait for 1.45 seconds

        canShootTorpedo = true;
    }

    IEnumerator FlameThrowerDeploy()
    {
        canShootFlames = false;
        FlamethrowerPrefab.SetActive(true);
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        FlamethrowerPrefab.SetActive(false);
        canShootFlames = true;
    }

    void HandleShielding()
    {
        if (Input.GetButton("Shield") && ForceFieldCharge > 0)
        {
            Shielding();
        }
        else
        {
            ForceField.gameObject.SetActive(false);
            ForceFieldActive = false;
        }
    }

    void Shielding()
    {
        if (ForceField != null)
        {
            ForceField.gameObject.SetActive(true);
            ForceFieldActive = true;
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
        if (collision.gameObject.CompareTag("Enemy") && !ForceFieldActive)
        {
            healthScript.TakeDamage(25f); // 25 dmg
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy") && ForceFieldActive)
        {
            // Stop the movement of the enemy it's colliding with
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                enemyRb.linearVelocity = Vector3.zero;
                enemyRb.angularVelocity = Vector3.zero;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !ForceFieldActive)
        {
            healthScript.TakeDamage(25f); // 25 dmg
            Destroy(other.gameObject);
        }
    }
}

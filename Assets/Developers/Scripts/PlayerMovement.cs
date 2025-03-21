using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings:")]
    public float moveSpeed = 4f;
    private Health healthScript;

    [Header("Base Laser Settings:")]
    public GameObject bulletPrefab;
    public Transform bulletPosistion;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    [Header("Torpedo Setting:")]
    public GameObject torpedoPrefab;
    public Transform torepedoPosistion;
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

    void Update()
    {
        HandleMovement();
        HandleShooting();
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
            nextFireTime = Time.time + 0f + fireRate;
        }

        if (Input.GetButton("Fire2") && Time.time >= nextTorpedoFireTime && canShootTorpedo)
        {
            StartCoroutine(DelayedShootTorpedo());
            nextTorpedoFireTime = Time.time + 0f + torpedoFireRate;
        }

        if (Input.GetButton("Fire3") && Time.time >= nextFlameThrowerFireTime && canShootFlames)
        {
            Debug.Log("Flames");
            StartCoroutine(FlameThrowerDeploy());
            nextFlameThrowerFireTime = Time.time + 0f + flameThrowerFireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, bulletPosistion.rotation); // Bullet is shot at set bullet point
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
        Instantiate(torpedoPrefab, torpedoFirePoint.position, torepedoPosistion.rotation); // Torpedo is shot at set torpedo point

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
        canShootFlames=false;
        FlamethrowerPrefab.SetActive(true);
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        FlamethrowerPrefab.SetActive(false);
        canShootFlames = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            healthScript.TakeDamage(10); // 10 dmg
        }
    }
}

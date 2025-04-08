using UnityEngine;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;


public class SpiderBoss : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public Transform[] minigunBarrels; // The transforms of the minigun barrels
    public Transform[] firePoints; // Array of fire points
    public Transform[] eyeFirePoints; // Array of eye fire points
    public Transform[] legs; // Array of leg transforms
    public float spinSpeed = 100f; // Speed at which the minigun spins
    public float fireRate = 0.1f; // Time between shots
    public float eyeAttackInterval = 5f; // Interval for eye attacks
    public float legAttackInterval = 10f; // Interval for leg attacks
    public bool isShielding = false; // Boolean to trigger the shield
    public GameObject shieldPrefab;
    public float reloadWindowDuration = 2f; // Duration of the reload window
    public float minigunCooldownDuration = 3f; // Duration of the minigun cooldown

    private float nextFireTime = 0f;
    private float nextEyeAttackTime = 0f;
    private float nextLegAttackTime = 0f;
    private int currentLegIndex = 0; // Index of the leg to shoot next
    private Health healthScript;
    private float maxHealth = 1500f;
    private int legsShot = 0;
    private int legsShotPhase1 = 0;
    private int legsShotPhase2 = 0;
    private int legsShotPhase3 = 0;
    private bool phase2ShieldActivated = false;
    private bool phase3ShieldActivated = false;
    private bool isReloading = false;
    private bool isMinigunCoolingDown = false;

    private void Start()
    {
        healthScript = GetComponent<Health>();
        if (healthScript == null)
        {
            Debug.LogError("Health component not found on the Spider Boss.");
        }
        healthScript.maxHealth = maxHealth;
        healthScript.currentHealth = maxHealth;
        StartCoroutine(MinigunCooldown());
    }

    private void Update()
    {
        if (healthScript != null)
        {
            CheckPhase();
        }

        if (!isShielding && !isReloading && !isMinigunCoolingDown)
        {
            foreach (Transform minigunBarrel in minigunBarrels)
            {
                minigunBarrel.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
            }
            shieldPrefab.SetActive(false);
        }

        // Check if it's time to fire
        if (Time.time >= nextFireTime && !isShielding && !isReloading && !isMinigunCoolingDown)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // Check if it's time for the eye attack
        if (Time.time >= nextEyeAttackTime && !isShielding && !isReloading)
        {
            nextEyeAttackTime = Time.time + eyeAttackInterval;
            ShootEyes();
        }

        // Check if it's time for the leg attack
        if (Time.time >= nextLegAttackTime && !isShielding && !isReloading && legsShot < legs.Length)
        {
            nextLegAttackTime = Time.time + legAttackInterval;
            ShootLeg();
        }

        // Check if the shield should be activated
        if (isShielding)
        {
            Shield();
        }
    }

    private void CheckPhase()
    {
        if (healthScript.currentHealth > 1000)
        {
            // Phase 1: Full health to 1000 HP
            fireRate = 0.1f;
            spinSpeed = 100f;
            eyeAttackInterval = 14f;
            legAttackInterval = 10f;
        }
        else if (healthScript.currentHealth > 500)
        {
            // Phase 2: 1000 HP to 500 HP
            fireRate = 0.05f;
            spinSpeed = 150f;
            eyeAttackInterval = 8f;
            legAttackInterval = 8f;
            if (!phase2ShieldActivated)
            {
                isShielding = true;
                phase2ShieldActivated = true;
                StartCoroutine(DeactivateShieldAfterDelay(3f));
            }
        }
        else
        {
            // Phase 3: 500 HP to 0 HP
            fireRate = 0.025f;
            spinSpeed = 200f;
            eyeAttackInterval = 5f;
            legAttackInterval = 6f;
            if (!phase3ShieldActivated)
            {
                isShielding = true;
                phase3ShieldActivated = true;
                StartCoroutine(DeactivateShieldAfterDelay(5f));
            }
        }
    }

    private void Shoot()
    {
        foreach (Transform firePoint in firePoints)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private void ShootEyes()
    {
        foreach (Transform eyeFirePoint in eyeFirePoints)
        {
            EyeProjectile eyeProjectileComponent = eyeFirePoint.GetComponent<EyeProjectile>();
            if (eyeProjectileComponent != null)
            {
                eyeProjectileComponent.Activate(); // Activate the EyeProjectile component
            }
            else
            {
                Debug.LogError("EyeProjectile component not found on eyeFirePoint.");
            }
        }
    }

    private void ShootLeg()
    {
        if (legs.Length > 0 && legsShot < legs.Length)
        {
            if (healthScript.currentHealth > 1000 && legsShotPhase1 < 2)
            {
                legsShotPhase1++;
            }
            else if (healthScript.currentHealth > 500 && healthScript.currentHealth < 1000 && legsShotPhase2 < 3)
            {
                legsShotPhase2++;
            }
            else if (healthScript.currentHealth <= 500 && legsShotPhase3 < 3)
            {
                legsShotPhase3++;
            }
            else
            {
                return; // Do not shoot leg if the limit for the current phase is reached
            }

            Transform legToShoot = legs[currentLegIndex];

            // Set the isLeftLeg variable based on the current leg index
            bool isLeftLeg = (currentLegIndex < 4);

            // Increment the leg index for the next shot
            currentLegIndex = (currentLegIndex + 1) % legs.Length;
            legsShot++;

            legToShoot.parent = null;

            LegProjectile legProjectile = legToShoot.gameObject.AddComponent<LegProjectile>();
            legProjectile.Initialize();
            legProjectile.isLeftLeg = isLeftLeg;

            Debug.Log("Shot leg at index: " + (currentLegIndex - 1) % legs.Length);
        }
    }

    private void Shield()
    {
        shieldPrefab.SetActive(true);
    }

    private IEnumerator DeactivateShieldAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isShielding = false;
    }

    public IEnumerator MinigunCooldown()
    {
        while (true)
        {
            isMinigunCoolingDown = true;
            yield return new WaitForSeconds(reloadWindowDuration);
            isMinigunCoolingDown = false;
            yield return new WaitForSeconds(minigunCooldownDuration);
        }
    }

    private void OnDestroy()
    {
        if (legsShot < legs.Length)
        {
            ShootLeg(); // Shoot the last leg when the boss dies
        }

        SceneManager.LoadScene("WinScreen");
    }
}

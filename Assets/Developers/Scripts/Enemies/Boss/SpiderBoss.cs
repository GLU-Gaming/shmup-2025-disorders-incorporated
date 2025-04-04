using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public Transform[] minigunBarrels; // The transforms of the minigun barrels
    public Transform[] firePoints; // Array of fire points
    public Transform[] eyeFirePoints; // Array of eye fire points
    public Transform[] legs; // Array of leg transforms
    public float spinSpeed = 100f; // Speed at which the minigun spins
    public float fireRate = 0.1f; // Time between shots
    public bool isAttackingWithEyes = false; // Boolean to trigger the eye attack
    public bool isAttackingWithLegs = false; // Boolean to trigger the leg attack

    private float nextFireTime = 0f;
    private int currentLegIndex = 0; // Index of the leg to shoot next

    private void Update()
    {
        foreach (Transform minigunBarrel in minigunBarrels)
        {
            minigunBarrel.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
        }

        // Check if it's time to fire
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // Check if the eye attack is triggered
        if (isAttackingWithEyes)
        {
            isAttackingWithEyes = false;
            ShootEyes();
        }

        // Check if the leg attack is triggered
        if (isAttackingWithLegs)
        {
            isAttackingWithLegs = false;
            ShootLeg();
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
        if (legs.Length > 0)
        {
            Transform legToShoot = legs[currentLegIndex];

            // Set the isLeftLeg variable based on the current leg index
            bool isLeftLeg = (currentLegIndex < 4);

            // Increment the leg index for the next shot
            currentLegIndex = (currentLegIndex + 1) % legs.Length;

            legToShoot.parent = null;

            LegProjectile legProjectile = legToShoot.gameObject.AddComponent<LegProjectile>();
            legProjectile.Initialize();
            legProjectile.isLeftLeg = isLeftLeg;

            Debug.Log("Shot leg at index: " + (currentLegIndex - 1) % legs.Length);
        }
    }
}

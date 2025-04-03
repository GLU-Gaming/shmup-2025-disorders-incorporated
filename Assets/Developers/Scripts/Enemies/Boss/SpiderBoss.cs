using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public GameObject homingProjectilePrefab; // The homing projectile prefab to instantiate
    public Transform[] minigunBarrels; // The transforms of the minigun barrels
    public Transform[] firePoints; // Array of fire points
    public Transform[] eyeFirePoints; // Array of eye fire points
    public float spinSpeed = 100f; // Speed at which the minigun spins
    public float fireRate = 0.1f; // Time between shots
    public bool isAttackingWithEyes = false; // Boolean to trigger the eye attack

    private float nextFireTime = 0f;

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
            GameObject projectile = Instantiate(homingProjectilePrefab, eyeFirePoint.position, eyeFirePoint.rotation);
            HomingProjectile homingProjectile = projectile.GetComponent<HomingProjectile>();
            homingProjectile.player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}

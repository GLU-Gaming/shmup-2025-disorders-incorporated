using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public Transform minigunBarrel; // The transform of the minigun barrel
    public Transform[] firePoints; // Array of fire points
    public float spinSpeed = 100f; // Speed at which the minigun spins
    public float fireRate = 0.1f; // Time between shots

    private float nextFireTime = 0f;

    private void Start()
    {
        // Initialize fire points if not set in the inspector
        if (firePoints == null || firePoints.Length == 0)
        {
            firePoints = new Transform[6];
            for (int i = 0; i < 6; i++)
            {
                GameObject firePoint = new GameObject("FirePoint" + i);
                firePoint.transform.parent = minigunBarrel;
                firePoint.transform.localPosition = new Vector3(0, 0, 0.5f); // Adjust position as needed
                firePoint.transform.localRotation = Quaternion.Euler(0, 0, i * 60f); // Distribute evenly around the minigun
                firePoints[i] = firePoint.transform;
            }
        }
    }

    private void Update()
    {
        // Spin the minigun barrel around its local Z-axis
        minigunBarrel.Rotate(Vector3.left, spinSpeed * Time.deltaTime, Space.World);

        // Check if it's time to fire
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        foreach (Transform firePoint in firePoints)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}

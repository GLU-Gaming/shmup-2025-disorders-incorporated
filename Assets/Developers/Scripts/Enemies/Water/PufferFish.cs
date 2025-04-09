using UnityEngine;

public class PufferFish : WaterEnemy
{
    public float explosionRadius = 5f;
    public int explosionDamage = 50;
    public float explosionForce = 700f;
    public GameObject explosionEffect; // Optional: Reference to an explosion effect prefab
    public AudioSource AudioExplosion;

    protected override void Start()
    {
        Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);

        gameObject.transform.rotation = spawnRotation;
    }
    private void OnDestroy()
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") || hitCollider.CompareTag("Enemy"))
            {
                // Apply damage
                Health health = hitCollider.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(explosionDamage);
                }

                // Apply force to push away
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }

        // Optional: Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            AudioExplosion.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

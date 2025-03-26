using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 1.5f;
    public float damageCount;


    private Health healthScript;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float moveX = 1;
        float moveY = 0;
        rb.linearVelocity = new Vector2(moveX,moveY) * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            Destroy(gameObject);
            other.GetComponent<Health>().TakeDamage(damageCount);


        }
    }
}

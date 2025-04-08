using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 1.5f;
    public float damageCount;

    public bool isEnemy = true;
    public bool isTorpedo = false; 

    private Health healthScript;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float moveX = 1;
        float moveY = 0;
        if (!isEnemy)
        {
            rb.linearVelocity = new Vector2(moveX, moveY) * speed;
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveX, moveY) * speed;
        }
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("Bullet") && !isTorpedo)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Bullet") && isTorpedo)
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }

        if ((other.gameObject.CompareTag("Enemy") && !isEnemy) || (other.gameObject.CompareTag("Player") && isEnemy))
        {

            Destroy(gameObject);
            other.GetComponent<Health>().TakeDamage(damageCount);


        }
    }
}

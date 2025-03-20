using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 1.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float moveX = 1;
        float moveY = 0;
        rb.linearVelocity = new Vector2(moveX,moveY) * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            Destroy(gameObject); 
            /*
             Destroy the enemy or let the enemy take damage etc etc.
           
            */

        }
    }
}

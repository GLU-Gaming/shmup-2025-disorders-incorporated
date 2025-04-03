using UnityEngine;

public class Drone : FlyingEnemy
{
   

    public override void Attack()
    {
        // Implement Drone attack logic
        if (Projectile != null)
        {
            Instantiate(Projectile, transform.position, ProjectilePositioning.rotation);
            Debug.Log("Drone attacked!");
        }
    }

    public override void Move()
    {
        // Move to the left (negative X direction)
        transform.Translate(Vector3.back * speed * Time.deltaTime);

    }
}

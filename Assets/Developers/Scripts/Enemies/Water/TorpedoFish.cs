using UnityEngine;

public class TorpedoFish : FlyingEnemy
{
    public override void Attack()
    {
        // Implement the attack logic here
        if (Projectile != null)
        {
            Instantiate(Projectile, transform.position, ProjectilePositioning.rotation);
            Debug.Log("TorpedoFish attacked!");
        }
    }

    public override void Move()
    {
        // Move in the direction the fish is facing
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}

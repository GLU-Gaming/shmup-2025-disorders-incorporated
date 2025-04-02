using UnityEngine;

public class Drone : FlyingEnemy
{
    private GameObject Player;

    protected override void Start()
    {
        base.Start();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

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

        if (transform.position.x < Player.transform.position.x - 40)
        {
            transform.position = new Vector3(Player.transform.position.x + 50, startPosition.y, transform.position.z);
        }
    }
}

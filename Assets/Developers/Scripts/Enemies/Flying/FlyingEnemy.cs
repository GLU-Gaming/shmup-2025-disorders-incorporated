using UnityEngine;
using System.Collections;

public abstract class FlyingEnemy : AbstractEnemy
{
   
    public float attackInterval = 2f; // Interval between attacks in seconds
    public GameObject Projectile;
    public Transform ProjectilePositioning;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackRoutine()); // Start the attack routine
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval); // Wait for the attack interval
            Attack(); // Perform the attack
        }
    }
}

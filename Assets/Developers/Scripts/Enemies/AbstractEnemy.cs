using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour
{
    public int health;
    public float speed;

    public abstract void Attack();
    public abstract void Move();
}

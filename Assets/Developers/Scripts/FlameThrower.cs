using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    private Health healthScript;

  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(2f);
        }
       
    }
}

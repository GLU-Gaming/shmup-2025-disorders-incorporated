using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    private Health healthScript;

  
    private void OnTriggerStay(Collider other)
    {
       other.GetComponent<Health>().TakeDamage(2f);
    }
}

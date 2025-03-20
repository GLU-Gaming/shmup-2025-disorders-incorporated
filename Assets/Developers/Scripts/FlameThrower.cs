using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.Playables;

public class FlameThrower : MonoBehaviour
{
    private Health healthScript;

  
    private void OnTriggerStay(Collider other)
    {
       other.GetComponent<Health>().TakeDamage(1.5f);
    }
}

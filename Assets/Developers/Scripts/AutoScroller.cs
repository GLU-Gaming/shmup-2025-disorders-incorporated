using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class AutoScroller : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    
    void Update()
    {
        float moveY = 0;
        float moveX = 1;
        Vector2 movement = new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}

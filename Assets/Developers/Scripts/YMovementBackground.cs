using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of movement
    public float moveHeight = 1f; // Maximum height increase
    public ParticleSystem[] particleEffects; // Array of particle systems to trigger

    private Vector3 startPos;
    private bool particlesTriggered = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Abs(Mathf.Sin(Time.time * moveSpeed)) * moveHeight;
        transform.position = new Vector3(transform.position.x, startPos.y + offset, transform.position.z);

        if (!particlesTriggered && offset >= moveHeight * 0.05f)
        {
            TriggerParticles();
            particlesTriggered = true;
        }
        else if (offset < moveHeight * 0.5f)
        {
            particlesTriggered = false;
        }
    }

    void TriggerParticles()
    {
        foreach (var particle in particleEffects)
        {
            if (particle != null)
            {
                particle.Play();
            }
        }
    }
}

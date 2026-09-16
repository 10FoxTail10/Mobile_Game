using UnityEngine;

public class PickupEffect : MonoBehaviour
{
    private ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        Destroy(gameObject, particles.main.duration);
    }
}
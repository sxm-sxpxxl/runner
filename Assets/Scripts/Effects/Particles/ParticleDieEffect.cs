using UnityEngine;

[RequireComponent(typeof(MortalObject))]
public class ParticleDieEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab = default;

    private void Awake()
    {
        GetComponent<MortalObject>().Die += OnDie;
    }

    private void OnDie()
    {
        ParticleSystem particleSystem = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
    }
}
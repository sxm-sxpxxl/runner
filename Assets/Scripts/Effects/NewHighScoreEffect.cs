using UnityEngine;

[RequireComponent(typeof(DistanceDeterminator))]
public class NewHighScoreEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystemPrefab = default;

    private void Awake()
    {
        GetComponent<DistanceDeterminator>().OnNewHighScoreObtained += OnNewHighScoreObtained;
    }

    private void OnNewHighScoreObtained()
    {
        ParticleSystem instance = Instantiate(particleSystemPrefab, Vector3.zero, Quaternion.identity);
        Destroy(instance.gameObject, instance.main.duration);
    }
}
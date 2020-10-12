using UnityEngine;

[RequireComponent(typeof(DistanceDeterminator))]
public class AudioNewHighScoreEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioPrefab = default;

    private void Awake()
    {
        GetComponent<DistanceDeterminator>().OnNewHighScoreObtained += OnNewHighScoreObtained;
    }

    private void OnNewHighScoreObtained()
    {
        AudioSource audio = Instantiate(audioPrefab, Vector3.zero, Quaternion.identity);
        audio.Play();

        Destroy(audio.gameObject, audio.clip.length);
    }
}
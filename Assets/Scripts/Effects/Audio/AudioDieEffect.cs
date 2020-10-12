using UnityEngine;

[RequireComponent(typeof(MortalObject))]
public class AudioDieEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioPrefab = default;

    private void Awake()
    {
        GetComponent<MortalObject>().Die += OnDie;
    }

    private void OnDie()
    {
        AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        audio.Play();

        Destroy(audio.gameObject, audio.clip.length);
    }
}
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField, Min(1)] private int damageAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        player?.TakeDamage(damageAmount);

        Destroy(gameObject);
    }
}
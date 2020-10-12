using UnityEngine;

public class Obstacle : MortalObject
{
    [SerializeField, Min(1)] private int damageAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        player?.TakeDamage(damageAmount);

        OnDie();
        Destroy(gameObject);
    }
}
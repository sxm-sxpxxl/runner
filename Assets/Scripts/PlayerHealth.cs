using System;

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Range(5, 10)] private int maxHealth = 5;

    public event Action<int, int> OnHealthChange = delegate { };
    public event Action OnDie = delegate { };

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChange?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
            Destroy(gameObject);
        }
    }
}
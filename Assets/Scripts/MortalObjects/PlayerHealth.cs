using System;

using UnityEngine;

public class PlayerHealth : MortalObject
{
    [SerializeField, Range(5, 10)] private int maxHealth = 5;

    public event Action<int, int> OnHealthChange = delegate { };

    private int currentHealth;

    private int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            OnHealthChange?.Invoke(currentHealth, maxHealth);
        }
    }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            OnDie();
            Destroy(gameObject);
        }
    }
}
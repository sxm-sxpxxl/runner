using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth = default;

    private Text targetText;

    private void Awake()
    {
        targetText = GetComponent<Text>();
        playerHealth.OnHealthChange += OnHealthChange;
        playerHealth.OnDie += OnDie;
    }

    private void OnDie()
    {
        targetText.text = $"DIE";
    }

    private void OnHealthChange(int currentHealth, int maxHealth)
    {
        targetText.text = $"{currentHealth}/{maxHealth}";
    }
}
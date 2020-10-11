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
    }

    private void OnHealthChange(int currentHealth, int maxHealth)
    {
        targetText.text = $"{currentHealth}/{maxHealth}";
    }
}
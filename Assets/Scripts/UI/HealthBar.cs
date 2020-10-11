using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform heartPrefab = default;
    [SerializeField] private PlayerHealth playerHealth = default;

    [SerializeField] private Sprite availableHeartSprite = default;
    [SerializeField] private Sprite unAvailableHeartSprite = default;

    private readonly Color availableHeartColor = Color.white, unAvailableHeartColor = new Color(1f, 1f, 1f, 0.5f);
    private int lastMaxHealth;

    private void Awake()
    {
        playerHealth.OnHealthChange += OnHealthChange;
    }

    private void Start()
    {
        CreateHealth(lastMaxHealth);
    }

    private void CreateHealth(int maxHealth)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < maxHealth; i++)
        {
            Instantiate(heartPrefab, transform);
        }
    }

    private void SetCurrentHealth(int health)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Image image = transform.GetChild(i).GetComponent<Image>();
            image.sprite = i < health ? availableHeartSprite : unAvailableHeartSprite;
            image.color = i < health ? availableHeartColor : unAvailableHeartColor;
        }
    }

    private void OnHealthChange(int currentHealth, int maxHealth)
    {
        if (lastMaxHealth != maxHealth)
        {
            CreateHealth(maxHealth);
            lastMaxHealth = maxHealth;
        }

        SetCurrentHealth(currentHealth);
    }
}
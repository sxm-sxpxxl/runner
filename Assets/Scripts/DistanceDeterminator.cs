using System;

using UnityEngine;

public class DistanceDeterminator : MonoBehaviour
{
    [SerializeField] private TileGenerator tileGenerator = default;
    [SerializeField] private PlayerHealth playerHealth = default;

    public event Action<string> OnCurrentDistanceChanged = delegate { };
    public event Action<string> OnHighDistanceChanged = delegate { };

    private readonly string highTraveledDistanceKey = "highTraveledDistanceKey";

    private float currentTraveledDistance, highTraveledDistance;

    private float CurrentTraveledDistance
    {
        get => currentTraveledDistance;
        set
        {
            currentTraveledDistance = value;
            OnCurrentDistanceChanged?.Invoke(FormatDistance(currentTraveledDistance));
        }
    }

    private float HighTraveledDistance
    {
        get => highTraveledDistance;
        set
        {
            highTraveledDistance = value;
            OnHighDistanceChanged?.Invoke(FormatDistance(highTraveledDistance));
            PlayerPrefs.SetFloat(highTraveledDistanceKey, highTraveledDistance);
        }
    }

    private void Awake()
    {
        if (tileGenerator != null) tileGenerator.OnDistanceTranslate += OnDistanceTranslate;
        if (playerHealth != null) playerHealth.OnDie += OnPlayerDie;
    }

    private void Start()
    {
        HighTraveledDistance = PlayerPrefs.GetFloat(highTraveledDistanceKey, 0f);
    }

    private void OnDistanceTranslate(float distance)
    {
        CurrentTraveledDistance += distance;

        if (CurrentTraveledDistance > HighTraveledDistance)
        {
            HighTraveledDistance = CurrentTraveledDistance;
        }
    }

    private void OnPlayerDie()
    {
        HighTraveledDistance = highTraveledDistance;
    }

    private string FormatDistance(float value) => $"{value:F} m";
}
using System;

using UnityEngine;

public class DistanceDeterminator : MonoBehaviour
{
    [SerializeField] private TileMotionController tileMotionController = default;
    [SerializeField] private PlayerHealth playerHealth = default;

    public event Action<string> OnCurrentDistanceChanged = delegate { };
    public event Action<string> OnHighDistanceChanged = delegate { };
    public event Action OnNewHighScoreObtained = delegate { };

    private readonly string highTraveledDistanceKey = "highTraveledDistanceKey";

    private float currentTraveledDistance, highTraveledDistance;

    private bool isNewHighScoreObtained;

    private void Awake()
    {
        if (tileMotionController != null) tileMotionController.OnDistanceAdd += OnDistanceAdd;
        if (playerHealth != null) playerHealth.Die += OnPlayerDestroy;
    }

    private void Start()
    {
        UpdateHighTraveledDistance(PlayerPrefs.GetFloat(highTraveledDistanceKey, 0f));
    }

    private void OnDistanceAdd(float distance)
    {
        currentTraveledDistance += distance;
        OnCurrentDistanceChanged?.Invoke(FormatDistance(currentTraveledDistance));

        if (currentTraveledDistance > highTraveledDistance)
        {
            UpdateHighTraveledDistance(currentTraveledDistance);
            if (isNewHighScoreObtained) return;

            OnNewHighScoreObtained?.Invoke();
            isNewHighScoreObtained = true;
        }
    }

    private void OnPlayerDestroy()
    {
        UpdateHighTraveledDistance(highTraveledDistance);
        tileMotionController.OnDistanceAdd -= OnDistanceAdd;
    }

    private void UpdateHighTraveledDistance(float value)
    {
        highTraveledDistance = value;
        OnHighDistanceChanged?.Invoke(FormatDistance(value));
        PlayerPrefs.SetFloat(highTraveledDistanceKey, value);
    }

    private string FormatDistance(float value) => $"{value:F} m";
}
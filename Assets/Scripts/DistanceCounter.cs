using UnityEngine;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    [SerializeField] private Text currentTraveledDistanceText = default;
    [SerializeField] private Text highTraveledDistanceText = default;

    private readonly string highTraveledDistanceKey = "highTraveledDistanceKey";

    private float currentTraveledDistance, highTraveledDistance;

    private float CurrentTraveledDistance
    {
        get => currentTraveledDistance;
        set
        {
            currentTraveledDistance = value;
            currentTraveledDistanceText.text = FormatDistance(value);
        }
    }

    private float HighTraveledDistance
    {
        get => highTraveledDistance;
        set
        {
            highTraveledDistance = value;
            highTraveledDistanceText.text = FormatDistance(value);
            PlayerPrefs.SetFloat(highTraveledDistanceKey, HighTraveledDistance);
        }
    }

    private void Awake()
    {
        GetComponent<TileGenerator>().OnDistanceTranslate += OnDistanceTranslate;
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

    private string FormatDistance(float value) => $"{value:F} m";
}
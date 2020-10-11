using UnityEngine;
using UnityEngine.UI;

public class DistanceBar : MonoBehaviour
{
    [SerializeField] protected DistanceDeterminator distanceCounter = default;

    protected Text targetText;

    protected virtual void Awake()
    {
        targetText = GetComponent<Text>();
    }
}
public class HighDistanceBar : DistanceBar
{
    protected override void Awake()
    {
        base.Awake();
        distanceCounter.OnHighDistanceChanged += OnHighDistanceChanged;
    }

    private void OnHighDistanceChanged(string value)
    {
        targetText.text = value;
    }
}
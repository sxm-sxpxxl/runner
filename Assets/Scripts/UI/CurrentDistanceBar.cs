public class CurrentDistanceBar : DistanceBar
{
    protected override void Awake()
    {
        base.Awake();
        distanceCounter.OnCurrentDistanceChanged += OnCurrentDistanceChanged;
    }

    private void OnCurrentDistanceChanged(string value)
    {
        targetText.text = value;
    }
}
using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab = default;
    [SerializeField, Range(0.25f, 1f)] private float durationMovement = 0.5f;
    [SerializeField, Range(0f, 360f)] private float rotationSpeed = 180f;

    private Bounds tileBounds;

    private Vector3 inputDirection;
    private Vector3 lastPosition, nextPosition;
    private bool isMovement;
    private float lerpWeight, rate;

    private void Awake()
    {
        lastPosition = nextPosition = transform.position;
        rate = 1f / durationMovement;
        tileBounds = BoundsDeterminator.Determine(tilePrefab);
        GetComponent<SwipeDetector>().OnSwipe += OnSwipe;
    }

    private void Update()
    {
        HandleInput();
        UpdateMotion();
        UpdateAutonomousRotation();
    }

    private void OnSwipe(SwipeDirection direction)
    {
        if (direction == SwipeDirection.Left || direction == SwipeDirection.Right)
        {
            int sign = direction == SwipeDirection.Right ? 1 : -1;
            inputDirection = Vector3.right * sign;
        }
    }

    private void HandleInput()
    {
        if (inputDirection.sqrMagnitude < 0.01f) return;

        if (isMovement)
        {
            bool isDirectionNotChanged = Vector3.Dot(inputDirection, nextPosition - lastPosition) > 0f;
            if (isDirectionNotChanged) return;

            Vector3 temp = lastPosition;
            lastPosition = nextPosition;
            nextPosition = temp;
            lerpWeight = 1f - lerpWeight;
        }
        else
        {
            Vector3 newPosition = nextPosition + inputDirection * tileBounds.size.x / tilePrefab.childCount;
            if (!tileBounds.Contains(Vector3.right * newPosition.x)) return;

            lastPosition = nextPosition;
            nextPosition = newPosition;
            isMovement = true;
        }

        inputDirection = Vector3.zero;
    }

    private void UpdateMotion()
    {
        if (!isMovement) return;

        lerpWeight += rate * Time.deltaTime;
        transform.position = Vector3.Slerp(lastPosition, nextPosition, lerpWeight);

        if (lerpWeight > 1f)
        {
            isMovement = false;
            lerpWeight = 0f;
        }
    }

    private void UpdateAutonomousRotation()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
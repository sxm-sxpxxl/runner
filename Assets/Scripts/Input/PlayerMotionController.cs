using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotionController : MonoBehaviour
{
    private const float FullRotationDegrees = 360f;
    
    [SerializeField] private Transform tilePrefab = default;
    [SerializeField, Range(0.1f, 1f)] private float shiftDuration = 0.5f;
    [SerializeField] private AnimationCurve shiftCurve = default;
    [Space]
    [SerializeField] private Transform rotationTarget;
    [SerializeField, Range(0f, 10f)] private float rotationSpeed = 10f;

    private Bounds tileBounds;

    private Vector3 inputDirection;
    private Vector3 lastPosition, nextPosition;
    private bool isShifting;
    private float lerpWeight;

    private void Awake()
    {
        lastPosition = nextPosition = transform.position;
        tileBounds = BoundsDeterminator.Determine(tilePrefab);
    }

    private void Update()
    {
        UpdateNextPosition();
        UpdateMotion();
        UpdateAutonomousRotation();
    }

    public void HandleMoveInput(InputAction.CallbackContext context)
    {
        int direction = (int) Mathf.Clamp(Mathf.Round(context.ReadValue<Vector2>().x), -1f , 1f);

        Mouse mouse = Mouse.current;
        if (mouse.name == context.control.device.name && mouse.leftButton.isPressed == false)
        {
            direction = 0;
        }

        inputDirection = Vector3.right * direction;
    }

    private void UpdateNextPosition()
    {
        if (inputDirection.sqrMagnitude < 0.01f) return;

        if (isShifting)
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
            isShifting = true;
        }

        inputDirection = Vector3.zero;
    }

    private void UpdateMotion()
    {
        if (!isShifting) return;

        float rate = 1f / shiftDuration;
        lerpWeight += rate * Time.deltaTime;
        transform.position = Vector3.Slerp(lastPosition, nextPosition, shiftCurve.Evaluate(lerpWeight));

        if (lerpWeight > 1f)
        {
            isShifting = false;
            lerpWeight = 0f;
        }
    }

    private void UpdateAutonomousRotation()
    {
        rotationTarget.Rotate(FullRotationDegrees * rotationSpeed * Time.deltaTime * Vector3.right);
    }
}
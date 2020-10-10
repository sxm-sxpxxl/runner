using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab = default;
    [SerializeField, Range(0.25f, 1f)] private float durationMovement = 0.5f;

    private Bounds tileBounds;

    private Vector3 lastPosition, nextPosition;
    private bool isMovement;
    private float lerpWeight, rate;

    private void Awake()
    {
        lastPosition = nextPosition = transform.position;
        rate = 1f / durationMovement;
        tileBounds = BoundsDeterminator.Determine(tilePrefab);
    }

    private void Update()
    {
        HandleInput();
        UpdateMotion();
    }

    private void HandleInput()
    {
        float input = Input.GetAxisRaw("Horizontal");
        bool isInputExist = !Mathf.Approximately(input, 0f);

        if (!isInputExist) return;

        Vector3 inputDirection = Vector3.right * input;
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
}
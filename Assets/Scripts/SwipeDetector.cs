using System;

using UnityEngine;

public enum SwipeDirection { Up, Down, Left, Right }

public enum MouseButton { Left = 0, Right = 1 }

public class SwipeDetector : MonoBehaviour
{
    [SerializeField, Range(20f, 50f)] private float minDistanceForSwipe = 30f;
    [SerializeField] private bool detectSwipeOnlyAfterRelease = true;
    [SerializeField] private MouseButton simulateTouchesMouseButton = MouseButton.Left;

    public event Action<SwipeDirection> OnSwipe = delegate { };

    private Vector2 touchDownPosition, touchUpPosition;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleMouseInput()
    {
        int mouseButton = (int) simulateTouchesMouseButton;

        if (Input.GetMouseButtonDown(mouseButton))
        {
            touchDownPosition = touchUpPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(mouseButton) || !detectSwipeOnlyAfterRelease)
        {
            touchDownPosition = Input.mousePosition;
            DetectSwipe();
        }
    }

    private void HandleTouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                touchDownPosition = touchUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended || (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved))
            {
                touchDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        float verticalMovementDistance = Mathf.Abs(touchDownPosition.y - touchUpPosition.y);
        float horizontalMovementDistance = Mathf.Abs(touchDownPosition.x - touchUpPosition.x);

        if (verticalMovementDistance < minDistanceForSwipe && horizontalMovementDistance < minDistanceForSwipe) return;

        SwipeDirection direction;
        if (verticalMovementDistance > horizontalMovementDistance)
        {
            direction = touchDownPosition.y - touchUpPosition.y > 0f ?
                SwipeDirection.Up :
                SwipeDirection.Down;
        }
        else
        {
            direction = touchDownPosition.x - touchUpPosition.x > 0f ?
                SwipeDirection.Right :
                SwipeDirection.Left;
        }

        OnSwipe(direction);
        touchUpPosition = touchDownPosition;
    }
}
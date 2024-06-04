using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private float _tresholdDistance = 300f;
    [SerializeField] private float _rotationSpeed = 5f;

    private enum RotationState { North, East, South, West }
    private RotationState _currentState = RotationState.North;

    private float _startX;
    private bool _isDragging;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            HandleTouch(touch);
        }

        RotateFigure();
    }

    private void HandleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _startX = touch.position.x;
                _isDragging = true;
                break;
            case TouchPhase.Moved:
                if (_isDragging)
                {
                    float deltaX = touch.position.x - _startX;
                    if (Mathf.Abs(deltaX) > _tresholdDistance)
                    {
                        UpdateRotationState(deltaX);
                        _startX = touch.position.x;
                    }
                }
                break;
            case TouchPhase.Ended:
                _isDragging = false;
                break;
        }
    }

    private void UpdateRotationState(float deltaX)
    {
        if (deltaX > 0)
        {
            _currentState = (RotationState)(((int)_currentState + 1) % 4);
        }
        else
        {
            _currentState = (RotationState)(((int)_currentState - 1 + 4) % 4);
        }
    }

    private void RotateFigure()
    {
        float targetRotation = GetTargetRotation();
        float step = _rotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0f, Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, step), 180f);
    }

    private float GetTargetRotation()
    {
        switch (_currentState)
        {
            case RotationState.North:
                return 0f;
            case RotationState.East:
                return 90f;
            case RotationState.South:
                return 180f;
            case RotationState.West:
                return 270f;
            default:
                return 0f;
        }
    }
}
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private float _tresholdDistance = 300f;
    [SerializeField] private float _rotationSpeed = 5f;
    
    private float _targetRotation;
    private float _startX;
    private bool _isDragging;

    private void OnEnable()
    {
        WinZone.OnPlayerArrived += DeactivateThis;
        UIManager.OnGameStarted += ActivateThis;
    }

    private void OnDisable()
    {
        WinZone.OnPlayerArrived -= DeactivateThis;
        UIManager.OnGameStarted -= ActivateThis;
    }

    private void DeactivateThis()
    {
        enabled = false;
    }
    
    private void ActivateThis()
    {
        enabled = true;
    }
    
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
                        _isDragging = false;
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
            _targetRotation += 90f;
        }
        else
        {
            _targetRotation -= 90f;
        }
    }

    private void RotateFigure()
    {
        float step = _rotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0f, Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, step), 0);
    }
}
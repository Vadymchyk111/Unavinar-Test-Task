using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _minSpeed;
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isAccelerating;

    private void Start()
    {
        _rigidbody.centerOfMass = Vector3.zero;
        _rigidbody.inertiaTensorRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }
        
        Touch touch = Input.touches[0];
        _isAccelerating = touch.phase switch
        {
            TouchPhase.Began => true,
            TouchPhase.Ended => false,
            _ => _isAccelerating
        };
    }

    private void FixedUpdate()
    {
        _speed = _isAccelerating ? Mathf.Min(_acceleration * Time.fixedDeltaTime + _speed, _maxSpeed)  : Mathf.Max(_speed - _acceleration * Time.fixedDeltaTime, _minSpeed);
        _rigidbody.velocity = Vector3.forward * _speed;
    }
}
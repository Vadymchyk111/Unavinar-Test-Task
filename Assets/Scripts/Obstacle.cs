using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public void SetRigidBodyDynamic()
    {
        _rigidbody.isKinematic = false;
    }
}
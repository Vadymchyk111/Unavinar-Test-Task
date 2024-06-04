using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            return;
        }
        
        obstacle.SetRigidBodyDynamic();
        Destroy(obstacle.gameObject, 2f);
    }
}

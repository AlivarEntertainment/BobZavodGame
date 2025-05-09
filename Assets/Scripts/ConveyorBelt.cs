using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 2.0f;
    public Vector3 direction = Vector3.forward; // направление вдоль Z

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            // Передаем движение объекту на ленте
            rb.linearVelocity = direction.normalized * speed;
        }
    }
}

using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;   
    public Vector3 offset = new Vector3(0, 5, -8);
    public float smoothSpeed = 10f; // SmoothSpeed biraz artırıldı

    private Vector3 velocity = Vector3.zero; // yeni: hız vektörü

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Daha yumuşak ve stabil hareket için SmoothDamp kullanıyoruz:
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / smoothSpeed);

        transform.LookAt(target);
    }
}

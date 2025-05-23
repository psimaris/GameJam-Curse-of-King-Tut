using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;    
    public Vector3 offset = new Vector3(0f, 1.6f, -1f);
    public float smoothSpeed = 0.125f;

    void LateUpdate() {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}

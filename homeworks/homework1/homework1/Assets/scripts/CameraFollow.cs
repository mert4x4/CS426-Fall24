using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // The object to follow
    public Vector3 offset;     // Offset distance between the camera and target

    void LateUpdate()
    {
        // Set the camera position to the target's position plus the offset
        transform.position = target.position + offset;
    }
}

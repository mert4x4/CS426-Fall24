using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;  // The player's transform to follow
    public Vector3 offset = new Vector3(0, 10, -7);  // Adjusted offset for better view
    public bool useOrthographic = true;  // Option to use orthographic view

    private void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Keep the camera at a fixed offset relative to the player
        transform.position = playerTransform.position + offset;

        // Look down at the player
        transform.LookAt(playerTransform);
    }
}

using UnityEngine;

public class MoveableObjectController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float maxVelocity = 5f;  // Maximum allowed velocity
    private Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from the player
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Apply force to the Rigidbody
        rb.AddForce(movement * moveSpeed);

        // Limit the maximum velocity of the object
        if (rb.velocity.magnitude > maxVelocity)
        {
            // Clamp the velocity if it exceeds the max velocity
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}

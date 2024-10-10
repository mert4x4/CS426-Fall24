using UnityEngine;

public class ConstantForce : MonoBehaviour
{
    public float forceAmount = 0.2f;  // Amount of force to apply to the Rigidbody
    public float maxVelocity = 2f;    // Maximum velocity allowed for the object
    private Rigidbody rb;             // Reference to the object's Rigidbody component

    void Start()
    {
        // Acquire the Rigidbody component attached to this object
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Apply a constant horizontal force to the Rigidbody
        rb.AddForce(Vector3.right * forceAmount);

        // Check if the velocity exceeds the maxVelocity, and if so, clamp it
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}

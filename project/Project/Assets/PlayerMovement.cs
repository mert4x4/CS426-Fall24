using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed for horizontal movement
    public float jumpForce = 5f;  // Force applied for jumping
    public float dashSpeed = 20f; // Speed during dash
    public float dashDuration = 0.2f; // Duration of the dash
    public int maxJumps = 2; // Maximum number of jumps (for double jump)

    private Rigidbody rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDashing)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 move = new Vector3(moveX, rb.velocity.y, moveZ);
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset vertical velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // Reset jump count when on the ground
        }
    }

    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        Vector3 dashDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward; // Default forward dash if no input
        }

        float originalDrag = rb.drag; // Save original drag to avoid friction during dash
        rb.drag = 0; // Set drag to 0 for smooth dashing

        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.drag = originalDrag; // Restore original drag
        isDashing = false;
    }
}

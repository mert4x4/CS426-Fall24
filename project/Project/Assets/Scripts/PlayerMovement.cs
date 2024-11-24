using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.35f;
    public float slideSpeed = 5f;  // Speed for sliding down
    public int maxJumps = 2;

    private int currentMaxJumps;
    public int jumpCount { get; private set; }

    private Rigidbody rb;
    private bool isGrounded;
    private bool isDashing = false;
    private bool isSliding = false;  // Tracks if the player is sliding

    private Renderer playerRenderer;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }
        currentMaxJumps = maxJumps;
    }

    void Update()
    {
        if (!isDashing && !isSliding)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < currentMaxJumps)
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

        if (!isSliding)  // Allow movement only when not sliding
        {
            rb.velocity = move;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;
        Debug.Log($"Jumped! Current jump count: {jumpCount}/{currentMaxJumps}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            currentMaxJumps = maxJumps;
            isSliding = false;  // Stop sliding when grounded
            Debug.Log($"Landed on ground. Jump count reset. Current max jumps: {currentMaxJumps}");
        }

        HandleSideCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            HandleSideCollision(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isSliding = false;  // Stop sliding when no longer colliding
            Debug.Log("Stopped sliding due to exiting collision.");
        }
    }

    private void HandleSideCollision(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 normal = contact.normal;

            // Check for side collisions (horizontal collision normal)
            if (Mathf.Abs(normal.x) > 0.5f || Mathf.Abs(normal.z) > 0.5f)
            {
                Debug.Log("Side collision detected. Applying sliding effect.");
                isSliding = true;

                // Apply downward velocity
                rb.velocity = new Vector3(0, -slideSpeed, 0);
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Coin detected.");

            if (isDashing)
            {
                CollectCoin();
            }
            else
            {
                Debug.Log("Player touched coin, but was not dashing.");
            }

            Destroy(other.gameObject);  // Destroy the coin after collection
        }
    }

    private void CollectCoin()
    {
        currentMaxJumps++;
        Debug.Log($"Coin collected while dashing! Extra jump granted. Current max jumps: {currentMaxJumps}");
    }

    public void ResetMaxJumps()
    {
        currentMaxJumps = maxJumps;
        Debug.Log("Max jumps reset to default.");
    }

    public void ResetJumpCount()
    {
        jumpCount = 0;
        Debug.Log("Jump count reset.");
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        // Change color to indicate dashing
        if (playerRenderer != null)
        {
            playerRenderer.material.color = Color.red;
        }

        Vector3 dashDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        float originalDrag = rb.drag;
        rb.drag = 0;

        rb.velocity = dashDirection * dashSpeed;

        Debug.Log($"Dashing in direction: {dashDirection}");

        yield return new WaitForSeconds(dashDuration);

        rb.drag = originalDrag;
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.3f);

        // Reset color after dash
        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }
        isDashing = false;
        Debug.Log("Dash ended.");
    }
}

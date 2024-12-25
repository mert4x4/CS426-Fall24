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

    // Particle systems for coin collection effects
    public ParticleSystem coinCollectionParticles;         // For collecting a coin while dashing
    public ParticleSystem normalCoinCollectionParticles;   // For collecting a coin without dashing

    // Scoring system
    private int score = 0;       // Player's current score
    private float scoreMultiplier = 1f;  // Score multiplier
    
    
    private bool canDash = true; // Indicates if the player can dash in the air

    
    public int GetScore()
{
    return score;
}

public float GetMultiplier()
{
    return scoreMultiplier;
}

    public void SetScore(int newScore)
{
    score = newScore;
    Debug.Log($"Score has been set to: {score}");
}

public void SetMultiplier(float newMultiplier)
{
    scoreMultiplier = newMultiplier;
    Debug.Log($"Multiplier has been set to: {scoreMultiplier:F2}");
}

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }
        currentMaxJumps = maxJumps;

        // Ensure the particle systems are not active initially
        if (coinCollectionParticles != null)
        {
            coinCollectionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else
        {
            Debug.LogError("Coin collection particle system for dashing is not assigned in the Inspector.");
        }

        if (normalCoinCollectionParticles != null)
        {
            normalCoinCollectionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else
        {
            Debug.LogError("Coin collection particle system for normal collection is not assigned in the Inspector.");
        }
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

if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isGrounded && canDash)
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
        canDash = true;     // Reset dash ability upon landing
        Debug.Log($"Landed on ground. Jump count reset. Dash reset.");
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
        isGrounded = false;
        Debug.Log("Left platform, dash available.");
    }
}


    private void HandleSideCollision(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Coin detected.");

            if (isDashing)
            {
                CollectCoinWithDashParticles(other.transform.position);
            }
            else
            {
                CollectCoinWithNormalParticles(other.transform.position);
            }

            Destroy(other.gameObject);  // Destroy the coin after collection
        }
    }

    private void CollectCoinWithDashParticles(Vector3 coinPosition)
    {
        currentMaxJumps++;
        scoreMultiplier += 1f;  // Increase multiplier
        AddScore(1);  // Add base score

        Debug.Log($"Coin collected while dashing! Extra jump granted. Current max jumps: {currentMaxJumps}, Score: {score}, Multiplier: {scoreMultiplier:F2}");

        // Play the dash particle effect
        if (coinCollectionParticles != null)
        {
            coinCollectionParticles.transform.position = coinPosition;
            coinCollectionParticles.Play();
            Debug.Log("Dash coin collection particles activated.");
        }
        else
        {
            Debug.LogWarning("Dash coin collection particle system is not assigned.");
        }
    }

    private void CollectCoinWithNormalParticles(Vector3 coinPosition)
    {
        scoreMultiplier = 1f;  // Reset multiplier
        AddScore(1);  // Add base score

        Debug.Log($"Coin collected normally! Score: {score}, Multiplier reset to {scoreMultiplier:F2}");

        // Play the normal particle effect
        if (normalCoinCollectionParticles != null)
        {
            normalCoinCollectionParticles.transform.position = coinPosition;
            normalCoinCollectionParticles.Play();
            Debug.Log("Normal coin collection particles activated.");
        }
        else
        {
            Debug.LogWarning("Normal coin collection particle system is not assigned.");
        }
    }

    private void AddScore(int baseScore)
    {
        score += Mathf.RoundToInt(baseScore * scoreMultiplier);
        Debug.Log($"Updated Score: {score}, Multiplier: {scoreMultiplier:F2}");
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
    canDash = false; // Player has used their dash

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

    // Extending the animation
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

using System.Collections;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    public float deadlyDuration = 2f; // Duration for which the platform is deadly
    public float safeDuration = 2f;   // Duration for which the platform is safe
    private bool isDeadly = false;    // Whether the platform is currently deadly

    private Renderer platformRenderer; // Renderer to change the platform's color

    // Property to access the deadly state
    public bool IsDeadly => isDeadly;

    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        if (platformRenderer == null)
        {
            Debug.LogError("TrapPlatform requires a Renderer component!");
            return;
        }

        // Start the trap cycle
        StartCoroutine(TrapCycle());
    }

    IEnumerator TrapCycle()
    {
        while (true)
        {
            // Set to deadly state
            isDeadly = true;
            platformRenderer.material.color = Color.red; // Red indicates deadly
            yield return new WaitForSeconds(deadlyDuration);

            // Set to safe state
            isDeadly = false;
            platformRenderer.material.color = Color.blue; // Blue indicates safe
            yield return new WaitForSeconds(safeDuration);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckPlayerState(collision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckPlayerState(collision);
    }

    private void CheckPlayerState(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isDeadly)
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die(); // Call the Die method on the player
                Debug.Log("Player died on a deadly trap.");
            }
        }
    }
}

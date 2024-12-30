using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float openSpeed = 2f; // Speed of the door scaling down
    private Vector3 closedScale; // Original scale of the door
    private Vector3 openScale; // Target scale when the door is open
    private bool isOpen = false; // Whether the door is currently open

    void Start()
    {
        closedScale = transform.localScale; // Store the initial scale
        openScale = new Vector3(closedScale.x, 0f, closedScale.z); // Target scale with Y set to zero
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision with player detected.");
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null && player.HasKey())
            {
                OpenDoor();
                player.DropKey(); // Optional: Drop the key after opening the door
            }
        }
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            Debug.Log("Opening door...");
            StartCoroutine(OpenDoorScalingAnimation());
        }
    }

    private IEnumerator OpenDoorScalingAnimation()
    {
        isOpen = true;

        while (Vector3.Distance(transform.localScale, openScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, openScale, openSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = openScale; // Ensure exact scale
        Debug.Log("Door opened successfully!");
    }

    public void ResetDoor()
    {
        // Stop any ongoing animations
        StopAllCoroutines();

        // Reset the door's scale to the closed state
        transform.localScale = closedScale;

        // Reset the door's state
        isOpen = false;

        Debug.Log("Door reset to closed state.");
    }
}

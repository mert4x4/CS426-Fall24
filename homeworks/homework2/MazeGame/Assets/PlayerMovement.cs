using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // Player movement speed
    public Transform cameraTransform; // Reference to the camera for rotation and follow
    public float cameraDistance = 5f; // Distance of the camera from the player
    public float cameraHeight = 1.8f; // Height of the camera above the player
    public float cameraRotationSpeed = 5f; // Speed of camera rotation with mouse
    public float cameraPitchLimit = 45f; // Limit for camera pitch (up/down rotation)
    public float carryOffset = 1.0f; // Offset above the player for carrying the key

    public Rigidbody rb; // Player's Rigidbody
    public float currentYaw = 0f; // Current yaw rotation of the camera
    public float pitch = 0f; // Vertical rotation angle (pitch) of the camera
    public GameObject carriedKey = null; // The key object the player is carrying

    private Vector3 initialPosition; // To store the player's initial position
    private Quaternion initialRotation; // To store the player's initial rotation


private GameManager gameManager; // Reference to the GameManager
public void Start()
{
    // Assign Rigidbody component
    rb = GetComponent<Rigidbody>();
    if (rb == null)
    {
        Debug.LogError("Rigidbody (rb) is not assigned or missing! Please attach a Rigidbody component to the Player GameObject.");
        return; // Exit if Rigidbody is not found
    }
    rb.freezeRotation = true; // Prevent Rigidbody from rotating due to collisions

    // Save initial position and rotation
    initialPosition = transform.position;
    initialRotation = transform.rotation;

    // Automatically assign Main Camera if cameraTransform is not assigned
    if (cameraTransform == null)
    {
        cameraTransform = Camera.main?.transform;
        if (cameraTransform == null)
        {
            Debug.LogError("No Main Camera found in the scene! Please ensure a Camera is tagged as 'MainCamera'.");
        }
    }

    // Find the GameManager in the scene
    gameManager = FindObjectOfType<GameManager>();
    if (gameManager == null)
    {
        Debug.LogError("GameManager not found in the scene! Please ensure a GameManager GameObject exists.");
    }

    Debug.Log("PlayerMovement initialized successfully.");
}


    public void Update()
    {
        HandleCameraRotation();

        // Handle picking up and dropping the key
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (carriedKey != null)
            {
                DropKey();
            }
            else
            {
                TryPickUpKey();
            }
        }
    }

    public void FixedUpdate()
    {
        HandleMovement();
        UpdateCameraPosition();
    }

    public void Die()
    {
        Debug.Log("Player has died!");
               if (gameManager != null)
        {
            gameManager.ResetGame(); // Call the GameManager's reset method
        }
    }

    public void ResetPlayer()
    {
        // Reset the player's position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset the Rigidbody's velocity and angular velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Drop the carried key if there is one
        if (carriedKey != null)
        {
            DropKey();
        }

        Debug.Log("Player reset to initial state.");
    }

    public void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraRotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * cameraRotationSpeed;

        currentYaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -cameraPitchLimit, cameraPitchLimit);
    }

    public void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D keys or Left/Right arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S keys or Up/Down arrow

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = (cameraRight * moveX + cameraForward * moveZ).normalized;
        Vector3 currentPosition = rb.position;
        Vector3 newPosition = currentPosition + movement * moveSpeed * Time.fixedDeltaTime;
        newPosition.y = currentPosition.y; // Lock the Y-axis
        rb.MovePosition(newPosition);
    }

    public void UpdateCameraPosition()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned!");
            return;
        }

        Vector3 offset = Quaternion.Euler(pitch, currentYaw, 0) * new Vector3(0, cameraHeight, -cameraDistance);
        Vector3 targetPosition = transform.position + offset;

        cameraTransform.position = targetPosition;
        cameraTransform.LookAt(transform.position + Vector3.up * cameraHeight);
    }

    public void TryPickUpKey()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f); // Adjust radius as needed
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Key"))
            {
                PickUpKey(collider.gameObject);
                break;
            }
        }
    }

    public void PickUpKey(GameObject key)
    {
        carriedKey = key;

        // Disable physics and collisions for the key while carrying it
        Rigidbody keyRigidbody = carriedKey.GetComponent<Rigidbody>();
        if (keyRigidbody != null)
        {
            keyRigidbody.isKinematic = true;
        }
        carriedKey.GetComponent<Collider>().enabled = false;

        // Parent the key to the player
        carriedKey.transform.SetParent(transform);

        // Position the key dynamically above the player
        PositionKeyAbovePlayer();

        Debug.Log("Key picked up!");
    }

    public void PositionKeyAbovePlayer()
    {
        if (carriedKey == null) return;

        // Dynamically position the key above the player
        carriedKey.transform.localPosition = new Vector3(0, 1.5f + carryOffset, 0); // Adjust Y-axis offset
        carriedKey.transform.localRotation = Quaternion.identity; // Reset rotation
    }

    public void DropKey()
    {
        if (carriedKey == null) return;

        // Re-enable physics and collisions for the key
        Rigidbody keyRigidbody = carriedKey.GetComponent<Rigidbody>();
        if (keyRigidbody != null)
        {
            keyRigidbody.isKinematic = false;
        }
        carriedKey.GetComponent<Collider>().enabled = true;

        // Unparent the key
        carriedKey.transform.SetParent(null);

        // Drop the key slightly above the ground near the player
        carriedKey.transform.position = transform.position + Vector3.up * 0.5f;

        Debug.Log("Key dropped!");
        carriedKey = null;
    }

public void OnCollisionEnter(Collision collision)
{
    // Handle collision with "Door"
    if (collision.gameObject.CompareTag("Door") && HasKey())
    {
        Door door = collision.gameObject.GetComponent<Door>();
        if (door != null)
        {
            door.OpenDoor();
            Destroy(carriedKey);
            carriedKey = null;
            Debug.Log($"Collided with {collision.gameObject.name}, Key used and destroyed.");
        }
    }
    // Handle collision with "Finish"
    else if (collision.gameObject.CompareTag("Finish"))
    {
        Debug.Log("You won the game!");
               if (gameManager != null)
        {
            gameManager.ResetGame(); // Call the GameManager's reset method
        }
        // You can add additional functionality here (e.g., trigger victory screen or restart the game)
    }
}


    public bool HasKey()
    {
        return carriedKey != null;
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Transform startPlatformTransform;  // Starting platform position
    public PlatformSpawner platformSpawner;
    public float fallThreshold = -10f;  // Y position threshold for falling

    void Update()
    {
        CheckPlayerFall();
    }

    private void CheckPlayerFall()
    {
        if (player.transform.position.y < fallThreshold)
        {
            Debug.Log("Player fell off the platform. Resetting game...");
            ResetGame();
        }
    }

    public void ResetGame()
    {
        // Reset player position
        player.transform.position = startPlatformTransform.position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Reset player state
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        movement.ResetJumpCount();
        movement.ResetMaxJumps();

        // Reset platform spawner
        platformSpawner.ResetSpawner();

        Debug.Log("Game reset completed.");
    }
}

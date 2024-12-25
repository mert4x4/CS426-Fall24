using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Transform startPlatformTransform;
    public PlatformSpawner platformSpawner;
    public float fallThreshold = -10f;

    void Update()
    {
        CheckPlayerFall();
    }

    private void CheckPlayerFall()
    {
        if (player.transform.position.y < fallThreshold)
        {
            ResetGame();
        }
    }

    public void ResetGame()
    {
        player.transform.position = startPlatformTransform.position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.ResetJumpCount();
            movement.ResetMaxJumps();
        }

        platformSpawner.ResetSpawner();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MazeLoader mazeLoader; // Reference to the MazeLoader script
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script

    private Guard[] guards; // Array of all Guard instances
    private Door[] doors; // Array of all Door instances

    void Start()
    {
        // Find all guards and doors in the scene
        guards = FindObjectsOfType<Guard>();
        doors = FindObjectsOfType<Door>();

        Debug.Log($"Found {guards.Length} guards and {doors.Length} doors in the scene.");
    }

    public void ResetGame()
    {
        Debug.Log("Starting game reset...");

        // Reset Maze
        if (mazeLoader != null)
        {
            mazeLoader.ResetMazeLoader();
            Debug.Log("Maze reset successfully.");
        }
        else
        {
            Debug.LogError("MazeLoader reference is missing in GameManager!");
        }

        // Reset Player
        if (playerMovement != null)
        {
            playerMovement.ResetPlayer(); // Assuming ResetPlayer resets player state
            Debug.Log("Player reset successfully.");
        }
        else
        {
            Debug.LogError("PlayerMovement reference is missing in GameManager!");
        }

        // Reset Guards
        foreach (Guard guard in guards)
        {
            if (guard != null)
            {
                guard.ResetGuard();
                Debug.Log("Guard reset successfully.");
            }
            else
            {
                Debug.LogWarning("A guard reference is missing or null.");
            }
        }

        // Reset Doors
        foreach (Door door in doors)
        {
            if (door != null)
            {
                door.ResetDoor();
                Debug.Log("Door reset successfully.");
            }
            else
            {
                Debug.LogWarning("A door reference is missing or null.");
            }
        }

        Debug.Log("Game reset complete.");
        SceneManager.LoadScene("menu_scene");
        
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
Cursor.visible = true; // Makes the cursor visible


    }
}

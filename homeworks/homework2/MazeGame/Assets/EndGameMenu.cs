using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // For TextMeshPro

public class EndGameMenu : MonoBehaviour
{
    private static bool isGameStarted = false; // Tracks whether the game has started

    void Start()
    {
        // Find the StartText object
        GameObject startTextObject = GameObject.Find("StartText");
        if (startTextObject != null)
        {
            TMP_Text startText = startTextObject.GetComponent<TMP_Text>(); // Get the TMP_Text component
            if (startText != null)
            {
                // Set the text based on whether the game has started
                startText.text = isGameStarted ? "RESTART" : "START";
                Debug.Log($"StartText set to {(isGameStarted ? "RESTART" : "START")}.");
            }
            else
            {
                Debug.LogError("StartText object does not have a TMP_Text component!");
            }
        }
        else
        {
            Debug.LogError("StartText object not found in the scene!");
        }
    }

    // Called when Restart button is clicked
    public void RestartGame()
    {
        Debug.Log("Restart button pressed. Loading SampleScene...");
        isGameStarted = true; // Mark the game as started

        // Load the SampleScene
        SceneManager.LoadScene("SampleScene");
    }

    // Called when Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Quits the application
    }
}

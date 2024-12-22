using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshProUGUI

public class MainMenuManager : MonoBehaviour
{
    private TextMeshProUGUI topScoreText; // Reference to display the top score

    void Start()
    {
        // Find the TopScoreText GameObject by its tag
        GameObject topScoreObject = GameObject.FindGameObjectWithTag("TopScore");
        if (topScoreObject != null)
        {
            topScoreText = topScoreObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("GameObject with tag 'TopScore' not found!");
        }

        // Retrieve and display the top score
        int topScore = PlayerPrefs.GetInt("HighScore", 0); // Default to 0 if no high score is saved
        if (topScoreText != null)
        {
            topScoreText.text = $"Top Score: {topScore}";
        }
        else
        {
            Debug.LogError("TopScoreText component is not assigned or found.");
        }
    }

    // Start the game by loading the main game scene
    public void StartGame()
    {
        Debug.Log("Start Game button clicked!");
        // Load the main game scene (ensure the scene is added in Build Settings)
        SceneManager.LoadScene("SampleScene");
    }

    // Quit the application
    public void QuitGame()
    {
        Debug.Log("Quit Game button clicked!");
        // Exit the game (will only work in a built application, not in the editor)
        Application.Quit();
    }
}

using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreAndMultiplierText;  // Reference to TextMeshProUGUI
    private PlayerMovement playerMovement;          // Reference to the PlayerMovement script
    private int highScore;                          // High score variable

    void Start()
    {
        // Load the saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Find the player by its tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found!");
        }

        // Find the TextMeshProUGUI component by its tag
        GameObject scoreDisplayerObject = GameObject.FindGameObjectWithTag("ScoreDisplayer");
        if (scoreDisplayerObject != null)
        {
            scoreAndMultiplierText = scoreDisplayerObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("GameObject with tag 'ScoreDisplayer' not found!");
        }

        if (scoreAndMultiplierText != null)
        {
            Debug.Log("TextMeshProUGUI is successfully assigned.");
            scoreAndMultiplierText.text = $"High Score: {highScore}";
        }
    }

    void Update()
    {
        if (playerMovement != null && scoreAndMultiplierText != null)
        {
            // Retrieve the score and multiplier from PlayerMovement
            int score = playerMovement.GetScore();
            float multiplier = playerMovement.GetMultiplier();

            // Check and update the high score
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore); // Save the new high score
                PlayerPrefs.Save();
                Debug.Log($"New High Score Saved: {highScore}");
            }

            // Debug the retrieved values
            Debug.Log($"[ScoreDisplay] Score: {score}, Multiplier: {multiplier:F2}, High Score: {highScore}");

            // Update the text
            scoreAndMultiplierText.text = $"Score: {score}\nMultiplier: {multiplier:F2}\nHigh Score: {highScore}";
        }
        else
        {
            if (playerMovement == null)
                Debug.LogError("[ScoreDisplay] PlayerMovement script is not found or assigned.");
            if (scoreAndMultiplierText == null)
                Debug.LogError("[ScoreDisplay] TextMeshProUGUI component is not assigned.");
        }
    }
}

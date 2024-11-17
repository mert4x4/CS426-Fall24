using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10;  // Points awarded when the coin is collected

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collected coin: {gameObject.name}");
            // Add score logic here if needed
            Destroy(gameObject);  // Destroy the coin
        }
        else
        {
            Debug.LogWarning($"Unexpected collision detected with: {other.gameObject.name}, Tag: {other.gameObject.tag}");
        }
    }
}

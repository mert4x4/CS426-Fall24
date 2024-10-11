using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Public references to the Slippery and Bouncy Prefabs
    public GameObject slipperyPrefab;  
    public GameObject bouncyPrefab;    

    // Fixed coordinates for spawning objects
    private Vector3 spawnPosition = new Vector3(-1.934042f, 7.053102f, -2.538458f);

    void Update()
    {
        // Check if the "U" key is pressed to spawn the Slippery Object
        if (Input.GetKeyDown(KeyCode.U))
        {
            // Ensure the slipperyPrefab is assigned before attempting to spawn it
            if (slipperyPrefab != null)
            {
                Instantiate(slipperyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Slippery Prefab is not assigned in the Inspector.");
            }
        }

        // Check if the "Y" key is pressed to spawn the Bouncy Object
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Ensure the bouncyPrefab is assigned before attempting to spawn it
            if (bouncyPrefab != null)
            {
                Instantiate(bouncyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Bouncy Prefab is not assigned in the Inspector.");
            }
        }
    }
}

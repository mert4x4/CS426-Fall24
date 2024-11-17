using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;  // Prefab for the platform
    public GameObject coinPrefab;      // Prefab for the coin
    public int initialPlatformCount = 5;  // Number of platforms to spawn initially
    public float platformLength = 10f;  // Length of each platform
    public Transform playerTransform;  // Reference to the player's transform
    public float spawnDistanceAhead = 30f;  // Distance to spawn platforms ahead of the player

    private Queue<GameObject> activePlatforms = new Queue<GameObject>();  // Platforms in use
    private float nextSpawnZ = 0f;  // Z-coordinate for the next platform spawn

    private Vector3[] spawnPatterns = 
    {
        new Vector3(-6f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(4f, 0f, 0f),
        new Vector3(-5f, 1f, 0f), new Vector3(6f, 1f, 0f), new Vector3(0f, 2f, 0f)
    };

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                Debug.Log("Player Transform assigned dynamically.");
            }
            else
            {
                Debug.LogError("Player object not found! Make sure it is tagged as 'Player'.");
            }
        }

        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        while (playerTransform.position.z + spawnDistanceAhead > nextSpawnZ)
        {
            SpawnPlatform();
            RemovePreviousPlatforms();
        }
    }

    void SpawnPlatform()
    {
        Vector3 randomOffset = spawnPatterns[Random.Range(0, spawnPatterns.Length)];
        GameObject platform = Instantiate(platformPrefab, new Vector3(randomOffset.x, randomOffset.y, nextSpawnZ), Quaternion.identity);
        activePlatforms.Enqueue(platform);

        // Spawn a coin between platforms
        Vector3 coinPosition = new Vector3(randomOffset.x, randomOffset.y + 1f, nextSpawnZ - platformLength / 2);
        SpawnCoin(coinPosition);

        nextSpawnZ += platformLength;
    }

    void SpawnCoin(Vector3 position)
    {
        GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
        Debug.Log($"Coin spawned at position: {position}");
    }

    void RemovePreviousPlatforms()
    {
        while (activePlatforms.Count > 0 && activePlatforms.Peek().transform.position.z + platformLength < playerTransform.position.z)
        {
            GameObject oldPlatform = activePlatforms.Dequeue();
            if (oldPlatform != null)
            {
                Debug.Log($"Removing platform: {oldPlatform.name} at position {oldPlatform.transform.position}");
                Destroy(oldPlatform);
            }
        }
    }
}

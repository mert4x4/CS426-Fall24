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
    private List<GameObject> activeCoins = new List<GameObject>();        // Coins in use
    private float nextSpawnZ = 0f;  // Z-coordinate for the next platform spawn

    private Vector3[] spawnPatterns =
    {
        new Vector3(-6f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(4f, 0f, 0f),
        new Vector3(-5f, 1f, 0f), new Vector3(6f, 1f, 0f), new Vector3(0f, 2f, 0f)
    };

    private bool isFirstSpawnSkipped = false;  // Flag to skip the first platform and coin

    void Start()
    {
        InitializePlayerTransform();

        for (int i = 0; i < initialPlatformCount; i++)
        {
            if (!isFirstSpawnSkipped)
            {
                // Skip the first spawn
                isFirstSpawnSkipped = true;
                nextSpawnZ += platformLength;  // Skip the first platform space
                continue;
            }

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

    private void InitializePlayerTransform()
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
        activeCoins.Add(coin);  // Track the active coins
        Debug.Log($"Coin spawned at position: {position}");
    }

    void RemovePreviousPlatforms()
    {
        while (activePlatforms.Count > 0)
        {
            GameObject oldPlatform = activePlatforms.Peek();

            if (oldPlatform == null)
            {
                activePlatforms.Dequeue();
                continue;
            }

            if (oldPlatform.transform.position.z + platformLength < playerTransform.position.z)
            {
                Debug.Log($"Removing platform: {oldPlatform.name} at position {oldPlatform.transform.position}");
                activePlatforms.Dequeue();
                Destroy(oldPlatform);
            }
            else
            {
                break;
            }
        }

        // Remove coins that are behind the player
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            if (activeCoins[i] != null && activeCoins[i].transform.position.z < playerTransform.position.z)
            {
                Destroy(activeCoins[i]);
                activeCoins.RemoveAt(i);
            }
        }
    }

    public void ResetSpawner()
    {
        // Destroy all active platforms and coins
        while (activePlatforms.Count > 0)
        {
            GameObject platform = activePlatforms.Dequeue();
            Destroy(platform);
        }

        foreach (GameObject coin in activeCoins)
        {
            if (coin != null)
            {
                Destroy(coin);
            }
        }
        activeCoins.Clear();

        // Reset spawn position and state
        nextSpawnZ = 0f;
        isFirstSpawnSkipped = false;  // Reset the first spawn skip state

        // Spawn initial platforms again
        for (int i = 0; i < initialPlatformCount; i++)
        {
            if (!isFirstSpawnSkipped)
            {
                isFirstSpawnSkipped = true;
                nextSpawnZ += platformLength;  // Skip the first platform space
                continue;
            }

            SpawnPlatform();
        }

        Debug.Log("PlatformSpawner reset completed.");
    }
}

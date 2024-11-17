using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;  // Prefab for the platform
    public int initialPlatformCount = 5;  // Number of platforms to spawn initially
    public float platformLength = 10f;  // Length of each platform
    public Transform playerTransform;  // Reference to the player's transform
    public float spawnDistanceAhead = 30f;  // Distance to spawn platforms ahead of the player

    private Queue<GameObject> activePlatforms = new Queue<GameObject>();  // Platforms in use
    private float nextSpawnZ = 0f;  // Z-coordinate for the next platform spawn

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

        Debug.Log("PlatformSpawner initialized.");

        // Spawn initial platforms
        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        Debug.Log($"Player Z: {playerTransform.position.z}, Next Spawn Threshold: {nextSpawnZ - spawnDistanceAhead}");

        // Spawn new platforms as the player moves forward
        if (playerTransform.position.z + spawnDistanceAhead > nextSpawnZ)
        {
            Debug.Log($"Spawning new platform at Z: {nextSpawnZ}");
            SpawnPlatform();
            RemovePreviousPlatforms();
        }
    }

    void SpawnPlatform()
    {
        GameObject platform = Instantiate(platformPrefab, new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
        activePlatforms.Enqueue(platform);  // Add platform to queue
        nextSpawnZ += platformLength;  // Update the next spawn Z position

        Debug.Log($"Platform spawned at position: {platform.transform.position}");
    }

    void RemovePreviousPlatforms()
    {
        while (activePlatforms.Count > 0 && activePlatforms.Peek().transform.position.z + platformLength < playerTransform.position.z)
        {
            GameObject oldPlatform = activePlatforms.Dequeue();
            Debug.Log($"Removing platform: {oldPlatform.name} at position {oldPlatform.transform.position}");
            Destroy(oldPlatform);  // Destroy the platform
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"TriggerExit detected on object: {other.gameObject.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger zone of a platform.");
            RemovePreviousPlatforms();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"TriggerEnter detected on object: {other.gameObject.name}");
    }
}

using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public int initialPlatformCount = 5;
    public int poolSize = 10;
    public float platformLength = 10f;
    public Transform playerTransform;
    public float spawnDistanceAhead = 30f;

    private List<GameObject> platformPool = new List<GameObject>();
    private List<GameObject> coinPool = new List<GameObject>();
    private Queue<GameObject> activePlatforms = new Queue<GameObject>();
    private List<GameObject> activeCoins = new List<GameObject>();

    private float nextSpawnZ = 0f;
    private Vector3[] spawnPatterns = {
        new Vector3(-6f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(4f, 0f, 0f),
        new Vector3(-5f, 1f, 0f), new Vector3(6f, 1f, 0f), new Vector3(0f, 2f, 0f)
    };
    private bool isFirstSpawnSkipped = false;

    void Start()
    {
        InitializePlayerTransform();
        InitializePool(platformPool, platformPrefab, poolSize);
        InitializePool(coinPool, coinPrefab, poolSize);

        for (int i = 0; i < initialPlatformCount; i++)
        {
            if (!isFirstSpawnSkipped)
            {
                isFirstSpawnSkipped = true;
                nextSpawnZ += platformLength;
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

    private void InitializePool(List<GameObject> pool, GameObject prefab, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    private void SpawnPlatform()
    {
        Vector3 randomOffset = spawnPatterns[Random.Range(0, spawnPatterns.Length)];
        GameObject platform = GetFromPool(platformPool);

        if (platform != null)
        {
            platform.transform.position = new Vector3(randomOffset.x, randomOffset.y, nextSpawnZ);
            platform.transform.rotation = Quaternion.identity;
            platform.transform.localScale = new Vector3(2.698692f, 0.44969f, 2.811703f);
            platform.SetActive(true);
            activePlatforms.Enqueue(platform);
        }

        Vector3 coinPosition = new Vector3(randomOffset.x, randomOffset.y + 1f, nextSpawnZ - platformLength / 2);
        SpawnCoin(coinPosition);

        nextSpawnZ += platformLength;
    }

    private void SpawnCoin(Vector3 position)
    {
        GameObject coin = GetFromPool(coinPool);

        if (coin != null)
        {
            coin.transform.position = position;
            coin.transform.rotation = Quaternion.identity;
            coin.SetActive(true);
            activeCoins.Add(coin);
        }
    }

    private void RemovePreviousPlatforms()
    {
        while (activePlatforms.Count > 0)
        {
            GameObject oldPlatform = activePlatforms.Peek();

            if (oldPlatform == null || !oldPlatform.activeInHierarchy || oldPlatform.transform.position.z + platformLength < playerTransform.position.z)
            {
                activePlatforms.Dequeue();
                if (oldPlatform != null)
                {
                    ReturnToPool(platformPool, oldPlatform);
                }
            }
            else
            {
                break;
            }
        }

        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            GameObject coin = activeCoins[i];
            if (coin != null && (!coin.activeInHierarchy || coin.transform.position.z < playerTransform.position.z))
            {
                ReturnToPool(coinPool, coin);
                activeCoins.RemoveAt(i);
            }
        }
    }

    private GameObject GetFromPool(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                return obj;
            }
        }

        Debug.LogWarning("Pool is empty. Expanding the pool dynamically.");
        GameObject newObj = Instantiate(pool == platformPool ? platformPrefab : coinPrefab);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }

    private void ReturnToPool(List<GameObject> pool, GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    public void ResetSpawner()
    {
        while (activePlatforms.Count > 0)
        {
            GameObject platform = activePlatforms.Dequeue();
            if (platform != null)
            {
                ReturnToPool(platformPool, platform);
            }
        }

        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            GameObject coin = activeCoins[i];
            if (coin != null)
            {
                ReturnToPool(coinPool, coin);
            }
        }
        activeCoins.Clear();

        nextSpawnZ = 0f;
        isFirstSpawnSkipped = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetScore(0);
                playerMovement.SetMultiplier(1f);
                Debug.Log("Player's score and multiplier reset.");
            }
        }

        for (int i = 0; i < initialPlatformCount; i++)
        {
            if (!isFirstSpawnSkipped)
            {
                isFirstSpawnSkipped = true;
                nextSpawnZ += platformLength;
                continue;
            }
            SpawnPlatform();
        }
    }
}
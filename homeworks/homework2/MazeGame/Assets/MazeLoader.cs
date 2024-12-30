using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeLoader : MonoBehaviour
{
    public GameObject playerPrefab;  // Assign the player prefab in the Inspector
    public GameObject wallPrefab;   // Assign the wall prefab in the Inspector
    public GameObject floorPrefab;  // Assign the floor prefab in the Inspector
    public GameObject keyPrefab;    // Assign the key prefab in the Inspector
    public GameObject doorPrefab;   // Assign the door prefab in the Inspector
    public GameObject trapPrefab;   // Assign the trap prefab in the Inspector
    public GameObject guardPrefab;  // Assign the guard prefab in the Inspector
    public GameObject finishPrefab; // Assign the Finish prefab in the Inspector


    private GameObject playerInstance;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Define the maze as a 2D array
int[,] maze = new int[,]
{
    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    {1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1},
    {1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1},
    {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 2, 0, 1},
    {1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1},
    {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    {1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1},
    {1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1},
    {1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1},
    {1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1},
    {1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1},
    {1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1},
    {1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1},
    {1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1},
    {1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1},
    {1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1},
    {1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1},
    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1},
    {1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1},
    {1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 2, 1},
    {1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1},
    {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1},
    {1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
    {1, 0, 1, 3, 0, 0, 1, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
};


    void Start()
    {
        GenerateMaze();
        SpawnPlayerAndKey();
        SpawnDoor();
        SpawnGuard(); // Spawn guards in the maze
    }

void GenerateMaze()
{
    int rows = maze.GetLength(0);
    int cols = maze.GetLength(1);

    // Loop through the maze array and instantiate prefabs
    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < cols; x++)
        {
            // Adjust position for correct orientation
            Vector3 position = new Vector3(x, 0, -y); // Swap Y with Z for a top-down view

            if (maze[y, x] == 1)  // Wall
            {
                Vector3 wallPosition = new Vector3(position.x, wallPrefab.transform.localScale.y / 2, position.z);
                spawnedObjects.Add(Instantiate(wallPrefab, wallPosition, Quaternion.identity));
            }
            else if (maze[y, x] == 0)  // Path
            {
                spawnedObjects.Add(Instantiate(floorPrefab, position, Quaternion.identity));
            }
            else if (maze[y, x] == 2)  // Trap
            {
                spawnedObjects.Add(Instantiate(trapPrefab, position, Quaternion.identity));
            }
            else if (maze[y, x] == 3)  // Finish
            {
                spawnedObjects.Add(Instantiate(finishPrefab, position, Quaternion.identity));
                Debug.Log($"Finish object created at position {position}");
            }
        }
    }
}


public void SpawnPlayerAndKey()
{
    if (playerPrefab == null || keyPrefab == null)
    {
        Debug.LogError("Player or Key Prefab is not assigned!");
        return;
    }

    // Spawn the player
    int playerSpawnX = 1, playerSpawnY = 1;
    Vector3 playerSpawnPosition = new Vector3(playerSpawnX, 0.9f, -playerSpawnY);
    playerInstance = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    Debug.Log("Player spawned successfully.");

    // List of key spawn positions (customize these as needed)
    List<Vector3> keySpawnPositions = new List<Vector3>
    {
        new Vector3(1, 0.9f, -3),
        new Vector3(22, 0.9f, -3),
        new Vector3(23, 0.9f, -21),
    };

    // Spawn each key
    foreach (Vector3 keyPosition in keySpawnPositions)
    {
        spawnedObjects.Add(Instantiate(keyPrefab, keyPosition, Quaternion.identity));
        Debug.Log($"Key spawned at position {keyPosition}");
    }
}


void SpawnDoor()
{
    if (doorPrefab == null)
    {
        Debug.LogError("Door Prefab is not assigned!");
        return;
    }

    // Mock coordinates for doors
    List<Vector3> doorSpawnPositions = new List<Vector3>
    {
        new Vector3(3, 1f, -10),
        new Vector3(13, 1f, -15),
        new Vector3(5, 1f, -22)
    };

    // Define a 90-degree rotation
    Quaternion rotation = Quaternion.Euler(0, 90, 0);

    // Spawn each door with rotation
    foreach (Vector3 doorPosition in doorSpawnPositions)
    {
        spawnedObjects.Add(Instantiate(doorPrefab, doorPosition, rotation));
        Debug.Log($"Door spawned at position {doorPosition} with rotation {rotation.eulerAngles}");
    }
}



    void SpawnGuard()
    {
        if (guardPrefab == null)
        {
            Debug.LogError("Guard Prefab is not assigned!");
            return;
        }

        List<(Vector3, Vector3)> guardPaths = new List<(Vector3, Vector3)>
        {
            (new Vector3(1, 1f, -11), new Vector3(5, 1f, -11)),
            (new Vector3(13, 1f, -5), new Vector3(23, 1f, -5)),
            (new Vector3(11, 1f, -17), new Vector3(17, 1f, -17)),
            (new Vector3(21, 1f, -15), new Vector3(21, 1f, -21))
        };

        foreach (var path in guardPaths)
        {
            GameObject guard = Instantiate(guardPrefab, path.Item1, Quaternion.identity);
            spawnedObjects.Add(guard);
            Guard guardScript = guard.GetComponent<Guard>();
            if (guardScript != null)
            {
                guardScript.SetPatrolPoints(path.Item1, path.Item2);
            }
        }
    }

    public void ResetMazeLoader()
    {
        Debug.Log("Resetting the maze...");

        // Destroy all spawned objects
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();

        // Destroy player instance
        if (playerInstance != null)
        {
            Destroy(playerInstance);
            playerInstance = null;
        }

        // Reinitialize the maze and objects
        GenerateMaze();
        SpawnPlayerAndKey();
        SpawnDoor();
        SpawnGuard();

        Debug.Log("Maze reset complete.");
    }
}

using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject playerPrefab; // Assign your player prefab in the inspector
    public Transform spawnPoint1;  // Assign the first spawn point in the inspector
    public Transform spawnPoint2;  // Assign the second spawn point in the inspector

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Randomly choose a spawn point (0 or 1)
        Transform chosenSpawnPoint = Random.value < 0.5f ? spawnPoint1 : spawnPoint2;

        // Instantiate the player at the chosen spawn point
        Instantiate(playerPrefab, chosenSpawnPoint.position, chosenSpawnPoint.rotation);
    }
}

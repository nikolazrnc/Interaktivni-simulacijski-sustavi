using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    public GameObject playerPrefab;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        float randomValue = Random.value;

        Vector3 spawnPosition;

        if (randomValue < 0.5f)
        {
            spawnPosition = spawnPoint1.position;
        }
        else
        {
            spawnPosition = spawnPoint2.position;
        }

        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}
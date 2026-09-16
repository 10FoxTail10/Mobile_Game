using System.Collections.Generic;
using UnityEngine;

public enum ItemSpawnType
{
    Universal,
    KeyRoom
}

public class ItemRandomSpawn : MonoBehaviour
{
    [SerializeField] private Transform[] universalSpawnPoints;
    [SerializeField] private Transform[] keyRoomSpawnPoints;
    [SerializeField] private GameObject[] items;
    [SerializeField] private ItemSpawnType[] itemSpawnTypes;

    private List<Transform> usedSpawnPoints = new List<Transform>();

    void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            SpawnItem(i);
        }
    }

    private void SpawnItem(int itemIndex)
    {
        Transform[] spawnPoints;

        if (itemSpawnTypes[itemIndex] == ItemSpawnType.KeyRoom)
        {
            spawnPoints = keyRoomSpawnPoints;
        }
        else
        {
            spawnPoints = universalSpawnPoints;
        }

        List<Transform> freeSpawnPoints = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            if (!usedSpawnPoints.Contains(point))
            {
                freeSpawnPoints.Add(point);
            }
        }

        int randomIndex = Random.Range(0, freeSpawnPoints.Count);
        Transform selectedPoint = freeSpawnPoints[randomIndex];
        items[itemIndex].transform.position = selectedPoint.position;
        usedSpawnPoints.Add(selectedPoint);
    }
}

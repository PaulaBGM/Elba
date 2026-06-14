using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceNode prefab;

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 300f;

    [Header("Random Respawn")]
    [SerializeField] private bool useRandomSpawnPoint;
    [SerializeField] private Transform[] spawnPoints;

    private ResourceNode currentNode;

    private void Start()
    {
        SpawnNode(GetSpawnPosition());
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        while (true)
        {
            if (currentNode == null || !currentNode.gameObject.activeSelf)
            {
                yield return new WaitForSeconds(respawnTime);

                if (currentNode == null)
                {
                    SpawnNode(GetSpawnPosition());
                }
                else
                {
                    currentNode.transform.position = GetSpawnPosition();
                    currentNode.ResetNode();
                    currentNode.gameObject.SetActive(true);
                }
            }

            yield return null;
        }
    }

    private void SpawnNode(Vector3 position)
    {
        currentNode = Instantiate(prefab, position, Quaternion.identity);
    }

    private Vector3 GetSpawnPosition()
    {
        if (!useRandomSpawnPoint || spawnPoints == null || spawnPoints.Length == 0)
            return transform.position;

        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }
}
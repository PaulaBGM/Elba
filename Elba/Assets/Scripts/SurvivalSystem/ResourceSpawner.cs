using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceNode prefab;

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 300f;

    [Header("Spawn Points")]
    [SerializeField] private bool useRandomSpawnPoint;
    [SerializeField] private Transform[] spawnPoints;

    private ResourceNode currentNode;

    private Coroutine respawnRoutine;

    private void Start()
    {
        SpawnNode(GetSpawnPosition());
    }

    private void Update()
    {
        if (respawnRoutine != null)
            return;

        if (currentNode == null || currentNode.gameObject.activeSelf)
            return;

        respawnRoutine = StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
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

        respawnRoutine = null;
    }

    private void SpawnNode(Vector3 position)
    {
        currentNode = Instantiate(prefab, position, Quaternion.identity);
    }

    private Vector3 GetSpawnPosition()
    {
        if (!useRandomSpawnPoint)
            return transform.position;

        if (spawnPoints == null || spawnPoints.Length == 0)
            return transform.position;

        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }
}
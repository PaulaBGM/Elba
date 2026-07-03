using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public EnemySpawnData data;

        [HideInInspector]
        public readonly List<GameObject> aliveEnemies = new();
    }

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Enemies")]
    [SerializeField] private EnemyGroup[] enemies;

    private void Start()
    {
        foreach (EnemyGroup enemy in enemies)
        {
            StartCoroutine(SpawnRoutine(enemy));
        }
    }

    private IEnumerator SpawnRoutine(EnemyGroup enemy)
    {
        while (true)
        {
            enemy.aliveEnemies.RemoveAll(e => e == null);

            if (enemy.aliveEnemies.Count < enemy.data.maxAlive)
            {
                SpawnEnemy(enemy);
            }

            yield return new WaitForSeconds(enemy.data.respawnTime);
        }
    }

    private void SpawnEnemy(EnemyGroup enemy)
    {
        if (spawnPoints.Length == 0)
            return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject instance = Instantiate(
            enemy.data.prefab,
            point.position,
            Quaternion.identity);

        enemy.aliveEnemies.Add(instance);
    }
}
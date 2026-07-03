using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Spawn Data")]
public class EnemySpawnData : ScriptableObject
{
    public GameObject prefab;

    [Min(1)]
    public int maxAlive = 5;

    [Min(1)]
    public float respawnTime = 30f;
}
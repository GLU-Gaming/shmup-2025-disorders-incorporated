using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyConfiguration", menuName = "Enemy Configuration")]
public class EnemyConfiguration : ScriptableObject
{
    public GameObject enemyPrefab;
    public float spawnDistance = 10f;
    public float spawnAmount = 5f;
    public float spawnDelay = 1f;
    public float minY = -11f;
    public float maxY = 8f;
}

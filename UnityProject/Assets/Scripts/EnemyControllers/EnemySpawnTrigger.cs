using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int enemyCount = 3;

    [Header("Spawn Area")]
    public Collider spawnAreaCollider;

    [Header("Nur einmal spawnen?")]
    public bool spawnOnce = true;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!hasSpawned || !spawnOnce))
        {
            SpawnEnemies();
            if (spawnOnce)
                hasSpawned = true;
        }
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null || spawnAreaCollider == null)
        {
            Debug.LogWarning("EnemyPrefab oder SpawnCollider fehlt!");
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = GetRandomPointInBounds(spawnAreaCollider.bounds);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y, // oder z.B. terrain height
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
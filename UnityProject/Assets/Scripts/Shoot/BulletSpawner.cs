using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to spawn
    public Transform[] spawnPoints; // Array of bullet spawn points
    public float spawnInterval = 2f; // Time between each spawn

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(SpawnBullets), 0f, spawnInterval);
    }

    private void SpawnBullets()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            Vector3 direction = (player.position - spawnPoint.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = direction * 10f; // Bullet speed
        }
    }
}

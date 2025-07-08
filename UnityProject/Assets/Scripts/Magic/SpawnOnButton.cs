using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnOnButton : MonoBehaviour
{
    [Header("Zu spawnende Prefabs")]
    public GameObject[] prefabsToSpawn;

    [Header("Spawnposition")]
    public Transform spawnPoint;

    [Header("Button")]
    public Button spawnButton;

    [Header("Wie lange sichtbar?")]
    public float lifetime = 3f;

    void Start()
    {
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(SpawnTemporaryObjects);
        }
    }

    public void SpawnTemporaryObjects()
    {
        foreach (GameObject prefab in prefabsToSpawn)
        {
            if (prefab != null && spawnPoint != null)
            {
                GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                Destroy(clone, lifetime); // Zerstört nach 3 Sekunden
            }
        }
    }
}
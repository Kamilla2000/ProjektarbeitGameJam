using UnityEngine;
using UnityEngine.UI;
using TMPro; // Falls du TextMeshPro verwendest

public class SpawnerController : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public Transform targetPoint;

    public Button spawnButton;
    public TextMeshProUGUI ammoText; // Falls du normalen Text nutzt, ändere auf "Text"
    
    public int maxSpawns = 3;
    private int currentSpawns;

    private void Start()
    {
        currentSpawns = 0;
        UpdateAmmoUI();

        spawnButton.onClick.AddListener(Spawn);
    }

    private void Spawn()
    {
        if (currentSpawns >= maxSpawns)
        {
            Debug.Log("Keine Spawns mehr übrig!");
            return;
        }

        GameObject spawnedObj = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // Bewegung zu Ziel (optional smooth)
        StartCoroutine(MoveToTarget(spawnedObj.transform, targetPoint.position));

        currentSpawns++;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = "Schüsse übrig: " + (maxSpawns - currentSpawns);
    }

    private System.Collections.IEnumerator MoveToTarget(Transform obj, Vector3 target)
    {
        float speed = 5f;
        while (Vector3.Distance(obj.position, target) > 0.1f)
        {
            obj.position = Vector3.MoveTowards(obj.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}
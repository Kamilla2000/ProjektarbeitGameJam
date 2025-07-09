using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoveLetterHealer : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public Transform targetPoint;

    public Button spawnButton;
    public TextMeshProUGUI ammoText;

    public int maxSpawns = 3;
    private int currentSpawns;

    public PlayerHealthWithKissAndGameOver princessHealth;
    public PrincessDialogSystem princessDialog;

    private void Start()
    {
        currentSpawns = 0;
        UpdateAmmoUI();

        if (spawnButton != null)
            spawnButton.onClick.AddListener(SendLoveLetter);
    }

    private void SendLoveLetter()
    {
        if (currentSpawns >= maxSpawns) return;

        if (princessHealth != null)
        {
            princessHealth.Heal(30f);
        }

        if (princessDialog != null)
        {
            princessDialog.ShowRandomHealReply();
        }

        GameObject spawnedObj = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        StartCoroutine(MoveToTarget(spawnedObj.transform, targetPoint.position));

        currentSpawns++;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = "Letters left: " + (maxSpawns - currentSpawns);
    }

    private System.Collections.IEnumerator MoveToTarget(Transform obj, Vector3 target)
    {
        float speed = 5f;
        while (Vector3.Distance(obj.position, target) > 0.1f)
        {
            obj.position = Vector3.MoveTowards(obj.position, target, speed * Time.deltaTime);
            yield return null;
        }
        Destroy(obj.gameObject);
    }
}

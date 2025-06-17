using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Item Einstellungen")]
    [Tooltip("Eindeutiger Name des Items, z.B. 'coin', 'potion', 'key'")]
    public string itemKey = "coin";

    [Tooltip("Menge, die beim Einsammeln hinzugefügt wird")]
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ItemManager.Instance != null)
            {
                ItemManager.Instance.AddItem(itemKey, amount);
            }
            else
            {
                Debug.LogError("ItemManager.Instance ist null! Stelle sicher, dass ein ItemManager in der Szene ist.");
            }

            Destroy(gameObject);
        }
    }
}
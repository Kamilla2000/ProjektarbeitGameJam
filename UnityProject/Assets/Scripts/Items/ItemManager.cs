using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    // Speichert, wie viele Items von welchem Typ vorhanden sind
    private Dictionary<string, int> itemCounts = new();

    [System.Serializable]
    public class ItemUI
    {
        [Tooltip("Interner Schlüssel, z.B. 'coin', 'apple', 'diamond'")]
        public string itemKey;

        [Tooltip("Anzeigename im UI, z.B. 'Münzen', 'Äpfel'")]
        public string displayName;

        [Tooltip("Textfeld, das den Wert anzeigt")]
        public TextMeshProUGUI uiText;
    }

    [Header("Item UI Zuweisungen")]
    [Tooltip("Trage hier alle Items mit Textfeldern ein")]
    public List<ItemUI> itemUIList;

    private Dictionary<string, TextMeshProUGUI> uiDictionary;
    private Dictionary<string, string> nameDictionary;

    private void Awake()
    {
        // Singleton erstellen
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persistent
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitUIDictionaries();
    }

    private void Start()
    {
        UpdateAllUI();
    }

    private void InitUIDictionaries()
    {
        uiDictionary = new();
        nameDictionary = new();

        foreach (var entry in itemUIList)
        {
            if (!uiDictionary.ContainsKey(entry.itemKey))
            {
                uiDictionary[entry.itemKey] = entry.uiText;
                nameDictionary[entry.itemKey] = entry.displayName;

                // Startwert setzen
                if (!itemCounts.ContainsKey(entry.itemKey))
                    itemCounts[entry.itemKey] = 0;
            }
            else
            {
                Debug.LogWarning($"ItemKey '{entry.itemKey}' ist doppelt vergeben!");
            }
        }
    }

    public void AddItem(string itemKey, int amount)
    {
        if (!itemCounts.ContainsKey(itemKey))
            itemCounts[itemKey] = 0;

        itemCounts[itemKey] += amount;
        UpdateUI(itemKey);
    }

    private void UpdateUI(string itemKey)
    {
        if (uiDictionary.TryGetValue(itemKey, out var uiText) &&
            nameDictionary.TryGetValue(itemKey, out var displayName))
        {
            uiText.text = $"{displayName}: {itemCounts[itemKey]}";
        }
        else
        {
            Debug.LogWarning($"Kein UI für ItemKey '{itemKey}' gefunden.");
        }
    }

    private void UpdateAllUI()
    {
        foreach (var itemKey in itemCounts.Keys)
        {
            UpdateUI(itemKey);
        }
    }
}
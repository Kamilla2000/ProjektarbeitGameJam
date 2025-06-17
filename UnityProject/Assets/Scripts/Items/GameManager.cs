using UnityEngine;
using TMPro; // Oder: using UnityEngine.UI; wenn du normales UI-Text-Element nutzt

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coinCount = 0;
    public TextMeshProUGUI coinText; // Weist du im Inspector zu

    void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coinCount;
    }
}
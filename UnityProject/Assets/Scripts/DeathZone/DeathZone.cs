using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class DeathZone : MonoBehaviour
{
    public GameObject endScreen;            // Das UI-Panel, das angezeigt wird
    public TextMeshProUGUI endMessageText;  // Der Text auf dem Endscreen
    public string deathMessage = "Du bist gestorben.";

    private void Reset()
    {
        // Damit der BoxCollider automatisch als Trigger gesetzt wird, wenn das Script hinzugefügt wird
        BoxCollider box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowEndScreen(deathMessage);
            Time.timeScale = 0f; // Pausiert das Spiel
        }
    }

    private void ShowEndScreen(string message)
    {
        if (endScreen != null && endMessageText != null)
        {
            endScreen.SetActive(true);
            endMessageText.text = message;
        }
        else
        {
            Debug.LogWarning("Endscreen oder Endnachricht nicht gesetzt!");
        }
    }
}

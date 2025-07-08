using UnityEngine;
using TMPro;

public class PrincessDialogSystem : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogUI; // The UI panel that displays the dialogue
    public TextMeshProUGUI dialogText; // The text component for displaying dialogue lines

    [Header("Dialogues when kissed")]
    [TextArea]
    public string[] kissReplies; // Lines shown when the princess is kissed (attacked)

    [Header("Dialogues when player approaches")]
    [TextArea]
    public string[] approachReplies; // Lines shown when the player gets close

    private bool isPlayerNear = false;

    void Start()
    {
        // Make sure the dialogue UI is hidden on start
        if (dialogUI != null)
            dialogUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player enters the trigger, show an approach dialogue
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            ShowRandomApproachReply();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player leaves, hide the dialogue UI
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (dialogUI != null)
                dialogUI.SetActive(false);
        }
    }

    // Shows a random dialogue line when the player approaches
    public void ShowRandomApproachReply()
    {
        if (approachReplies.Length == 0 || dialogUI == null || dialogText == null) return;

        dialogUI.SetActive(true);
        dialogText.text = approachReplies[Random.Range(0, approachReplies.Length)];
    }

    // Shows a random dialogue line when the princess is kissed (attacked)
    public void ShowRandomKissReply()
    {
        if (kissReplies.Length == 0 || dialogUI == null || dialogText == null) return;

        dialogUI.SetActive(true);
        dialogText.text = kissReplies[Random.Range(0, kissReplies.Length)];
    }

    // Hides the dialogue UI manually
    public void HideDialog()
    {
        if (dialogUI != null)
            dialogUI.SetActive(false);
    }
}

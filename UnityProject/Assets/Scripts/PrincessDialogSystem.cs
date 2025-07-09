using UnityEngine;
using TMPro;

public class PrincessDialogSystem : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;

    [Header("Dialogues when kissed")]
    [TextArea]
    public string[] kissReplies;

    [Header("Dialogues when player approaches")]
    [TextArea]
    public string[] approachReplies;

    [Header("Dialogues when healed")]
    [TextArea]
    public string[] healReplies;

    private bool isPlayerNear = false;
    private bool isDisabled = false; // ✅ disables dialog completely

    void Start()
    {
        if (dialogUI != null)
            dialogUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDisabled) return;

        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            ShowRandomApproachReply();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isDisabled) return;

        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            HideDialog();
        }
    }

    public void ShowRandomApproachReply()
    {
        if (isDisabled || approachReplies.Length == 0 || dialogUI == null || dialogText == null) return;

        dialogUI.SetActive(true);
        dialogText.text = approachReplies[Random.Range(0, approachReplies.Length)];
    }

    public void ShowRandomKissReply()
    {
        if (isDisabled || kissReplies.Length == 0 || dialogUI == null || dialogText == null) return;

        dialogUI.SetActive(true);
        dialogText.text = kissReplies[Random.Range(0, kissReplies.Length)];
    }

    public void ShowRandomHealReply()
    {
        if (isDisabled || healReplies.Length == 0 || dialogUI == null || dialogText == null) return;

        dialogUI.SetActive(true);
        dialogText.text = healReplies[Random.Range(0, healReplies.Length)];
    }

    public void HideDialog()
    {
        if (dialogUI != null)
            dialogUI.SetActive(false);
    }

    // ✅ Call this when she meets the prince to stop all further dialogue
    public void DisableDialog()
    {
        isDisabled = true;
        HideDialog();
    }
}

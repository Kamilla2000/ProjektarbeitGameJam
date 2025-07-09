using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public Button nextButton;

    [Header("Dialogue")]
    [TextArea] public string[] lines;
    public float typingSpeed = 0.03f;

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        StartCoroutine(BeginDialogSequence());
    }

    IEnumerator BeginDialogSequence()
    {
        Time.timeScale = 0f;

        // ??????? ?????????? ?????? ?????
        FindObjectOfType<CameraSwitcher>()?.ShowBossCamera();

        dialogPanel.SetActive(true);
        currentLine = 0;
        nextButton.onClick.AddListener(NextLine);
        ShowLine();

        yield return null;
    }

    void ShowLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(lines[currentLine]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char c in line)
        {
            dialogText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogText.text = lines[currentLine];
            isTyping = false;
            return;
        }

        currentLine++;
        if (currentLine < lines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        Time.timeScale = 1f;

        // ????????????? ?? ??????? ??????
        FindObjectOfType<CameraSwitcher>()?.ShowGameplayCamera();

        dialogPanel.SetActive(false);
        nextButton.onClick.RemoveListener(NextLine);
    }
}

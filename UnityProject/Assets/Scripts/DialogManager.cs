using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject sceneCamera;
    public GameObject gameplayCamera;

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
        // Start cinematic scene
        StartCoroutine(BeginDialogSequence());
    }

    IEnumerator BeginDialogSequence()
    {
        Time.timeScale = 0f;

        if (sceneCamera != null) sceneCamera.SetActive(true);
        if (gameplayCamera != null) gameplayCamera.SetActive(false);

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

        if (sceneCamera != null) sceneCamera.SetActive(false);
        if (gameplayCamera != null) gameplayCamera.SetActive(true);

        dialogPanel.SetActive(false);
        nextButton.onClick.RemoveListener(NextLine);
    }
}

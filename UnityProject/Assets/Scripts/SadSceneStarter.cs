using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DialogueEditor;

public class SadSceneStarter : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private NPCConversation myConversation;

    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button returnToMenuButton;

    private bool hasStarted = false;

    private void Start()
    {
         
        if (winPanel != null)
            winPanel.SetActive(false);

        
        if (returnToMenuButton != null)
            returnToMenuButton.onClick.AddListener(ReturnToMenu);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasStarted && other.CompareTag("Player"))
        {
            hasStarted = true;

           
            ConversationManager.Instance.StartConversation(myConversation);

             
            ConversationManager.OnConversationEnded += OnConversationEnded;
        }
    }

    private void OnConversationEnded()
    {
         
        ConversationManager.OnConversationEnded -= OnConversationEnded;

        // Pause the game
        Time.timeScale = 0f;

        // Show the win panel
        if (winPanel != null)
            winPanel.SetActive(true);
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene("Menu"); 
    }
}

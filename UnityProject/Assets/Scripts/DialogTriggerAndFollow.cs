using UnityEngine;
using DialogueEditor; 

public class DialogTriggerAndFollow : MonoBehaviour
{
    [SerializeField] private NPCConversation introConversation;
    [SerializeField] private GameObject enemy;       
    [SerializeField] private GameObject moneyPath;    
    private bool dialogueStarted = false;
    private bool triggered = false;

    private void OnTriggerStay(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            triggered = true;
            dialogueStarted = true;
            ConversationManager.Instance.StartConversation(introConversation);
        }
    }

    void Update()
    {
        if (dialogueStarted && !ConversationManager.Instance.IsConversationActive)
        {
            dialogueStarted = false;
            ActivateAfterDialogue();
        }
    }

    void ActivateAfterDialogue()
    {
        if (enemy != null)
            enemy.SetActive(true);

        if (moneyPath != null)
            moneyPath.SetActive(true);

      
    }
}
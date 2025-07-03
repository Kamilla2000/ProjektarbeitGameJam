using System.Collections;
using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation; // Link to the dialogue
    [SerializeField] private TaskManager taskManager;        // Link to TaskManager

    private bool hasStarted = false; // Prevent starting multiple times

    private void OnTriggerStay(Collider other)
    {
        // If player is inside trigger and hasn't started the convo yet
        if (other.CompareTag("Player") && !hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Start the conversation
                ConversationManager.Instance.StartConversation(myConversation);
                hasStarted = true;

                // Wait until the dialogue is over
                StartCoroutine(WaitForConversationEnd());
            }
        }
    }

    private IEnumerator WaitForConversationEnd()
    {
        // Wait until the dialogue ends
        yield return new WaitUntil(() => !ConversationManager.Instance.IsConversationActive);

        // Tell the player controller that dialogue is finished
        var controller = FindAnyObjectByType<AnimationAndMovementController>();
        if (controller != null)
        {
            controller.SetDialogueFinished();
        }

        // Now start showing tasks
        taskManager.StartTasks();
    }
}
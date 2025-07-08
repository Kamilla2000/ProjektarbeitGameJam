using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor; 

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private string nextSceneName = "Mariia_FULL";

    private bool hasStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasStarted) return;

        if (other.CompareTag("Player"))
        {
            hasStarted = true;
            ConversationManager.Instance.StartConversation(myConversation);
            StartCoroutine(WaitForDialogueAndLoadScene());
        }
    }

    private IEnumerator WaitForDialogueAndLoadScene()
    {
         
        while (ConversationManager.Instance.IsConversationActive)
        {
            yield return null;
        }

         
        SceneManager.LoadScene(nextSceneName);
    }
}

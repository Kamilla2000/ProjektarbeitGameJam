using UnityEngine;

public class PrincessDialogTrigger : MonoBehaviour
{
    public GameObject dialogUI; 

    private void Start()
    {
        if (dialogUI != null)
            dialogUI.SetActive(false);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dialogUI != null)
        {
            dialogUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && dialogUI != null)
        {
            dialogUI.SetActive(false);
        }
    }
}

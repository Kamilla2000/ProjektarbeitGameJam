using UnityEngine;
using UnityEngine.UI;

public class TriggerPrincessUIPanel : MonoBehaviour
{
    [Header("UI Panel to Show")]
    public GameObject panelToShow;

    private void Start()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(false); // Stelle sicher, dass Panel beim Start deaktiviert ist
        }
        else
        {
            Debug.LogWarning("UI Panel ist nicht zugewiesen im Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Princess") && panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeathUIController : MonoBehaviour
{
    public GameObject deathPanel;   // Reference to the death UI panel
    public Button restartButton;    // Reference to the restart button on the panel

    private void Start()
    {
        // Hide the death panel at the start
        if (deathPanel != null)
            deathPanel.SetActive(false);

        // Attach the RestartScene method to the restart button click event
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartScene);

        // Find the PlayerHealthPA instance and subscribe to its OnDeath event
        var playerHealth = FindObjectOfType<PlayerHealthPA>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath += ShowDeathPanel;
        }
    }

    // Method to show the death UI panel when the player dies
    public void ShowDeathPanel()
    {
        if (deathPanel != null)
            deathPanel.SetActive(true);
    }

    // Method to restart the current scene when restart button is pressed
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

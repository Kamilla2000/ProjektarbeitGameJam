using UnityEngine;

public class VillageNPCTrigger : MonoBehaviour
{
    public GameObject[] npcsToActivate; // List of NPCs that should appear when player enters the village

    private bool triggered = false; // To make sure it only runs once

    void OnTriggerEnter(Collider other)
    {
        // Don’t trigger again if already done
        if (triggered) return;

        // Check if it's the player entering the trigger zone
        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Activate all NPCs in the list
            foreach (GameObject npc in npcsToActivate)
            {
                npc.SetActive(true);
            }
        }
    }
}

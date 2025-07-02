using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public NPCBehavior npc; // Reference to the NPC that should appear

    private bool triggered = false; // Prevents it from triggering more than once

    void OnTriggerEnter(Collider other)
    {
        // Check if player enters the trigger and it hasn't already been triggered
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            npc.SpawnNPC(); // Call the method to make NPC appear and move

            Destroy(gameObject); // Remove the trigger so it can't be reused
        }
    }
}

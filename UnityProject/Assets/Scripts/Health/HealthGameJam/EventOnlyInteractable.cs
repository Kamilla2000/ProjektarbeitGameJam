using UnityEngine;

public class EventOnlyInteractable : MonoBehaviour
{
    public float damageAmount = 10f; // Wie viel Schaden zugefügt wird

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthPA playerHealth = other.GetComponent<PlayerHealthPA>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamege(damageAmount);
            }
        }
    }

}
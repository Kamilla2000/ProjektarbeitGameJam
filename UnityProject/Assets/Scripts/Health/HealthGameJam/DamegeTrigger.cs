using UnityEngine;
using System.Collections;

public class DamageTrigger : MonoBehaviour
{
    public float damageAmountPerSecond = 10f; // Schaden pro Sekunde

    private Coroutine damageCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthPA playerHealth = other.GetComponent<PlayerHealthPA>();
            if (playerHealth != null)
            {
                // Starte Coroutine für kontinuierlichen Schaden
                damageCoroutine = StartCoroutine(DealDamageOverTime(playerHealth));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stoppe Schaden, wenn der Spieler rausgeht
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamageOverTime(PlayerHealthPA playerHealth)
    {
        while (true)
        {
            playerHealth.TakeDamege(damageAmountPerSecond);
            yield return new WaitForSeconds(1f);
        }
    }
}
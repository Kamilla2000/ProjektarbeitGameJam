using UnityEngine;
using System.Collections;

public class DamageTriggerForPrincess : MonoBehaviour
{
    public float damageAmountPerSecond = 10f;

    private Coroutine damageCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Princess"))
        {
            var playerHealth = other.GetComponent<PlayerHealthWithKissAndGameOver>();
            if (playerHealth != null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(playerHealth));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Princess"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamageOverTime(PlayerHealthWithKissAndGameOver playerHealth)
    {
        while (true)
        {
            playerHealth.TakeDamage(damageAmountPerSecond);
            yield return new WaitForSeconds(1f);
        }
    }
}

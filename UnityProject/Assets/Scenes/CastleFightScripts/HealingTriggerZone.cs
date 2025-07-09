using UnityEngine;

public class HealingTriggerZone : MonoBehaviour
{
    public float healAmount = 20f;
    public bool destroyOnUse = true;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthWithKissAndGameOver health = other.GetComponent<PlayerHealthWithKissAndGameOver>();

        if (health == null && other.transform.parent != null)
        {
            health = other.transform.parent.GetComponent<PlayerHealthWithKissAndGameOver>();
        }

        if (health != null)
        {
            health.Heal(healAmount);

            if (destroyOnUse)
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class HealthTrigger : MonoBehaviour
{
    public float healAmount = 20f;
    public bool destroyOnUse = true;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthPA health = other.GetComponent<PlayerHealthPA>();

        if (health == null && other.transform.parent != null)
        {
            health = other.transform.parent.GetComponent<PlayerHealthPA>();
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
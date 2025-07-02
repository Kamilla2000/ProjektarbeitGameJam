using UnityEngine;

public class HealthTrigger : MonoBehaviour
{
    public float healAmount = 20f;
    public bool destroyOnUse = true;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthPA player = other.GetComponent<PlayerHealthPA>();
        if (player != null)
        {
            player.Heal(healAmount);
            if (destroyOnUse)
            {
                gameObject.SetActive(false); // oder: Destroy(gameObject);
            }
        }
    }
}
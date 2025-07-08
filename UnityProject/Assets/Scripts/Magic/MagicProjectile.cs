using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float damage = 25f;  // Amount of damage this projectile deals

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyChasingDieAndDamage>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage); // <== FIX: convert float to int
            }
        }

        Destroy(gameObject);
    }
}

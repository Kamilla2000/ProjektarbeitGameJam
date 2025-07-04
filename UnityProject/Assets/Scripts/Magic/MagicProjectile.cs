using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float damage = 25f;  // Amount of damage this projectile deals

    // Called when this projectile collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Try to get the BearPatrol script component on the enemy
            var enemy = other.GetComponent<BearPatrol>();
            if (enemy != null)
            {
                // Apply damage to the enemy
                enemy.TakeDamage(damage);
            }
        }

        // Destroy this magic projectile after hitting anything
        Destroy(gameObject);
    }
}

using UnityEngine;

public class ParticleDamageDealer : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyChasingDieAndDamage enemy = other.GetComponent<EnemyChasingDieAndDamage>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class EnemyChasingDieAndDamage : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    public Slider healthBar;

    [SerializeField] private GameObject heartPickupPrefab; //  Prefab to spawn after death

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthBar.maxValue = HP;
        healthBar.value = HP;
    }

    void Update()
    {
        healthBar.value = HP;
    }

    // Triggered when particle collides with enemy
    private void OnParticleCollision(GameObject other)
    {
        if (isDead) return;

        if (other.CompareTag("Rain"))
        {
            Debug.Log("Rain hit – instant death");
            TakeDamage(HP); // Kill instantly
        }
        else
        {
            Debug.Log("Standard particle hit – 20 damage");
            TakeDamage(20); // Deal 20 damage
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        HP -= damageAmount;

        if (HP <= 0)
        {
            HP = 0;
            isDead = true;

            animator.SetTrigger("die");

            // Remove rigidbody so it doesn't interfere with ragdoll/animation
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb);
            }

            // Delay heart spawn to let death animation play
            Invoke(nameof(SpawnHeart), 2f); //  Adjust delay to match your death animation
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    private void SpawnHeart()
    {
        if (heartPickupPrefab != null)
        {
            Instantiate(heartPickupPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // remove enemy after heart appears
    }

    public bool IsDead()
    {
        return isDead;
    }
}

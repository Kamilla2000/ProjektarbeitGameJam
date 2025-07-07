using UnityEngine;
using UnityEngine.UI;

public class EnemyChasingDieAndDamage : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    public Slider healthBar;

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

    // Wird aufgerufen, wenn Partikel das Objekt treffen
    private void OnParticleCollision(GameObject other)
    {
        if (isDead) return;

        if (other.CompareTag("Rain"))
        {
            Debug.Log("Rain hit – instant death");
            TakeDamage(HP); // Sofort töten
        }
        else
        {
            Debug.Log("Standard particle hit – 20 damage");
            TakeDamage(20); // Nur 20 Schaden
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

            // Nur Rigidbody löschen
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb);
            }

            // Collider bleibt aktiv!
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
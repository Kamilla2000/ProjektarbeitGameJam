using UnityEngine;
using UnityEngine.UI;

public class EnemyChasingDieAndDamage: MonoBehaviour
{
    [SerializeField] private int HP = 100;
    public Slider healthBar;

    private Animator animator;
    public GameObject fireBall;
    public Transform fireBallPoint;

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

    //public void Scream()
    //{
        //FindObjectOfType<AudioManager>().Play("DragonScream");
    //}

    //public void Attack()
    //{
        //FindObjectOfType<AudioManager>().Play("DragonAttack");
    //}

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            HP = 0;
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;

            //FindObjectOfType<AudioManager>().Play("DragonDeath");
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }
}
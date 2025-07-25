﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyChasingDieAndDamage : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int HP = 100;
    public Slider healthBar;

    [Header("Death & Pickup")]
    [SerializeField] private GameObject heartPickupPrefab;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int attackDamage = 10;
    private float lastAttackTime;

    [Header("Senses")]
    public Eyes eyes;
    public Ears ears;

    private Animator animator;
    private NavMeshAgent agent;
    private bool isDead = false;
    private Transform princessTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthBar.maxValue = HP;
        healthBar.value = HP;

        GameObject princess = GameObject.FindGameObjectWithTag("Princess");
        if (princess != null)
        {
            princessTarget = princess.transform;
        }
        else
        {
            Debug.LogError("⚠ Kein Objekt mit Tag 'Princess' gefunden.");
        }
    }

    void Update()
    {
        healthBar.value = HP;

        if (isDead || princessTarget == null) return;

        float distance = Vector3.Distance(transform.position, princessTarget.position);

        // Chase logic only when senses detect player
        if ((eyes != null && eyes.IsDetecting) || (ears != null && ears.IsDetecting))
        {
            animator.SetBool("isChasing", true);
            agent.SetDestination(princessTarget.position);
        }

        // Attack logic
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("attack");
            lastAttackTime = Time.time;

            PlayerHealthPA princessHealth = princessTarget.GetComponent<PlayerHealthPA>();
            if (princessHealth != null)
            {
                princessHealth.TakeDamege(attackDamage);
            }
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
            agent.enabled = false;

            Invoke(nameof(SpawnHeart), 2f);
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

        Destroy(gameObject);
    }

    public bool IsDead() => isDead;

    private void OnParticleCollision(GameObject other)
    {
        if (isDead) return;
        TakeDamage(other.CompareTag("Rain") ? HP : 20);
    }
}
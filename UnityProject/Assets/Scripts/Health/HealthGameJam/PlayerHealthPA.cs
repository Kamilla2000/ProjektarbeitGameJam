using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthPA : MonoBehaviour
{
    [Header("Health Bar")]
    public int maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TMPro.TextMeshProUGUI healthText;

    [Header("Damage Overlay")]
    public Image damagePanel;               // Panel that flashes red on damage
    public float flashSpeed = 2f;           // Speed of fading out
    public float maxAlpha = 0.5f;           // Max alpha when hit

    private float health;
    private float lerpTimer;
    private bool isDead = false;

    private Animator animator;
    private AnimationAndMovementController movementController;

    public delegate void OnDeathHandler();
    public event OnDeathHandler OnDeath;

    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        movementController = GetComponent<AnimationAndMovementController>();
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        FadeDamagePanel(); // Handle alpha fading
    }

    private void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }

        healthText.text = Mathf.RoundToInt(health).ToString();
    }

    public void TakeDamege(float damage)
    {
        if (isDead) return;

        health -= damage;
        lerpTimer = 0f;

        // Show damage panel with full alpha
        if (damagePanel != null)
        {
            var color = damagePanel.color;
            color.a = maxAlpha;
            damagePanel.color = color;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void FadeDamagePanel()
    {
        if (damagePanel != null && damagePanel.color.a > 0f)
        {
            var color = damagePanel.color;
            color.a = Mathf.Lerp(color.a, 0f, flashSpeed * Time.deltaTime);
            damagePanel.color = color;
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        lerpTimer = 0f;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null)
            animator.SetBool("isDead", true);

        if (movementController != null)
            movementController.enabled = false;

        var collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        OnDeath?.Invoke();
    }
}
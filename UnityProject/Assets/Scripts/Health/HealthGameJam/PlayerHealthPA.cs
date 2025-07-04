using UnityEngine;

public class PlayerHealthPA : MonoBehaviour
{
    [Header("Health Bar")]
    public int maxHealth = 100;               // Max health value
    public float chipSpeed = 2f;              // Speed for health bar interpolation
    public UnityEngine.UI.Image frontHealthBar;  // Front health bar image (green)
    public UnityEngine.UI.Image backHealthBar;   // Back health bar image (red)
    public TMPro.TextMeshProUGUI healthText;     // Health number text

    public float health;                      // Current health

    private float lerpTimer;                  // Timer for smooth health bar update

    private Animator animator;                // Animator reference
    private AnimationAndMovementController movementController; // Movement script reference

    // Delegate and event to notify when player dies
    public delegate void OnDeathHandler();
    public event OnDeathHandler OnDeath;

    private bool isDead = false;              // Dead flag

    private void Start()
    {
        health = maxHealth;                   // Initialize health to max
        animator = GetComponent<Animator>(); // Get Animator component
        movementController = GetComponent<AnimationAndMovementController>(); // Get movement script
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth); // Clamp health

        UpdateHealthUI();                     // Update the health bar visuals
    }

    // Update health bar and text UI
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

    // Method to apply damage to player
    public void TakeDamege(float damage)
    {
        if (isDead) return;                   // Ignore if already dead

        health -= damage;                     // Decrease health
        lerpTimer = 0f;                      // Reset UI timer

        if (health <= 0)                     // If health depleted
        {
            Die();                          // Trigger death
        }
    }

    // Heal method
    public void Heal(float amount)
    {
        if (isDead) return;                   // Can't heal if dead

        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        lerpTimer = 0f;
    }

    // Called on death
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

        OnDeath?.Invoke(); // Notify UI or other scripts
    }
}
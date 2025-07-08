using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthPA : MonoBehaviour
{
    [Header("Health Bar")]
    public int maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    [Header("Damage Overlay")]
    public Image damagePanel;
    public float flashSpeed = 2f;
    public float maxAlpha = 0.5f;

    [Header("References")]
    public PrincessDialogSystem dialogSystem; // kiss reply

    private float currentHealth;
    private float lerpTimer;
    private bool isDead = false;

    private Animator animator;

    public bool isKissed { get; private set; } = false;
    private float kissResetTimer = 0f;
    private float kissDuration = 2f;

    public delegate void OnDeathHandler();
    public event OnDeathHandler OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        FadeDamagePanel();
        HandleKissTimer();
    }

    private void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = currentHealth / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = Mathf.Pow(lerpTimer / chipSpeed, 2);
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = Mathf.Pow(lerpTimer / chipSpeed, 2);
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }

        if (healthText != null)
            healthText.text = Mathf.RoundToInt(currentHealth).ToString();
    }

    public void TakeDamege(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        lerpTimer = 0f;

        if (damagePanel != null)
        {
            var color = damagePanel.color;
            color.a = maxAlpha;
            damagePanel.color = color;
        }

        isKissed = true;
        kissResetTimer = kissDuration;

        if (animator != null)
        {
            animator.SetBool("isKissed", true);
        }

        // kiss reply
        if (dialogSystem != null)
        {
            dialogSystem.ShowRandomKissReply();
        }

        if (currentHealth <= 0)
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

    private void HandleKissTimer()
    {
        if (isKissed)
        {
            kissResetTimer -= Time.deltaTime;
            if (kissResetTimer <= 0f)
            {
                ResetKissState();
            }
        }
    }

    public void ResetKissState()
    {
        isKissed = false;

        if (animator != null)
        {
            animator.SetBool("isKissed", false);
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        lerpTimer = 0f;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null)
            animator.SetBool("isDead", true);

        var collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        OnDeath?.Invoke();
    }
}

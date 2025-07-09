using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealthWithKissAndGameOver : MonoBehaviour
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

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Princess Dialog")]
    public PrincessDialogSystem dialogSystem;

    private float currentHealth;
    private float lerpTimer;

    public bool isKissed { get; private set; } = false;
    private float kissResetTimer = 0f;
    private float kissDuration = 2f;

    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

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

    public void TakeDamage(float damage)
    {
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
            animator.SetBool("isKissed", true); // ???????? ????????

        if (dialogSystem != null)
            dialogSystem.ShowRandomKissReply();

        if (currentHealth <= 0)
            GameOver();
    }

    private void HandleKissTimer()
    {
        if (isKissed)
        {
            kissResetTimer -= Time.deltaTime;
            if (kissResetTimer <= 0f)
            {
                isKissed = false;

                if (animator != null)
                    animator.SetBool("isKissed", false); // ????????? ????????
            }
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
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        lerpTimer = 0f;

        if (dialogSystem != null)
            dialogSystem.ShowRandomHealReply();
    }

    private void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}

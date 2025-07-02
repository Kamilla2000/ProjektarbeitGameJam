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
    public Image overlay;
    public float duration = 2f;
    public float fadeSpeed = 1.5f;

    private float health;
    private float lerpTimer;
    private float durationTimer;

    private void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        // Overlay fading logic
        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;

            if (durationTimer > duration)
            {
                float newAlpha = overlay.color.a - Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, newAlpha);
            }
        }
    }

    public void TakeDamege(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;

        // Show overlay immediately
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1f);
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

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        lerpTimer = 0f;
    }
}

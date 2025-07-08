using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerSlider : MonoBehaviour
{
    [Header("Timer Einstellungen")]
    public Slider timerSlider;
    public TextMeshProUGUI ageText;
    public float totalTime = 600f; // z.B. 10 Minuten
    public int maxAge = 60;        // z.B. Alter von 0 bis 60

    [Header("NPC Animation")]
    public Animator npcAnimator;         // Der Animator deines NPCs
    public string triggerName = "TimeOver"; // Der Trigger im Animator

    private float currentTime = 0f;
    private bool isRunning = true;

    void Start()
    {
        if (timerSlider != null)
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = totalTime;
            timerSlider.value = 0f;
        }

        UpdateAgeText(0);
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime += Time.deltaTime;

        if (currentTime >= totalTime)
        {
            currentTime = totalTime;
            isRunning = false;

            // 👉 Hier wird die NPC-Animation ausgelöst
            if (npcAnimator != null && !string.IsNullOrEmpty(triggerName))
            {
                npcAnimator.SetTrigger(triggerName);
                Debug.Log("⌛ Zeit abgelaufen! NPC spielt Animation.");
            }
        }

        if (timerSlider != null)
        {
            timerSlider.value = currentTime;
        }

        float progress = currentTime / totalTime;
        int currentAge = Mathf.RoundToInt(progress * maxAge);
        UpdateAgeText(currentAge);
    }

    void UpdateAgeText(int age)
    {
        if (ageText != null)
            ageText.text = age.ToString();
    }
}
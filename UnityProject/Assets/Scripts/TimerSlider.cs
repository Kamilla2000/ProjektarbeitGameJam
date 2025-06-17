using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerSlider : MonoBehaviour
{
    public Slider timerSlider;
    public TextMeshProUGUI ageText; 
    public float totalTime = 600f;
    public int maxAge = 60;

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

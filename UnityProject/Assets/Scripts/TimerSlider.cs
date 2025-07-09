using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerSlider : MonoBehaviour
{
    public Slider timerSlider;
    public float totalTime = 120f;

    private float currentTime = 0f;
    private bool hasTriggered = false;

    void Start()
    {
        if (timerSlider != null)
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = totalTime;
            timerSlider.value = 0f;
        }
    }

    void Update()
    {
        if (hasTriggered) return;

        currentTime += Time.deltaTime;

        if (timerSlider != null)
            timerSlider.value = currentTime;

        if (currentTime >= totalTime)
        {
            hasTriggered = true;
            SceneManager.LoadScene("CutScene2");
        }
    }
}

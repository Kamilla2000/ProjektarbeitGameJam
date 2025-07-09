using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RainSpawner : MonoBehaviour
{
    public ParticleSystem rainParticles;
    public Button spawnButton;

    [Header("How long should it rain?")]
    public float rainDuration = 3f;

    [Header("How many points are needed to activate rain?")]
    public int requiredScore = 3;

    private Coroutine rainRoutine;

    void Start()
    {
        if (spawnButton != null)
            spawnButton.onClick.AddListener(ActivateRain);
    }

    void ActivateRain()
    {
        if (ScoreManager.Instance != null && ScoreManager.Instance.score >= requiredScore)
        {
            // Subtract all score when activating rain
            ScoreManager.Instance.score = 0;
            ScoreManager.Instance.UpdateScoreUI(); 

            if (rainParticles != null)
            {
                if (rainRoutine != null)
                    StopCoroutine(rainRoutine);

                rainParticles.Play();
                rainRoutine = StartCoroutine(StopRainAfterDelay(rainDuration));
            }
        }
        else
        {
            Debug.Log("Not enough score to make it rain!");
        }
    }

    IEnumerator StopRainAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
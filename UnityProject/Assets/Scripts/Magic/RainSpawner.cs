using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RainSpawner : MonoBehaviour
{
    public ParticleSystem rainParticles;
    public Button spawnButton;

    [Header("Wie lange soll es regnen?")]
    public float rainDuration = 3f;

    [Header("Wie viele Punkte braucht man, um Regen zu aktivieren?")]
    public int requiredScore = 3;

    private Coroutine rainRoutine;
    private bool hasPlayed = false;

    void Start()
    {
        if (spawnButton != null)
            spawnButton.onClick.AddListener(ActivateRain);
    }

    void ActivateRain()
    {

        if (ScoreManager.Instance != null && ScoreManager.Instance.score >= requiredScore)
        {
            if (rainParticles != null && !rainParticles.isPlaying && !hasPlayed)
            {
                rainParticles.Play();
                hasPlayed = true;

                if (rainRoutine != null)
                    StopCoroutine(rainRoutine);

                rainRoutine = StartCoroutine(StopRainAfterDelay(rainDuration));
            }
        }
        else
        {
            Debug.Log("Nicht Genug!");
        }
    }

    IEnumerator StopRainAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}

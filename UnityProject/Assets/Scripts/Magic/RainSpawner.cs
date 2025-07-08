using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RainSpawner : MonoBehaviour
{
    public ParticleSystem rainParticles;
    public Button spawnButton;

    [Header("Wie lange soll es regnen?")]
    public float rainDuration = 3f;

    private Coroutine rainRoutine;

    void Start()
    {
        if (spawnButton != null)
            spawnButton.onClick.AddListener(ActivateRain);
    }

    void ActivateRain()
    {
        if (rainParticles != null && !rainParticles.isPlaying)
        {
            rainParticles.Play();

            if (rainRoutine != null)
                StopCoroutine(rainRoutine);

            rainRoutine = StartCoroutine(StopRainAfterDelay(rainDuration));
        }
    }

    IEnumerator StopRainAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackgroundMusicPlayer : MonoBehaviour
{
    [Header("Music for this scene")]
    public AudioClip musicClip;
    [Range(0f, 1f)] public float volume = 0.6f;
    public bool loop = true;

    private static AudioSource activeMusic;

    void Start()
    {
        PlaySceneMusic();
    }

    void PlaySceneMusic()
    {
        if (activeMusic != null)
        {
            Destroy(activeMusic.gameObject);
        }

    
        GameObject musicObject = new GameObject("SceneMusic");
        activeMusic = musicObject.AddComponent<AudioSource>();
        activeMusic.clip = musicClip;
        activeMusic.volume = volume;
        activeMusic.loop = loop;
        activeMusic.playOnAwake = false;
        activeMusic.Play();

    
        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(musicObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (activeMusic != null)
        {
            Destroy(activeMusic.gameObject);
            activeMusic = null;
        }

       
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
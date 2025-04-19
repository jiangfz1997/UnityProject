using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public List<MusicMapping> musicMappings = new List<MusicMapping>();
    
    public float masterVolume = 1.0f;
    public float fadeTime = 1.0f;
    public bool playMusicOnAwake = true;
    public bool loopMusic = true;

    private AudioSource audioSource;
    private int currentMusicIndex = -1;
    private int changeMark = -1;
    private bool isFading = false;
    
    [System.Serializable]
    public class MusicMapping
    {
        public string mappingName;
        public string[] levelNames;
        public AudioClip bgmClip;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.loop = loopMusic;
            audioSource.playOnAwake = false;
            audioSource.volume = masterVolume;

            SceneManager.sceneLoaded += OnSceneLoaded;

            if (playMusicOnAwake)
            {
                PlayMusicForCurrentScene();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    public string GetLastLoadeScene() {
    int sceneCount = SceneManager.sceneCount;
    for (int i = sceneCount - 1; i >= 0; i--) {
        Scene scene = SceneManager.GetSceneAt(i);
        if (scene.name != "DontDestroyOnLoad" && scene.name != "Persistent") {
            return scene.name;
        }
    }
    return null;
}
    

    private void PlayMusicForCurrentScene()
    {
        string currentSceneName = GetLastLoadeScene();
        // Debug.Log("Current Scene for BGM: " + currentSceneName);
        if(currentSceneName == null)
            return;
            
        if(currentSceneName == "Boss_1")
        {
            changeMark = 3;
        }

        if(currentSceneName == "BreakRoom") 
        {
            if (changeMark > 0) {
                string newSceneName = "Level_" + changeMark.ToString();
                PlayMusicForScene(newSceneName);
                changeMark = -1;
            }
            return;
        }
        PlayMusicForScene(currentSceneName);
    }

    public void PlayMusicForScene(string sceneName)
    {
        for (int i = 0; i < musicMappings.Count; i++)
        {
            if (System.Array.Exists(musicMappings[i].levelNames, level => level == sceneName))
            {
                PlayMusicByIndex(i);
                return;
            }
        }
        // Debug.LogWarning("No music found");
    }

    public void PlayMusicByIndex(int index)
    {
        // Debug.Log("Play Music Index: " + index);
        if (index < 0 || index >= musicMappings.Count)
        {
            Debug.LogError("Music Index Error");
            return;
        }
        
        if (currentMusicIndex == index && audioSource.isPlaying)
            return;


        MusicMapping mapping = musicMappings[index];

        if (isFading)
        {
            StopAllCoroutines();
            isFading = false;
        }

        StartCoroutine(FadeBGM(mapping.bgmClip, fadeTime));
        currentMusicIndex = index;
    }


    private IEnumerator FadeBGM(AudioClip newClip, float fadeTime)
    {
        isFading = true;
        
        if (audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;
            while (audioSource.volume > 0.01f)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
        }

        // change clip
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < masterVolume - 0.01f)
        {
            audioSource.volume += masterVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        

        audioSource.volume = masterVolume;
        isFading = false;
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
 
    public MusicMapping GetCurrentMusicMapping()
    {
        if (currentMusicIndex >= 0 && currentMusicIndex < musicMappings.Count)
            return musicMappings[currentMusicIndex];
        
        return null;
    }

}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    private GameObject blackOverlay;
    // public bool skipContextForDebug = true; 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        blackOverlay = GameObject.Find("BlackOverlay");

// #if UNITY_EDITOR
//         if (skipContextForDebug)
//         {
//             Debug.Log("Skipping Context Scene in Debug Mode. Loading Level_1...");
//             //blackOverlay.SetActive(false);
//             SceneManager.LoadScene("Level_1", LoadSceneMode.Additive);
//             return;
//         }
// #endif
        // **���� Context Scene���� Persistent ��Ȼ�� Active Scene**
        // SceneManager.LoadScene("Context", LoadSceneMode.Additive);

        // **���� Scene ��������¼�**
        //SceneManager.LoadScene("Level_1", LoadSceneMode.Additive);
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // **ȡ�������������ڴ�й©**
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneConfig[] sceneConfigs = UnityEngine.Object.FindObjectsByType<SceneConfig>(FindObjectsSortMode.None);

        SceneConfig sceneConfig = null;

        // **ֻ���ҵ�ǰ Scene ��� SceneConfig**
        foreach (var config in sceneConfigs)
        {
            if (config.gameObject.scene == scene)  // **ȷ���ǵ�ǰ Scene**
            {
                sceneConfig = config;
                break;
            }
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (sceneConfig != null)
        {
            if (sceneConfig.sceneType == SceneType.Context)
            {
                FreezePlayer();  
                if (blackOverlay)
                {
                    StartCoroutine(FadeOutBlackOverlay());
                }
            }
            else if (sceneConfig.sceneType == SceneType.Level)
            {
                UnfreezePlayer();
            }
        }

    }

    private void FreezePlayer()
    {
        if (player)
        {
            //Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            //rb.bodyType = RigidbodyType2D.Kinematic;
            //rb.linearVelocity = Vector2.zero;
            //rb.constraints = RigidbodyConstraints2D.FreezePosition;
            player.SetActive(false);
        }
    }

    private void UnfreezePlayer()
    {
        if (player)
        {
            //Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            //rb.bodyType = RigidbodyType2D.Dynamic;
            //rb.constraints = RigidbodyConstraints2D.None;
            player.SetActive(true);

        }
    }
    IEnumerator FadeOutBlackOverlay()
    {
        CanvasGroup canvasGroup = blackOverlay.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = blackOverlay.AddComponent<CanvasGroup>(); 
        }

        float duration = 1f; 
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - (elapsedTime / duration);  
            yield return null;
        }

        canvasGroup.alpha = 0;  
        blackOverlay.SetActive(false); 
    }

    public void ReturnToMainMenu()
    {
        CleanDontDestroyOnLoad();
        SceneManager.LoadScene("MainMenu");
    }

    private void CleanDontDestroyOnLoad()
    {
        Destroy(GameObject.Find("MusicManager"));
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("PlayerCamera"));
        Destroy(GameObject.Find("EventSystem"));
        Destroy(GameObject.Find("ScenesManager"));
        Destroy(GameObject.Find("DelayedActionManager"));
        Destroy(GameObject.Find("[DOTween]"));
        Destroy(GameObject.Find("[Debug Updater]"));

    }





}

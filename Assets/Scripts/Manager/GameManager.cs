using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameObject player;
    private GameObject blackOverlay;
    public bool skipContextForDebug = true; // **调试模式时跳过 Context Scene**

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

#if UNITY_EDITOR
        if (skipContextForDebug)
        {
            Debug.Log("Skipping Context Scene in Debug Mode. Loading Level_1...");
            blackOverlay.SetActive(false);
            SceneManager.LoadScene("Level_1", LoadSceneMode.Additive);
            return;
        }
#endif
        // **加载 Context Scene，但 Persistent 仍然是 Active Scene**
        SceneManager.LoadScene("Context", LoadSceneMode.Additive);

        // **监听 Scene 加载完成事件**
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // **取消监听，避免内存泄漏**
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneConfig[] sceneConfigs = Object.FindObjectsByType<SceneConfig>(FindObjectsSortMode.None);

        SceneConfig sceneConfig = null;

        // **只查找当前 Scene 里的 SceneConfig**
        foreach (var config in sceneConfigs)
        {
            if (config.gameObject.scene == scene)  // **确保是当前 Scene**
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
                FreezePlayer();  // **Context 期间禁用 Player 物理**
                if (blackOverlay)
                {
                    StartCoroutine(FadeOutBlackOverlay());
                }
            }
            else if (sceneConfig.sceneType == SceneType.Level)
            {
                UnfreezePlayer();  // **进入 Level 关卡，恢复 Player 物理**
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
            canvasGroup = blackOverlay.AddComponent<CanvasGroup>();  // 如果没有 CanvasGroup，添加一个
        }

        float duration = 1f; // Fade out duration
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - (elapsedTime / duration);  // **调整透明度**
            yield return null;
        }

        canvasGroup.alpha = 0;  // **完全透明**
        blackOverlay.SetActive(false); // **确保完全透明后才禁用 UI**
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Unity.Cinemachine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance { get; private set; }
    public Transform playerTrans;
    public Vector3 firstPos;
    [Header("Event listener")]
    //public static SceneManagerController instance;
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;
    public int fadeSecond;

    [Header("Broadcast")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeScreenEvent;
    [SerializeField] private GameSceneSO currentLoadScene;

    public delegate void CameraEventHandler(Vector3 position, float size, float duration);
    public static event CameraEventHandler OnCameraEvent;
    public string CurrentLevelName => currentLoadScene.sceneName;
    private bool isRestarting = false;
    private Action onRestartComplete; 

    private bool isLoading;
    private GameSceneSO sceneToLoad;
    private Vector3 posToGo;
    private bool fadeScreen;
    private bool makePlayerTeleport=true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    //TODO: Main menu
    private void Start()
    {
        NewGame();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTrans = player.transform;
            playerTrans.gameObject.SetActive(true);
            
        }
        else
        {
            Debug.LogError("Player not found in the scene.");
        }
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        OnLoadRequestEvent(sceneToLoad, firstPos, true);

    }

    public void TriggerCameraEvent(Vector3 position, float size, float duration)
    {
        Debug.Log($"camera: pos={position}, size={size}, duration={duration}");
        OnCameraEvent?.Invoke(position, size, duration);
    }


    private void OnLoadRequestEvent(GameSceneSO sceneToLoad, Vector3 posToLoad, bool fadeScreen, bool makePlayerTeleport=true)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        this.sceneToLoad = sceneToLoad;
        this.makePlayerTeleport = makePlayerTeleport;

        if (makePlayerTeleport) 
        { 
            this.posToGo = posToLoad;
        }
        this.fadeScreen = fadeScreen;

        Debug.Log("Position to load: " + posToLoad);


        Debug.Log("Scene to load: " + sceneToLoad.sceneReference.SubObjectName);
        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadScene());
        }
        else
        {
            LoadNewScene();
        }

    }

    private void OnLoadRequestEventString(string sceneToLoadName, Vector3 posToLoad, bool fadeScreen, bool makePlayerTeleport = true)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        GameSceneSO sceneToLoad = GameSceneLookup.Instance.GetSceneSOByName(sceneToLoadName);
        this.sceneToLoad = sceneToLoad;
        this.makePlayerTeleport = makePlayerTeleport;

        if (makePlayerTeleport)
        {
            this.posToGo = posToLoad;
        }
        this.fadeScreen = fadeScreen;

        Debug.Log("Position to load: " + posToLoad);


        Debug.Log("Scene to load: " + sceneToLoad.sceneReference.SubObjectName);
        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadScene());
        }
        else
        {
            LoadNewScene();
        }

    }
    private IEnumerator UnLoadScene()
    {
        if (fadeScreen)
        {
            fadeScreenEvent.FadeOut(fadeSecond);
        }
        yield return new WaitForSeconds(fadeSecond);


        yield return currentLoadScene.sceneReference.UnLoadScene();

        // if(playerTrans != null)
        // {
        //     playerTrans.gameObject.SetActive(false);
        // } else {
        //     yield return new WaitForEndOfFrame(); // 等待一帧
        //     playerTrans = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        //     Debug.LogError("Player transform is null.");
        // }

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = sceneToLoad;
        if (this.makePlayerTeleport) 
        {
            playerTrans.position = posToGo;
        }
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeScreenEvent.FadeIn(fadeSecond);
        }
     

        SceneManager.SetActiveScene(handle.Result.Scene);
       
        afterSceneLoadedEvent.RaiseEvent();
        StartCoroutine(RefreshEventSystem());

        // 延迟启用 CinemachineBrain
        //StartCoroutine(EnableCinemachineBrainDelayed(brain));
        isLoading = false;
        if (isRestarting && onRestartComplete != null)
        {
            StartCoroutine(DelayedRestartCallback());
        }
        DeathDropManager.Instance.TrySpawnDropInCurrentScene();
    }

    private IEnumerator DelayedRestartCallback()
    {
        yield return new WaitForSeconds(0.2f); // 等待一帧或两帧确保加载完成

        onRestartComplete?.Invoke();

        // ✅ 清除状态
        isRestarting = false;
        onRestartComplete = null;

    }
    private IEnumerator EnableCinemachineBrainDelayed(CinemachineBrain brain)
    {
        yield return new WaitForSeconds(0.1f); // 等待 0.1 秒，确保所有初始化完成
        if (brain != null)
        {
            brain.enabled = true;
            Debug.Log("[Temp Fix] CinemachineBrain 已启用，Main Camera 位置: " + Camera.main.transform.position);
        }
    }

    private IEnumerator RefreshEventSystem()
    {
        yield return new WaitForSeconds(0.1f); // 确保所有初始化完成
        EventSystem eventSystem = FindFirstObjectByType<EventSystem>();
        if (eventSystem != null)
        {
            eventSystem.gameObject.SetActive(false);
            eventSystem.gameObject.SetActive(true);
            Debug.Log("EventSystem refreshed after scene load.");
        }
    }
    public void RestartCurrentLevel()
    {
        if (currentLoadScene == null)
        {
            Debug.LogWarning("No current scene to restart!");
            return;
        }

        Vector3 restartPos = firstPos;
        string targetLoadSceneName = "";
        var savedData = SaveSystem.LoadSpecificObject<PlayerStatsSaveData>("savegame.json", "PlayerStats");
        if (savedData != null)
        {
            restartPos = savedData.position;
            targetLoadSceneName = savedData.currentScene;
        }


        isRestarting = true;

        // ✅ 设置回调，等场景加载完之后自动调用 Respawn 和 Reset
        onRestartComplete = () =>
        {
            Player.Instance.Respawn();
            FindObjectOfType<DeathScreenController>()?.ResetDeathScreen();
        };

        OnLoadRequestEventString(targetLoadSceneName, restartPos, true, false);
        //OnLoadRequestEventString(targetLoadSceneName, restartPos, true, false);
        //SaveSystem.LoadGame();
    }
   



}

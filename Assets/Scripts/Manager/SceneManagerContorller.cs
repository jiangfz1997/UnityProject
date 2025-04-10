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



    private bool isLoading;
    private GameSceneSO sceneToLoad;
    private Vector3 posToGo;
    private bool fadeScreen;

    void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
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


    private void OnLoadRequestEvent(GameSceneSO sceneToLoad, Vector3 posToLoad, bool fadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        this.sceneToLoad = sceneToLoad;
        this.posToGo = posToLoad;
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
        playerTrans.position = posToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeScreenEvent.FadeIn(fadeSecond);
        }
        //// 禁用 CinemachineBrain（如果尚未禁用）
        //var brain = Camera.main.GetComponent<CinemachineBrain>();
        //if (brain != null)
        //{
        //    brain.enabled = false;
        //    Debug.Log("[Temp Fix] CinemachineBrain 已禁用");
        //}

        SceneManager.SetActiveScene(handle.Result.Scene);
       
        // 触发场景加载完成事件
        afterSceneLoadedEvent.RaiseEvent();
        StartCoroutine(RefreshEventSystem());

        // 延迟启用 CinemachineBrain
        //StartCoroutine(EnableCinemachineBrainDelayed(brain));
        isLoading = false;
    
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

    //public void LoadScene(string sceneName)
    //{
    //    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    //}


    //public void LoadSceneAdditive(string sceneName)
    //{
    //    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    //}


    //public void UnloadScene(string sceneName)
    //{
    //    if (SceneManager.GetSceneByName(sceneName).isLoaded)
    //    {
    //        SceneManager.UnloadSceneAsync(sceneName);
    //    }
    //}

}

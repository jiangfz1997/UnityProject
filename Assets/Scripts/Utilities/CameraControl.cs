using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System;
public class CameraControl : MonoBehaviour
{
    [Header("Event listener")]
    public VoidEventSO afterSceneLoadedEvent;
    private Vector3 lastCameraPosition;
    private CinemachineConfiner2D confiner2D;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
    private void OnEnable()
    {
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        Debug.Log("OnAfterSceneLoadedEvent");
        GetNewCameraBounds(); 
        
            
    }

    void Start()
    {
        if (Camera.main != null)
        {
            lastCameraPosition = Camera.main.transform.position;
            Debug.Log("[DEBUG] 初始 Main Camera 位置: " + lastCameraPosition);
        }
        //GetNewCameraBounds();
    }
    //void Update()
    //{
    //    if (Camera.main != null && Camera.main.transform.position != lastCameraPosition)
    //    {
    //        //Debug.Log("[DEBUG] Main Camera 位置更新: " + Camera.main.transform.position + " (之前: " + lastCameraPosition + ")", Camera.main.gameObject);
    //        lastCameraPosition = Camera.main.transform.position;
    //    }
    //}
    IEnumerator InitConfinerNextFrame()
    {
        yield return null; // 🚨 延迟一帧等场景完全加载完毕


        var playerCamera = GameObject.Find("PlayerCamera");
        var confiner = playerCamera.GetComponent<CinemachineConfiner2D>();

        var bound = GameObject.Find("CameraBound").GetComponent<Collider2D>();

        if (confiner && bound)
        {
            confiner.BoundingShape2D = bound;
            confiner.InvalidateBoundingShapeCache();
            Debug.Log("✅ Confiner 延迟初始化成功！");
        }
        //CinemachineCamera vcam = playerCamera.GetComponent<CinemachineCamera>();
        //vcam.enabled = false;
        //vcam.enabled = true;
        //if (!vcam)
        //{
        //    Debug.LogWarning("❌ 找不到 PlayerCamera 或 CameraBound！");
        //}
        //playerCamera.SetActive(false);
        //playerCamera.SetActive(true);
        CinemachineCamera vcam = playerCamera.GetComponent<CinemachineCamera>();
        vcam.enabled = false;
        yield return null; // 🚨 延迟一帧等场景完全加载完毕

        vcam.enabled = true;
        if (!vcam)
        {
            Debug.LogWarning("❌ 找不到 PlayerCamera 或 CameraBound！");
        }
    }
    private void GetNewCameraBounds()
    {
        //var playerCamera = GameObject.Find("PlayerCamera");
        //var confiner = playerCamera.GetComponent<CinemachineConfiner2D>();
        //var bound = GameObject.Find("CameraBound").GetComponent<Collider2D>();
        //var obj = GameObject.FindGameObjectWithTag("Bounds");

        //confiner.BoundingShape2D = bound;
        //// Remove the line causing the error
        //// confiner.InvalidatePathCache();

        //// Replace it with the correct method call
        //confiner.InvalidateBoundingShapeCache();
        StartCoroutine(InitConfinerNextFrame());


        //if (obj == null)
        //    return;

        //confiner2D.BoundingShape2D = obj.GetComponent<Collider2D>();
        //Debug.Log(confiner2D.BoundingShape2D);
        //Debug.Log("[DEBUG] Confiner2D 更新前 Main Camera 位置: " + Camera.main.transform.position);

        //confiner2D.InvalidateBoundingShapeCache();
       
        //Debug.Log("[DEBUG] Confiner2D 更新后 Main Camera 位置: " + Camera.main.transform.position);

    }

    //void LateUpdate()
    //{
    //    var brain = Camera.main.GetComponent<CinemachineBrain>();
    //    if (brain != null)
    //    {
    //        Debug.Log("[DEBUG] CinemachineBrain 当前 Active Virtual Camera: " + brain.ActiveVirtualCamera);
    //    }
        
    //}
}

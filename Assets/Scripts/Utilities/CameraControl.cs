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

    private void GetNewCameraBounds()
    {

        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;

        confiner2D.BoundingShape2D = obj.GetComponent<Collider2D>();
        Debug.Log(confiner2D.BoundingShape2D);
        Debug.Log("[DEBUG] Confiner2D 更新前 Main Camera 位置: " + Camera.main.transform.position);

        confiner2D.InvalidateBoundingShapeCache();
       
        Debug.Log("[DEBUG] Confiner2D 更新后 Main Camera 位置: " + Camera.main.transform.position);

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

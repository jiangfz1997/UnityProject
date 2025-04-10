using UnityEngine;

using Unity.Cinemachine;

using System.Collections;

public class ConfinerAutoFixer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(InitConfinerNextFrame());
    }

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
        CinemachineCamera vcam = playerCamera.GetComponent<CinemachineCamera>();
        vcam.enabled = false;
        //vcam.enabled = true;
        if (!vcam)
        {
            Debug.LogWarning("❌ 找不到 PlayerCamera 或 CameraBound！");
        }
        playerCamera.SetActive(false);
        //playerCamera.SetActive(true);
    }
}

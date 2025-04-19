using UnityEngine;


using System.Collections;
using Unity.Cinemachine;

public class CameraDirector : MonoBehaviour
{
    public static CameraDirector Instance { get; private set; }

    public CinemachineCamera vcam;                // 替换为新的 CinemachineCamera
    public Transform playerTarget;                 // 玩家 transform
    private bool isFocusing = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void FocusOnTarget(Transform target, float duration = 2f, float returnTime = 0.5f)
    {
        if (!isFocusing)
            StartCoroutine(FocusRoutine(target, duration, returnTime));
    }

    private IEnumerator FocusRoutine(Transform target, float duration, float returnTime)
    {
        isFocusing = true;

        vcam.Follow = null;
        vcam.LookAt = target;

        yield return new WaitForSeconds(duration);

        vcam.Follow = playerTarget;
        vcam.LookAt = playerTarget;

        yield return new WaitForSeconds(returnTime);
        isFocusing = false;
    }
}


using UnityEngine;
// using Cinemachine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public Camera mainCamera;

    public static CameraController Instance;
    public static Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
            
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator MoveAndZoom(Vector3 targetPos, float targetSize, float duration)
    {
        Debug.Log("Move and Zoom"+targetPos);
        float elapsedTime = 0;
        float startSize = mainCamera.orthographicSize;
        Vector3 startPosition = mainCamera.transform.position;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            t = Mathf.SmoothStep(0, 1, t);
            
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPos, t);
            
            yield return null;
        }
        
        // 确保相机达到目标大小和位置
        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = targetPos;
    }
    public IEnumerator FollowPlayer(float duration)
    {
        Debug.Log("Follow Player"+playerTransform.position);

        float targetSize = 8.56f; // default value

        Vector3 targetPosition = playerTransform.position;
        
        yield return StartCoroutine(MoveAndZoom(targetPosition, targetSize, duration));
    }
}
using UnityEngine;
// using Cinemachine;
using System.Collections;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{

    public CinemachineCamera vcam; // Updated to use CinemachineCamera as CinemachineVirtualCamera is deprecated.
    public float defaultSize = 8.56f; // Default camera size
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

    public IEnumerator Zoom(float targetSize, float duration)
    {
        Debug.Log("Vcm Zooming to: " + targetSize);
        float elapsedTime = 0;
        float startSize = vcam.Lens.OrthographicSize;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            t = Mathf.SmoothStep(0, 1, t);

            vcam.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        vcam.Lens.OrthographicSize = targetSize;
    }

    public void SizeRecovery(float duration)
    {
        StartCoroutine(Zoom(defaultSize, duration));
    }
    
    //public IEnumerator FollowPlayer(float duration)
    //{
    //    playerTransform = Player.Instance.transform;

    //    Debug.Log("Follow Player" + playerTransform.position);

    //    float targetSize = 8.56f; // default value

    //    Vector3 targetPosition = playerTransform.position;

    //    yield return StartCoroutine(MoveAndZoom(targetPosition, targetSize, duration));
    //}

    public void FocusOn(Transform focusTarget, float focusDuration = 2f, float returnDuration = 0.5f, float cameraSize=8.56f)
    {
        StartCoroutine(FocusRoutine(focusTarget, focusDuration, returnDuration, cameraSize));
    }

    private IEnumerator FocusRoutine(Transform focusTarget, float focusDuration, float returnDuration, float targetSize=8.56f)
    {
        playerTransform = Player.Instance?.transform; 
        if (focusTarget == null || playerTransform == null)
        {
            Debug.LogWarning("Focus target or player transform is null!");
            yield break;
        }


        Transform originalFollow = vcam.Follow;
        Transform originalLookAt = vcam.LookAt;

        float originalSize = vcam.Lens.OrthographicSize;

        vcam.Follow = focusTarget;
        vcam.LookAt = focusTarget;

        yield return StartCoroutine(AdjustLensSize(originalSize, targetSize, focusDuration * 0.5f));

        yield return new WaitForSeconds(focusDuration);


        vcam.Follow = playerTransform;
        vcam.LookAt = playerTransform;

        yield return StartCoroutine(AdjustLensSize(targetSize, originalSize, returnDuration));
    }

    private IEnumerator AdjustLensSize(float startSize, float targetSize, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);
            vcam.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }

        vcam.Lens.OrthographicSize = targetSize;
    }

    public IEnumerator FollowPlayer(float duration)
    {
        if (playerTransform == null)
        {
            playerTransform = Player.Instance?.transform;
            if (playerTransform == null)
            {
                Debug.LogWarning("Player transform is null!");
                yield break;
            }
        }

        Debug.Log("Follow Player: " + playerTransform.position);

        vcam.Follow = playerTransform;
        vcam.LookAt = playerTransform;

        float targetSize = 8.56f; 
        float startSize = vcam.Lens.OrthographicSize;
        yield return StartCoroutine(AdjustLensSize(startSize, targetSize, duration));
    }
}

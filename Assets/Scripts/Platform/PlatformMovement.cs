using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlatformMovement : MonoBehaviour, IActivatable
{
    public PlatformData settings;  
    public float speed = 2.0f;  
    public Vector3[] waypoints;  
    public bool isActivated = true;
    private int currentTargetIndex = 0;

    [Header("Shake Settings")]
    public float shakeDuration = 0.5f;
    public float shakeStrength = 0.1f;

    public void Activate()
    {
        isActivated = true;
        StartCoroutine(ShakeRoutine());
    }
    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;
        Vector3 originalPos = transform.position;

        while (elapsed < shakeDuration)
        {
            float offsetX = (Random.value - 0.5f) * 2f * shakeStrength;
            float offsetY = (Random.value - 0.5f) * 2f * shakeStrength;

            transform.position = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        
        transform.position = originalPos;
    }
    void Start()
    {
        if (settings != null)
        {
            speed = settings.speed;
        }
    }

    void Update()
    {
        if (!isActivated || waypoints == null || waypoints.Length < 2)
            return;
        


        Vector3 target = waypoints[currentTargetIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
        }
    }
}

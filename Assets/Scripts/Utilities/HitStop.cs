using UnityEngine;
using System.Collections;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }

    private void Awake()
    {
        Instance = this; 
    }

    public void StopTime(float duration, float slowScale = 0.1f)
    {
        StartCoroutine(HitStopCoroutine(duration, slowScale));
    }

    private IEnumerator HitStopCoroutine(float duration, float slowScale)
    {
        Time.timeScale = slowScale; 
        yield return new WaitForSecondsRealtime(duration); 
        Time.timeScale = 1f; 
    }
}

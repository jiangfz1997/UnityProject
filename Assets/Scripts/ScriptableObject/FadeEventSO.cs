using System.Linq;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public UnityAction<Color, float, bool> OnEventRaised;
    // scene fade out
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }

    // scene fade in
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
    }

    public void RaiseEvent(Color target, float duration, bool fadeIn)
    {
        FadeCanvas fadeCanvas = Object.FindObjectsByType<FadeCanvas>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault();

        if (fadeCanvas != null)
        {
            fadeCanvas.gameObject.SetActive(true); // 🟢 每次触发时启用 FadeCanvas
        }
        OnEventRaised?.Invoke(target, duration, fadeIn);
    }
}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FadeCanvas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Event listener")]
    public FadeEventSO fadeEventSO;
    public Image fadeImage;

    private void OnFadeEvent(Color target, float duration, bool fadeOut)
    {
        fadeImage.DOBlendableColor(target, duration);
    }
    private void OnEnable()
    {
        fadeEventSO.OnEventRaised += OnFadeEvent;
    }
    private void OnDisable()
    {
        fadeEventSO.OnEventRaised -= OnFadeEvent;
    }
}

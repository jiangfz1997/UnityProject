using UnityEngine;
using UnityEngine.UI;

public class ScrollToTop : MonoBehaviour
{
    public ScrollRect scrollRect;
    
    void OnEnable()
    {
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f; // 1-top, 0-bottom
        }
    }
}
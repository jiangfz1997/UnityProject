using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject Panel;             
    
    private bool isPanelActive = false;
    
    void Start()
    {
        Panel.SetActive(false);
    }
    
    public void TogglePanel()
    {
        if (isPanelActive)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        isPanelActive = true;
        Panel.SetActive(true);
    }
    
    public void HidePanel()
    {
        isPanelActive = false;
        Panel.SetActive(false);
    }
}
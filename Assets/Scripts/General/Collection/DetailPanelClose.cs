using UnityEngine;
using UnityEngine.InputSystem;

public class DetailPanelClose : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public GameObject panel;
    
    void Update()
    {
        if (panel.activeSelf)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
            {
                panel.SetActive(false);
            }
        }
    }
}

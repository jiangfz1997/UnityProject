using System.Collections.Generic;
using UnityEngine;

public class ElementUIManager : MonoBehaviour
{
    [Header("Element UI References")]
    [SerializeField] private ElementIconUI element0;
    [SerializeField] private ElementIconUI element1;
    [SerializeField] private GameObject SwitchInfo;


    private ElementSystem elementSystem;

    private void Start()
    {
        elementSystem = Player.Instance.GetComponent<ElementSystem>();

        elementSystem.OnElementChanged += RefreshElementIcons;
        elementSystem.OnCooldownStarted += HandleCooldownStart;

        //RefreshElementIcons(); // 初始化一下
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    elementSystem.SwitchElement();
        //    UpdateSelection(); // 更新哪个是当前选中
        //}
    }

    public void RefreshElementIcons()
    {
        Debug.Log("RefreshElementIcons called");
        List<ElementType> elements = Player.Instance.GetAvailableElements();
        Debug.Log($"Available elements: {string.Join(", ", elements)}");
        if (elements.Count > 0)
        {
            element0.gameObject.SetActive(true);
            element0.SetElement(elements[0]);
        }
        else
        {
            element0.gameObject.SetActive(false);
        }

        if (elements.Count > 1)
        {
            element1.gameObject.SetActive(true);
            element1.SetElement(elements[1]);
            SwitchInfo.SetActive(true);
        }
        else
        {
            element1.gameObject.SetActive(false);
        }

        UpdateSelection();
    }

    private void UpdateSelection()
    {
        ElementType current = elementSystem.CurrentElement;

        element0.SetSelected(element0.Element == current);
        element1.SetSelected(element1.Element == current);
    }
    private void HandleCooldownStart(float duration)
    {
        var otherIcon = element0.Element == elementSystem.CurrentElement ? element1 : element0;
        otherIcon.StartCooldown(duration);
    }


}

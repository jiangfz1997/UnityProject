using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollectibleItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ItemType { Script, Effect }

    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite lockedSprite; 
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private ItemType type;
    [SerializeField] private int itemIndex; 
    [SerializeField] private bool isCollected = false;

    [SerializeField] private string collectedDescription = "collected";
    [SerializeField] private string lockedDescription = "locked";
    
    [SerializeField] private GameObject tooltipPrefab; 
    [SerializeField] private Canvas canvas; 
    [SerializeField] private int tooltipSortingOrder = 100; // 提示框排序顺序，一直不显示所以一怒之下怒了一下

    private GameObject currentTooltip;

    public int ItemIndex => itemIndex;
    public ItemType Type => type;
    public bool IsCollected => isCollected;

    private void Start()
    {
        UpdateVisual();
    }

    public void SetCollected(bool collected)
    {
        isCollected = collected;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        itemImage.sprite = isCollected ? unlockedSprite : lockedSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void ShowTooltip()
    {
        HideTooltip();

        if (tooltipPrefab == null || canvas == null)
        {
            Debug.LogWarning("Tooltip prefab or canvas is not assigned!");
            return;
        }

        currentTooltip = Instantiate(tooltipPrefab, canvas.transform);

        Canvas tooltipCanvas = currentTooltip.GetComponent<Canvas>();
        if (tooltipCanvas != null)
        {
            tooltipCanvas.overrideSorting = true;
            tooltipCanvas.sortingOrder = tooltipSortingOrder;
        }
        else 
        {
            // 添加一个canvas
            tooltipCanvas = currentTooltip.AddComponent<Canvas>();
            tooltipCanvas.overrideSorting = true;
            tooltipCanvas.sortingOrder = tooltipSortingOrder;
            
            currentTooltip.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }
        
        TMPro.TextMeshProUGUI tooltipText = currentTooltip.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (tooltipText != null)
        {
            tooltipText.text = GetTooltipText();
        }

    }

    private void HideTooltip()
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
            currentTooltip = null;
        }
    }

    private string GetTooltipText()
    {

        if(type == ItemType.Script)
        {
            if(isCollected)
            {
                // return "Click to see more detail about this script.";
                 return "You may click to see more detail in the near future.";
            }
            else
            {
                return "Locked. Can be discovered in " + lockedDescription + ".";
            }
        }
        else if(type == ItemType.Effect)
        {
            if(isCollected)
            {
                return collectedDescription;
            }
            else
            {
                return "Unlock after finding the Script " + (itemIndex + 1) + ".";
            }
        }

        return "";
    }
}
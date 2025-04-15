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
    [SerializeField] private GameObject DetailPanel;
    [SerializeField] private TMPro.TextMeshProUGUI DetailTitle;
    [SerializeField] private TMPro.TextMeshProUGUI DetailContent;
    [TextArea(10, 30)]
    [SerializeField] private string ContentText = "";

    [SerializeField] private string collectedDescription = "collected";
    [SerializeField] private string lockedDescription = "locked";
    
    [SerializeField] private GameObject tooltipPrefab; 
    [SerializeField] private Canvas canvas; 
    [SerializeField] private int tooltipSortingOrder = 100; 

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
                return "Click to see more detail about this script.";
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

    public void ShowDetailPanel()
    {
        if(!isCollected)
            return;
        if (DetailPanel != null)
        {
            DetailTitle.text = "Scene " + (itemIndex+1);
            DetailContent.text = ContentText;
            DetailPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Failed to show. Detail panel is not assigned!");
        }
    }
}
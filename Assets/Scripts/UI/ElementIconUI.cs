using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class ElementIconUI : MonoBehaviour
{
    [SerializeField] private Transform prefabMountPoint;

    private GameObject currentInstance;
    private bool isCurrentlySelected = false;

    private float pendingCooldownDuration = 0f;

    private readonly Dictionary<ElementType, string> ElementNames = new()
    {
        { ElementType.Fire, "Fire" },
        { ElementType.Ice, "Ice" },
        { ElementType.Water, "Water" },
        { ElementType.Wind, "Wind" },
        { ElementType.Electric, "Electric" }
    };

    public string GetAddressableName(ElementType type)
    {
        return ElementNames.TryGetValue(type, out string name) ? name : null;
    }

    public ElementType Element { get; private set; }

    public void SetElement(ElementType type)
    {
        Element = type;

        string addressName = GetAddressableName(type);
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }

        Addressables.LoadAssetAsync<GameObject>(addressName).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                currentInstance = Instantiate(handle.Result, prefabMountPoint);

                var visual = currentInstance.GetComponent<ElementVisualController>();
                if (visual != null)
                {
                    visual.SetSelected(isCurrentlySelected);
                    if (pendingCooldownDuration > 0f)
                    {
                        visual.StartCooldown(pendingCooldownDuration);
                        pendingCooldownDuration = 0f;
                    }
                }
            }
            else
            {
                Debug.LogError($"[ElementIconUI] 加载失败：{addressName}");
            }
        };
    }

    public void SetSelected(bool isSelected)
    {
        isCurrentlySelected = isSelected; 

        //if (currentInstance == null) return;

        //var visual = currentInstance.GetComponent<ElementVisualController>();
        //if (visual != null)
        //{
        //    visual.SetSelected(isSelected);
        //}
    }

    public void StartCooldown(float duration)
    {
        if (currentInstance == null)
        {
            pendingCooldownDuration = duration;
            return;
        }
    }
}

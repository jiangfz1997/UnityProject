using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class ElementIconUI : MonoBehaviour
{
    [SerializeField] private Transform prefabMountPoint;

    private GameObject currentInstance;
    private AsyncOperationHandle<GameObject>? currentHandle;

    private bool isCurrentlySelected = false;

    private float pendingCooldownDuration = 0f;
    private int _loadVersion = 0;

    private readonly Dictionary<ElementType, string> ElementNames = new()
    {
        { ElementType.Fire, "Fire" },
        { ElementType.Ice, "Ice" },
        //{ ElementType.Water, "Water" },
        //{ ElementType.Wind, "Wind" },
        { ElementType.Lightning, "Lightning" }
    };

    public string GetAddressableName(ElementType type)
    {
        return ElementNames.TryGetValue(type, out string name) ? name : null;
    }

    public ElementType Element { get; private set; }

    public void SetElement(ElementType type)
    {
        Element = type;
        _loadVersion++;
        int myVersion = _loadVersion;

        string addressName = GetAddressableName(type);
        foreach (Transform child in prefabMountPoint)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }

        Addressables.LoadAssetAsync<GameObject>(addressName).Completed += handle =>
        {
            if (myVersion != _loadVersion) return;
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
                Debug.LogError($"[ElementIconUI] loading failed：{addressName}");
            }
        };
    }
    //public void SetElement(ElementType type)
    //{
    //    Element = type;

    //    string addressName = GetAddressableName(type);

    //    // ✅ 清理旧对象
    //    if (currentInstance != null)
    //    {
    //        Destroy(currentInstance);
    //        currentInstance = null;
    //    }

    //    // ✅ 如果之前加载过，释放 Addressables 句柄（好习惯）
    //    if (currentHandle.HasValue)
    //    {
    //        Addressables.Release(currentHandle.Value);
    //        currentHandle = null;
    //    }

    //    // ✅ 加载新 prefab
    //    var handle = Addressables.LoadAssetAsync<GameObject>(addressName);
    //    currentHandle = handle;

    //    handle.Completed += result =>
    //    {
    //        if (result.Status == AsyncOperationStatus.Succeeded)
    //        {
    //            // ✅ 实例化新的图标
    //            currentInstance = Instantiate(result.Result, prefabMountPoint);

    //            // ✅ 设置视觉控制器（如选中、冷却）
    //            var visual = currentInstance.GetComponent<ElementVisualController>();
    //            if (visual != null)
    //            {
    //                visual.SetSelected(isCurrentlySelected);
    //                if (pendingCooldownDuration > 0f)
    //                {
    //                    visual.StartCooldown(pendingCooldownDuration);
    //                    pendingCooldownDuration = 0f;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError($"[ElementIconUI] loading failed：{addressName}");
    //        }
    //    };
    //}

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

using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private List<CollectibleItem> scriptItems = new List<CollectibleItem>();
    [SerializeField] private List<CollectibleItem> effectItems = new List<CollectibleItem>();

    public static CollectionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadCollectionState();
    }

    public void CollectScript(int scriptId)
    {
        if (scriptId >= 0 && scriptId < scriptItems.Count)
        {
            scriptItems[scriptId].SetCollected(true);
            
            if (scriptId < effectItems.Count)
            {
                effectItems[scriptId].SetCollected(true);
            }
            
            SaveCollectionState();
        }
    }

    public void ActivateEffect(int effectId)
    {
        if (effectId >= 0 && effectId < effectItems.Count)
        {
            effectItems[effectId].SetCollected(true);
            SaveCollectionState();
        }
    }

    public bool IsScriptCollected(int scriptId)
    {
        if (scriptId >= 0 && scriptId < scriptItems.Count) {
            // Debug.Log($"Script ID: {scriptId}, Collected: {scriptItems[scriptId].IsCollected}");
            return scriptItems[scriptId].IsCollected;
        }
            
        return false;
    }

    public bool IsEffectActivated(int effectId)
    {
        if (effectId >= 0 && effectId < effectItems.Count)
            return effectItems[effectId].IsCollected;
        return false;
    }

    private void SaveCollectionState()
    {
        for (int i = 0; i < scriptItems.Count; i++)
        {
            PlayerPrefs.SetInt("script_" + i, scriptItems[i].IsCollected ? 1 : 0);
        }

        for (int i = 0; i < effectItems.Count; i++)
        {
            PlayerPrefs.SetInt("effect_" + i, effectItems[i].IsCollected ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private void LoadCollectionState()
    {
        for (int i = 0; i < scriptItems.Count; i++)
        {
            bool isCollected = PlayerPrefs.GetInt("script_" + i, 0) == 1;
            scriptItems[i].SetCollected(isCollected);
            // Debug.Log($"Load: Script ID: {i}, Collected: {isCollected}");
            if (isCollected && i < effectItems.Count)
            {
                effectItems[i].SetCollected(true);
            }
        }

        // ��������Ч��״̬
        // for (int i = 0; i < effectItems.Count; i++)
        // {
        //     bool isCollected = PlayerPrefs.GetInt("effect_" + i, 0) == 1;
        //     effectItems[i].SetCollected(isCollected);
        // }
    }

    public void ResetCollection()
    {
        foreach (var item in scriptItems)
        {
            item.SetCollected(false);
        }
        
        foreach (var item in effectItems)
        {
            item.SetCollected(false);
        }
        
        SaveCollectionState();
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.AddressableAssets;

public static class NecklaceFactory
{
    private static Dictionary<int, NecklaceSO> necklaceDict;
    private static bool isInitialized = false;

    public static void InitAsync(System.Action onComplete = null)
    {
        if (isInitialized)
        {
            onComplete?.Invoke();
            return;
        }

        necklaceDict = new Dictionary<int, NecklaceSO>();

        Addressables.LoadAssetsAsync<NecklaceSO>("necklace", so =>
        {
            necklaceDict[so.id] = so;
        }).Completed += handle =>
        {
            isInitialized = true;
            onComplete?.Invoke();
        };
    }

    public static void GetNecklaceSOById(int id, System.Action<NecklaceSO> callback)
    {
        InitAsync(() =>
        {
            if (necklaceDict.TryGetValue(id, out var so))
            {
                callback?.Invoke(so);
            }
            else
            {
                callback?.Invoke(null);
            }
        });
    }
}

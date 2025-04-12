using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]

public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool, bool> LoadRequestEvent;


    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen, bool makePlayerTeleport=true) 
    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen, makePlayerTeleport);
    }
}

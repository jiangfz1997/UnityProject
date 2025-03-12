using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]

public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;


    ///<summary> 
    /// scene load request event.
    /// </summary>
    /// <param name="locationToLoad">The location to load.</param>
    /// <param name="posToGo">The position to go.</param>
    /// <param name="fadeScreen">if set to <c>true</c> [fade screen].</param>
    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen) 
    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}

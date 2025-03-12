using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/Void Event SO")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
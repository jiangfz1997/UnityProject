using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName ="Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;

    public void Raise(Character character)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(character);
        }
    }
}

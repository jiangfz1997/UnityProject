using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("Event")]
    public CharacterEventSO healthEvent;
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.currentHP / character.maxHP;
        playerStateBar.OnHealthChange(percentage);
    }

}

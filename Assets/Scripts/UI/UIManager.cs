using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    public GoldUI goldUI;

    [Header("Event")]
    public PlayerData playerData;
    private void OnEnable()
    {
        playerData.OnGoldChanged += OnGoldEvent;
        playerData.OnHPChanged += OnHealthEvent;
    }

    private void OnDisable()
    {
        playerData.OnGoldChanged -= OnGoldEvent;
        playerData.OnHPChanged -= OnHealthEvent;
    }
    
    private void OnHealthEvent(float percentage)
    {
        playerStateBar.OnHealthChange(percentage);
    }

    private void OnGoldEvent(int newGoldAmount)
    {
        goldUI.UpdateGoldUI(newGoldAmount);
    }

}

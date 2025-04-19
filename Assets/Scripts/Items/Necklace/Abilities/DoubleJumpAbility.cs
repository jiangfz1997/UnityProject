using System;
using UnityEngine;

public class DoubleJumpAbility : INecklaceAbility, IIdentifiableAbility
{
    private float doubleJumpDuration = -9999f; // -9999 means infinite duration
    private string abilityUID;


    public DoubleJumpAbility(float duration=-9999f, string abilityUID=null)
    {
        doubleJumpDuration = duration;
        this.abilityUID = abilityUID ?? Guid.NewGuid().ToString();
    }

    public void Apply(Player player)
    {
        Debug.Log($"Applying Double Jump Buff for {doubleJumpDuration} seconds");
        Player.Instance.ApplyBuff(BuffType.DoubleJump, -9999, 1, abilityUID); // -9999 means infinite duration
    }

    public void Remove(Player player)
    {
        Debug.Log($"Removing Double Jump Buff");
        Player.Instance.RemoveBuffBySource(abilityUID);
    }
    public void SetUID(string uid)
    {
        abilityUID = uid;
    }


}


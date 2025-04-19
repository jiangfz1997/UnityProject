using System;
using UnityEngine;

public class DefenceUpAbility : INecklaceAbility, IIdentifiableAbility
{
    private float defenceUpPercent;
    private float defenceUpDuration;
    private string abilityUID;


    public DefenceUpAbility(float percent, float duration=-9999f, string abilityUID=null)
    {
        defenceUpPercent = percent;
        defenceUpDuration = duration;
        this.abilityUID = abilityUID ?? Guid.NewGuid().ToString();
    }

    public void Apply(Player player)
    {
        Debug.Log($"Applying Defence Up Buff: {defenceUpPercent}% for {defenceUpDuration} seconds");
        Player.Instance.ApplyBuff(BuffType.SpeedUp, defenceUpDuration, defenceUpPercent, abilityUID); // -9999 means infinite duration
    }

    public void Remove(Player player)
    {
        Debug.Log($"Removing Defence Up Buff: {defenceUpPercent}% for {defenceUpDuration} seconds");
        Player.Instance.RemoveBuffBySource(abilityUID);
    }
    public void SetUID(string uid)
    {
        abilityUID = uid;
    }


}


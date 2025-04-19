using System;
using UnityEngine;

public class AttackUpAbility : INecklaceAbility, IIdentifiableAbility
{
    private float attackUpPercent;
    private float attackUpDuration;
    private string abilityUID;

    public AttackUpAbility(float percent, float duration = -9999f, string uid = null)
    {
        attackUpPercent = percent;
        attackUpDuration = duration;
        abilityUID = uid ?? Guid.NewGuid().ToString(); 
    }

    public void Apply(Player player)
    {
        Debug.Log($"Applying Attack Up Buff: {attackUpPercent}% for {attackUpDuration} seconds");
        Player.Instance.ApplyBuff(BuffType.AttackUp, attackUpDuration, attackUpPercent, abilityUID);
    }
    public void Remove(Player player)
    {
        Debug.Log($"Removing Attack Up Buff: {attackUpPercent}% for {attackUpDuration} seconds");
        Player.Instance.RemoveBuffBySource(abilityUID);
    }
    public void SetUID(string uid)
    {
        abilityUID = uid;
    }

}
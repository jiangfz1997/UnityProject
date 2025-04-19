using System;
using UnityEngine;

public class SlowEnemyAbility : INecklaceAbility, IIdentifiableAbility
{
    private float slowPercent;
    private float slowDuration;
    private string abilityUID;

    public SlowEnemyAbility(float percent, float duration=-9999f, string uid=null)
    {
        slowPercent = percent;
        slowDuration = duration;
        abilityUID = uid ?? Guid.NewGuid().ToString();
    }

    public void Apply(Player player)
    {
        Debug.Log($"Applying Slow Enemy Buff: {slowPercent}% for {slowDuration} seconds");
        player.ApplyBuff(BuffType.SlowEnemy, -9999, -1, abilityUID); // only for UI, ability is done in here
        player.OnHitEnemy += ApplySlowEffect;
    }

    public void Remove(Player player)
    {
        Debug.Log($"Removing Slow Enemy Buff: {slowPercent}% for {slowDuration} seconds");
        player.RemoveBuffBySource(abilityUID);
        player.OnHitEnemy -= ApplySlowEffect;
        
    }

    public void SetUID(string uid)
    {
        abilityUID = uid;
    }

    private void ApplySlowEffect(Enemy enemy)
    {
        enemy.ApplySlow(slowPercent, slowDuration);
    }
}


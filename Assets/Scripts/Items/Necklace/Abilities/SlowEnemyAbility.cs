using UnityEngine;

public class SlowEnemyAbility : INecklaceAbility
{
    private float slowPercent;
    private float slowDuration;

    public SlowEnemyAbility(float percent, float duration)
    {
        slowPercent = percent;
        slowDuration = duration;
    }

    public void Apply(Player player)
    {
        player.OnHitEnemy += ApplySlowEffect;
    }

    public void Remove(Player player)
    {
        player.OnHitEnemy -= ApplySlowEffect;
    }

    private void ApplySlowEffect(Enemy enemy)
    {
        enemy.ApplySlow(slowPercent, slowDuration);
    }
}


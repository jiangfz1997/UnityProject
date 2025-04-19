public enum EnemyState
{
    Patrol, Chase, Idle, Attack
}

public enum TrapType
{
    Harmful,
    NonHarmful
}


public enum DamageType
{
    Physical,
    Lava,
    Poison,
    Fire,
    Ice,
    Lightning,
    Superconduct,
}

public enum BuffType
{
    AttackUp,
    DefenseUp,
    AttackSpeedUp,
    SpeedUp,
    DoubleJump,
    SlowEnemy,
}

public enum StatusEffect
{
    // Debuff && injury type
    Burning,
    Frozen,
    Lightning,
    Paralyze,
    Superconduct,
}
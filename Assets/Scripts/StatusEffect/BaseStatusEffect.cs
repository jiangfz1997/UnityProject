public abstract class BaseStatusEffect
{
    protected float duration;
    protected float elapsedTime;
    protected Character target;
    public StatusEffect EffectType { get; protected set; }
    public BaseStatusEffect(Character target, float duration)
    {
        this.target = target;
        this.duration = duration;
        elapsedTime = 0f;
    }

    public virtual void Apply() { }
    public abstract void Update(float deltaTime);
    public virtual void End() { }

    public virtual void Refresh(BaseStatusEffect newEffect)
    {
        this.duration = newEffect.duration;
        this.elapsedTime = 0f;
    }

    public bool IsFinished => elapsedTime >= duration;
}

using UnityEngine;

public interface INecklaceAbility
{
    void Apply(Player player);
    void Remove(Player player);
}

public interface IIdentifiableAbility
{
    void SetUID(string uid);
}
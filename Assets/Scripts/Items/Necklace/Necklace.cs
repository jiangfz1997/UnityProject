using System;
using System.Collections.Generic;
using UnityEngine;

public class Necklace
{
    private List<INecklaceAbility> abilities = new List<INecklaceAbility>();
    private int id;
    private string abilityUID;
    public Necklace(int id, string uid=null)
    {
        this.id = id;
        this.abilityUID = uid ?? Guid.NewGuid().ToString();
    }

    public int Id => id;
    public void AddAbility(INecklaceAbility ability)
    {
        if (ability is IIdentifiableAbility identifiable)
        {
            identifiable.SetUID(abilityUID);
        }
        abilities.Add(ability);
    }

    public void Activate(Player player)
    {
        foreach (var ability in abilities)
        {
            ability.Apply(player);
        }
    }

    public void Deactivate(Player player)
    {
        foreach (var ability in abilities)
        {
            ability.Remove(player);
        }
    }

    public void SetUID(string uid)
    {
        abilityUID = uid;
        foreach (var ability in abilities)
        {
            if (ability is IIdentifiableAbility identifiable)
            {
                identifiable.SetUID(abilityUID);
            }
        }
    }

    public string GetUID()
    {
        return abilityUID;
    }
}

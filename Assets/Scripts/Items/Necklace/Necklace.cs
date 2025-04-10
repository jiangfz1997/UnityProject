using System.Collections.Generic;
using UnityEngine;

public class Necklace
{
    private List<INecklaceAbility> abilities = new List<INecklaceAbility>();
    private int id;
    public Necklace(int id)
    {
        this.id = id;
    }

    public int Id => id;
    public void AddAbility(INecklaceAbility ability)
    {
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
}

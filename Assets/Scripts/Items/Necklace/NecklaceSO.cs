using UnityEngine;

using System.Collections.Generic;

public enum AbilityType
{
    None,
    SpeedUp,
    AttackBoost,
    SlowEnemy,
    AttackUp,
    DoubleJump,
    DefenseUp,
}

[CreateAssetMenu(menuName = "Item/Necklace")]
public class NecklaceSO : ScriptableObject
{
    [Header("������Ϣ")]
    public int id;
    public string displayName;
    public Sprite icon;

    [Header("��������")]
    public List<AbilityType> abilityTypeList;


    public Necklace CreateRuntimeInstance(string UID)
    {
        var necklace = new Necklace(id, UID);

        foreach (AbilityType abilityType in abilityTypeList) 
        
        {
            var ability = NecklaceAbilityFactory.CreateAbility(abilityType, UID);
            if (ability != null)
            {
                necklace.AddAbility(ability);
            }
        }
        return necklace;
    }
}


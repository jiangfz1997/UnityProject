using UnityEngine;

using System.Collections.Generic;

public enum AbilityType
{
    None,
    DoubleJump,
    AttackBoost,
    SlowEnemy
}

[CreateAssetMenu(menuName = "Item/Necklace")]
public class NecklaceSO : ScriptableObject
{
    [Header("基础信息")]
    public int id;
    public string displayName;
    public Sprite icon;

    [Header("项链能力")]
    public List<AbilityType> abilityTypeList;


    public Necklace CreateRuntimeInstance()
    {
        var necklace = new Necklace(id);

        // 这里根据 ScriptableObject 中的 abilityType 创建对应能力
        foreach (AbilityType abilityType in abilityTypeList) 
        
        {
            var ability = NecklaceAbilityFactory.CreateAbility(abilityType);
            if (ability != null)
            {
                necklace.AddAbility(ability);
            }
        }
        return necklace;
    }
}


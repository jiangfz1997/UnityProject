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
    [Header("������Ϣ")]
    public int id;
    public string displayName;
    public Sprite icon;

    [Header("��������")]
    public List<AbilityType> abilityTypeList;


    public Necklace CreateRuntimeInstance()
    {
        var necklace = new Necklace(id);

        // ������� ScriptableObject �е� abilityType ������Ӧ����
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


using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerSaveData : MonoBehaviour
{
    public int gold;
    public float currentHP;
    public float maxHP;
    public Vector2 position;

    public Dictionary<ElementType, int> elementPoints = new Dictionary<ElementType, int>();
    public List<ElementType> availableElements = new List<ElementType>();

    public Dictionary<BuffType, float> activeBuffs = new Dictionary<BuffType, float>();
    public DamageType currentDamageType;



}

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    public int gold;
    public float maxHP;
    public float currentHP;
    //public int experience;
    //public int level;

    public UnityAction<float> OnHPChanged;
    public UnityAction<int> OnGoldChanged;
    public void Initialize()
    {
        Debug.Log("PlayerData Initialize, Updating UI...");
        OnGoldChanged?.Invoke(gold);
        OnHPChanged?.Invoke(currentHP / maxHP);
    }

    public void NotifyUI()
    {
        OnGoldChanged?.Invoke(gold);
        OnHPChanged?.Invoke(currentHP / maxHP);
    }
    public void ResetData()
    {
        gold = 10000;
        maxHP = 100;
        currentHP = maxHP;
        //experience = 0;
        //level = 1;
    }
    private void OnEnable()
    {
        OnGoldChanged?.Invoke(gold);
        OnHPChanged?.Invoke(currentHP / maxHP);
    }

    public float GetHP() => currentHP;
    public int GetGold() => gold;

    public float GetMaxHP() => maxHP;
    public void SetHP(float newHP)
    {
        currentHP = Mathf.Clamp(newHP, 0, maxHP);
        OnHPChanged?.Invoke(currentHP / maxHP); 
    }

    public void SetGold(int newGold)
    {
        gold = Mathf.Max(newGold, 0);
        OnGoldChanged?.Invoke(gold); 
    }


}

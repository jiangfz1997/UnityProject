using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerData playerData;
    public GoldGenerator goldGenerator;

    //public static event Action<int> OnGoldChanged;


    private void Start()
    {
        playerData.ResetData();
        goldGenerator = gameObject.AddComponent<GoldGenerator>();
        //OnGoldChanged?.Invoke(playerData.gold);
        playerData.NotifyUI();
        //LoadData();
    }
    public void ReduceHealth(float damage)
    {
        // TODO: HealthChange event should be triggered here? 
        playerData.SetHP(Math.Max(0, playerData.GetHP()-damage));

    }

    public void Heal(float amount)
    {
        //playerData.currentHP = Mathf.Min(playerData.currentHP + amount, playerData.maxHP);
        playerData.SetHP(Mathf.Max(playerData.GetHP() + amount, playerData.GetMaxHP()));
    }
    public void AddGold(int amount)
    {
        playerData.SetGold(playerData.GetGold() + amount);
        Debug.Log("Gain coin£º" + amount + "£¬current coin£º" + playerData.gold);
        SaveData();
        //OnGoldChanged?.Invoke(playerData.gold);
    }

    public bool SpendGold(int amount)
    {
        if (playerData.GetGold() < amount)
        {
            Debug.Log("Not enough coin");
            return false;
        }
        playerData.SetGold(playerData.GetGold() - amount);
        
        Debug.Log("Spend coin£º" + amount + "£¬current coin£º" + playerData.gold);
        SaveData();
        //OnGoldChanged?.Invoke(playerData.gold);
        return true;
    }

    public void DieLoseGold() 
    {
        playerData.SetGold(0);
    }

    public float GetCurrentHealth()
    {
        return playerData.currentHP;
    }

    public float GetMaxHealth()
    {
        return playerData.maxHP;
    }

    public int GetGold()
    {
        return playerData.gold;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Gold", playerData.gold);
        PlayerPrefs.SetFloat("HP", playerData.currentHP);
        PlayerPrefs.SetFloat("MaxHP", playerData.maxHP);
        //PlayerPrefs.SetInt("Level", playerData.level);
        //PlayerPrefs.SetInt("XP", playerData.experience);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        playerData.gold = PlayerPrefs.GetInt("Gold", playerData.gold);
        playerData.currentHP = PlayerPrefs.GetFloat("HP", playerData.currentHP);
        playerData.maxHP = PlayerPrefs.GetFloat("MaxHP", playerData.maxHP);
        //playerData.level = PlayerPrefs.GetInt("Level", playerData.level);
        //playerData.experience = PlayerPrefs.GetInt("XP", playerData.experience);
    }
}

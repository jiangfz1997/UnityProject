using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
public class PlayerStats : MonoBehaviour
{
    public PlayerData playerData;
    public GoldGenerator goldGenerator;


    public List<ItemData> inventoryItems = new List<ItemData>();
    private void Start()
    {
        playerData.ResetData();
        goldGenerator = gameObject.AddComponent<GoldGenerator>();
        playerData.NotifyUI();
        //LoadData();
    }
    //public void SyncInventoryItems(List<ItemData> inventory)
    //{
    //    inventoryItems = inventory;
    //}

    public void Restart() {
        playerData.ResetData();
        playerData.NotifyUI();
    }

    public void ReduceHealth(float damage)
    {
        playerData.SetHP(Math.Max(0, playerData.GetHP()-damage));

    }

    public void Heal(float amount)
    {
        playerData.SetHP(Mathf.Max(playerData.GetHP() + amount, playerData.GetMaxHP()));
        Player.Instance.ShowRestoreEffect();
    }
    public void AddGold(int amount)
    {
        playerData.SetGold(playerData.GetGold() + amount);
        Debug.Log("Gain coin£º" + amount + "£¬current coin£º" + playerData.gold);
        SaveData();
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

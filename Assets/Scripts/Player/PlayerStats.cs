using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerStatsSaveData
{
    //public int gold;
    public float currentHP;
    public float maxHP;
    public Vector3 position;
    public string currentScene;
    public int currentLevelIndex;
}

public class PlayerStats : MonoBehaviour, ISaveable
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
        Debug.Log("Gain coin��" + amount + "��current coin��" + playerData.gold);
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
        
        Debug.Log("Spend coin��" + amount + "��current coin��" + playerData.gold);
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
        float maxHP = playerData.UpdateMaxHP();
        return maxHP;
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

    public string SaveKey() => "PlayerStats";

    public object CaptureState()
    {
        string test = GetCurrentLevelSceneName();
        return new PlayerStatsSaveData
        {
            //gold = playerData.gold,
            currentHP = playerData.currentHP,
            maxHP = playerData.maxHP,
            position = transform.position,
            currentScene = GetCurrentLevelSceneName(),
            currentLevelIndex = LevelManager.Instance.GetCurrentLevelIndex()


        };
    }
    private string GetCurrentLevelSceneName()
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name != "Persistent")
            {
                return scene.name;
            }
        }

        return "Unknown";
    }
    public void RestoreState(object state)
    {
        var data = state as PlayerStatsSaveData;
        if (data == null)
        {
            Debug.LogWarning("PlayerStats recovery failed");
            return;
        }

        //playerData.gold = data.gold;
        playerData.currentHP = data.currentHP;
        playerData.maxHP = data.maxHP;
        playerData.NotifyUI();
        Vector3 offset = new Vector3(-3f, 2f, 0f); 
        transform.position = data.position + offset;
        Debug.Log($"PlayerStats recovery success, position: {transform.position}");
        LevelManager.Instance.SetCurrentLevelIndex(data.currentLevelIndex);

    }
}

using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    //public int goldAmount = 2000; 
    public TextMeshProUGUI moneyText; 
    public Alert alert;
    private Player player;

    void Start()
    {
        player = Object.FindFirstObjectByType<Player>();
        
        if (player != null)
        {
         
            UpdateMoneyText(player.GetGold());
        }
        else
        {
            Debug.LogError("❌ Money: 找不到 Player 对象！");
        }
    }

    public void UpdateMoneyText(int amount)
    {
        moneyText.text = "Your Money: $" + amount.ToString();
    }
    
    public bool EnoughGold(int amount)
    {
        if (player.GetGold() >= amount)
        {
            return true;
        }
        else
        {
            alert.ShowAlert("Not enough gold to complete the transaction!");
            return false;
        }
    }

    public bool SpendGold(int amount)
    {
        if (player.GetGold() >= amount) 
        {
            player.SpendGold(amount);
            UpdateMoneyText(player.GetGold());
            return true;


        }
        else
        {
         
            alert.ShowAlert("Not enough gold to complete the transaction!");
            return false;
        }
    }


    public void AddGold(int amount)
    {
        player.CollectGold(amount);
        UpdateMoneyText(player.GetGold()); 
    }

    
    //public int GetGold()
    //{
    //    return goldAmount;
    //}
}
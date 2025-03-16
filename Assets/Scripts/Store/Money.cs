using UnityEngine;
using TMPro; // 引入TextMeshPro命名空间

public class Money : MonoBehaviour
{
    public int goldAmount = 2000; // 默认金币数量为2000
    public TextMeshProUGUI moneyText; // 用于显示金币数量的TextMeshProUGUI

    void Start()
    {
        UpdateMoneyText(); // 启动时更新显示
    }

    // 更新显示金币数量
    void UpdateMoneyText()
    {
        moneyText.text = "Your Money: $" + goldAmount.ToString();
    }

    // 减少金币数量
    public bool SpendGold(int amount)
    {
        if (goldAmount >= amount)
        {
            goldAmount -= amount;
            UpdateMoneyText(); // 消耗金币后更新显示
            return true; // 足够金币，成功减少
        }
        else
        {
            return false; // 金币不足
        }
    }

    // 增加金币数量
    public void AddGold(int amount)
    {
        goldAmount += amount;
        UpdateMoneyText(); // 增加金币后更新显示
    }

    // 获取当前金币数量
    public int GetGold()
    {
        return goldAmount;
    }
}

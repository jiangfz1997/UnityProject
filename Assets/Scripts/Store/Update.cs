using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Update : MonoBehaviour
{
    public int refreshCost = 100; // 初始刷新消耗金币
    public Money userMoney; // 用户金币管理脚本的引用
    public Button refreshButton;
    public TextMeshProUGUI refreshButtonText;
    public ProductGridManager productGridManager; // 关联商店管理器

    void Start()
    {
        refreshButton.onClick.AddListener(OnRefreshButtonClick);
        UpdateRefreshButtonText();
    }

    void OnRefreshButtonClick()
    {
        if (userMoney.SpendGold(refreshCost)) // 检查金币是否足够
        {
            Debug.Log("商店刷新成功!");
            productGridManager.RefreshProducts(); // 调用刷新方法

            refreshCost *= 2; // 每次刷新后金币消耗翻倍
            UpdateRefreshButtonText();
        }
        else
        {
            Debug.Log("金币不足，无法刷新商店!");
        }
    }

    void UpdateRefreshButtonText()
    {
        refreshButtonText.text = "NEW ($ " + refreshCost + ")";
    }
}

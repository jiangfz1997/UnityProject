using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Update : MonoBehaviour
{
    public int refreshCost = 100; 
    public Money userMoney; 
    public Button refreshButton;
    public TextMeshProUGUI refreshButtonText;
    public ProductGridManager productGridManager; 
    public Alert alert; 

    void Start()
    {
        refreshButton.onClick.AddListener(OnRefreshButtonClick);
        UpdateRefreshButtonText();
    }

    void OnRefreshButtonClick()
    {
        if (userMoney.SpendGold(refreshCost)) 
        {
            Debug.Log("商店刷新成功!");
            productGridManager.RefreshProducts(); 

            refreshCost *= 2; 
            UpdateRefreshButtonText();
        }
        else
        {
            
            alert.ShowAlert("Not enough gold to refresh the shop!");
        }
    }

    void UpdateRefreshButtonText()
    {
        refreshButtonText.text = "REFRESH ($ " + refreshCost + ")";
    }
}

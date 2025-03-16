using UnityEngine;

using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText; 

    //private void OnEnable()
    //{
    //    PlayerStats.OnGoldChanged += UpdateGoldUI;
    //}

    //private void OnDisable()
    //{
    //    PlayerStats.OnGoldChanged -= UpdateGoldUI; 
    //}

    public void UpdateGoldUI(int newGoldAmount)
    {
        goldText.text = newGoldAmount.ToString();
    }
}


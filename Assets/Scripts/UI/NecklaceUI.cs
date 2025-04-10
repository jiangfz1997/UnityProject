using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NecklaceUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image iconImage;
    //public TMP_Text nameText;

    public void UpdateUI(NecklaceSO necklaceSO)
    {
        if (necklaceSO != null)
        {
            iconImage.sprite = necklaceSO.icon;
            iconImage.color = Color.white; // ��ʾ����ͼ��
        }
        else
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0); // ����Ϊ��ȫ͸��
        }
    }
}

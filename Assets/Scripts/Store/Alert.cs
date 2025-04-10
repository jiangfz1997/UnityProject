using UnityEngine;
using UnityEngine.UI; // 需要添加这个命名空间
using TMPro; // 如果你在使用 TextMeshPro 组件

public class Alert : MonoBehaviour
{
    public GameObject errorPanel; // 错误面板
    public TextMeshProUGUI errorMessageText; // 错误消息文本
    public Button closeButton; // 关闭按钮

    void Start()
    {
        // 确保按钮点击事件绑定
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideAlert);
        }
        else
        {
            Debug.LogError("Close button not assigned!");
        }

        // 一开始隐藏错误面板
        errorPanel.SetActive(false); // 确保面板开始时不可见
    }

    // 显示错误面板并更新消息
    public void ShowAlert(string message)
    {
        errorPanel.SetActive(true); // 显示面板
        errorMessageText.text = message; // 设置错误消息文本
    }

    // 隐藏错误面板
    public void HideAlert()
    {
        errorPanel.SetActive(false); // 隐藏面板
    }
}

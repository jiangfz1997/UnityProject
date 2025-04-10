using UnityEngine;
using TMPro;
using System.Collections;

public class BuffSlot : MonoBehaviour
{
    //public TextMeshProUGUI buffText;
    private BuffType buffType;
    private float duration;

    public void Initialize(BuffType type, float duration)
    {
        this.buffType = type;
        this.duration = duration;
        UpdateUI();
        StartCoroutine(StartCountdown());
    }

    public void UpdateDuration(float newDuration)
    {
        this.duration = Mathf.Max(duration, newDuration); // **刷新持续时间**
        UpdateUI();
    }

    private void UpdateUI()
    {
        //buffText.text = duration.ToString("F1"); // **显示 Buff 剩余时间**
    }

    private IEnumerator StartCountdown()
    {
        while (duration > 0)
        {
            yield return new WaitForSeconds(1f);
            duration -= 1f;
            UpdateUI();
        }
        BuffManager.instance.RemoveBuff(buffType); // **时间到自动删除**
    }
}

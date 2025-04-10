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
        this.duration = Mathf.Max(duration, newDuration); // **ˢ�³���ʱ��**
        UpdateUI();
    }

    private void UpdateUI()
    {
        //buffText.text = duration.ToString("F1"); // **��ʾ Buff ʣ��ʱ��**
    }

    private IEnumerator StartCountdown()
    {
        while (duration > 0)
        {
            yield return new WaitForSeconds(1f);
            duration -= 1f;
            UpdateUI();
        }
        BuffManager.instance.RemoveBuff(buffType); // **ʱ�䵽�Զ�ɾ��**
    }
}

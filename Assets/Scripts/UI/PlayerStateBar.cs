using UnityEngine;
using UnityEngine.UI;
public class PlayerStateBar : MonoBehaviour
{
    public Image healthBarGreen;
    public Image healthBarRed;

    public void OnHealthChange(float percentage)
    {
        healthBarGreen.fillAmount = percentage;
    }

    private void Update()
    {
        if (healthBarRed.fillAmount > healthBarGreen.fillAmount)
        {
            healthBarRed.fillAmount -= Time.deltaTime;
        }
        else
        {
            healthBarRed.fillAmount = healthBarGreen.fillAmount;
        }
    }

}


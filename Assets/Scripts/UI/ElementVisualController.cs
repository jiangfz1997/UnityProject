using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElementVisualController : MonoBehaviour
{
    [Header("缩放设置")]
    [SerializeField] private float selectedScale = 1.2f;
    [SerializeField] private float unselectedScale = 0.9f;
    [SerializeField] private float scaleSpeed = 8f; // 缩放速度（越大越快）
    [SerializeField] private Image cooldownCircle;

    private float targetScale = 1f;

    private void Update()
    {
        // 平滑缩放到目标大小
        float currentScale = transform.localScale.x;
        float newScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleSpeed);
        transform.localScale = new Vector3(newScale, newScale, 1f);
    }

    public void SetSelected(bool isSelected)
    {
        targetScale = isSelected ? selectedScale : unselectedScale;
    }

    public void StartCooldown(float duration)
    {
        StartCoroutine(CooldownRoutine(duration));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        cooldownCircle.fillAmount = 1f;
        cooldownCircle.gameObject.SetActive(true);

        float timer = duration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            cooldownCircle.fillAmount = timer / duration;
            yield return null;
        }

        cooldownCircle.fillAmount = 0f;
        cooldownCircle.gameObject.SetActive(false);
    }
}

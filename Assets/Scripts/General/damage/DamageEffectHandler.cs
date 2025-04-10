using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DamageEffectHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material material;

    public void Initialize(SpriteRenderer sr)
    {
        spriteRenderer = sr;
        material = spriteRenderer.material;
    }

    public void TriggerEffect()
    {
        Debug.Log("TriggerEffect!!");
        if (spriteRenderer != null && material != null)
        {
            StartCoroutine(HurtEffectCoroutine());
        }
    }

    private IEnumerator HurtEffectCoroutine()
    {
        float totalDuration = 0.5f;   
        float flashDuration = 0.1f; 
        float redDuration = 0.5f;   
        float elapsed = 0f;

        Color originalColor = spriteRenderer.color;
        material.SetFloat("_HurtIntensity", 1); 

        while (elapsed < totalDuration)
        {

            spriteRenderer.color = new Color(1f, 0.2f, 0.2f, 0.5f); 
            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.color = new Color(1f, 0.2f, 0.2f, 1f); 
            yield return new WaitForSeconds(flashDuration);

            elapsed += flashDuration * 2; 


            float redT = Mathf.Clamp01(elapsed / redDuration);
            material.SetFloat("_HurtIntensity", Mathf.Lerp(1, 0, redT));
        }


        material.SetFloat("_HurtIntensity", 0);
        spriteRenderer.color = originalColor;
    }

    public void SetColorEffect(Color effectColor, float duration)
    {
        StartCoroutine(ColorEffectCoroutine(effectColor, duration));
    }

    private IEnumerator ColorEffectCoroutine(Color effectColor, float duration)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = effectColor; // 变色

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor; // 恢复原色
    }
    public void ApplyFreezeEffect(float duration)
    {
        StartCoroutine(FreezeEffectCoroutine(duration));
    }
    private IEnumerator FreezeEffectCoroutine(float duration)
    {
        // ✅ 1. **创建一个 `SpriteRenderer` 作为遮罩**
        GameObject freezeOverlay = new GameObject("FreezeOverlay");
        freezeOverlay.transform.SetParent(transform); // ✅ **跟随目标**
        freezeOverlay.transform.localPosition = Vector3.zero;

        SpriteRenderer overlayRenderer = freezeOverlay.AddComponent<SpriteRenderer>();
        overlayRenderer.sprite = spriteRenderer.sprite; // ✅ **使用原角色的 Sprite**
        overlayRenderer.color = new Color(0.3f, 0.5f, 1f, 0.8f); // ✅ **更明显的蓝色**

        // ✅ 2. **确保 `Sorting Layer` & `Sorting Order`**
        overlayRenderer.sortingLayerName = spriteRenderer.sortingLayerName; // ✅ **匹配角色的 Layer**
        overlayRenderer.sortingOrder = spriteRenderer.sortingOrder + 5; // ✅ **确保在 `角色` 之上**
        float elapsed = 0f;
        while (elapsed < duration)
        {
            overlayRenderer.color = new Color(0.3f, 0.5f, 1f, Mathf.PingPong(elapsed * 3f, 0.6f) + 0.3f);
            freezeOverlay.transform.position = transform.position; // ✅ **同步位置**
            overlayRenderer.flipX = !spriteRenderer.flipX; // ✅ **同步翻转**
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(freezeOverlay);
    }



}

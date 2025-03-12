using System.Collections;
using UnityEngine;

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
        float totalDuration = 2f;   
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

}

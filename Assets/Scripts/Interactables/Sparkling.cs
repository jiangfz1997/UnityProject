using UnityEngine;
using System.Collections;

public class Sparkling : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Color glowColor = Color.grey;
    private Color originalColor;

    public float blinkSpeed = 5f;
    public float blinkIntensity = 1f;


    private void Start()
    {
        originalColor = spriteRenderer.color;
        StartCoroutine(Spark());
    }

    private IEnumerator Spark()
    {
        float t = 0;

        while (true)
        {
            t += Time.deltaTime * blinkSpeed;
            float lerp = (Mathf.Sin(t * Mathf.PI) + 1) * 0.5f * blinkIntensity;

            spriteRenderer.color = Color.Lerp(originalColor, glowColor, lerp);
            
            yield return null;
        }
    }

    public void OnCollected()
    {
        StopAllCoroutines();
        spriteRenderer.color = originalColor;

        this.enabled = false;
    }
}
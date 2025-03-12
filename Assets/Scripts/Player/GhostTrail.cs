using UnityEngine;
using System.Collections;

public class GhostTrail : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color color;

    public void Initialize(SpriteRenderer sr)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sr.sprite;
        transform.position = sr.transform.position;
        transform.localScale = sr.transform.localScale;
        transform.rotation = sr.transform.rotation;

        color = new Color(1f, 1f, 1f, 0.5f);
        spriteRenderer.color = color;

        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float fadeDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            color.a = Mathf.Lerp(0.5f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

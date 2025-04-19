using System.Collections;
using System.Linq;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    private Material[] materials;
    private Color[] originalColors;
    private Coroutine flashCoroutine;

    void Awake()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        materials = renderers.SelectMany(r => r.materials).ToArray();

        originalColors = materials.Select(m => m.HasProperty("_Color") ? m.color : Color.white).ToArray();
    }

    public void Flash(Color flashColor, float duration = 0.1f)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(DoFlash(flashColor, duration));
    }
    private IEnumerator DoFlash(Color flashColor, float duration)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].HasProperty("_Color"))
                materials[i].color = flashColor;
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].HasProperty("_Color"))
                materials[i].color = originalColors[i];
        }
    }
}

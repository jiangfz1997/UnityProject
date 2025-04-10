using UnityEngine;

public class BloodDecal : MonoBehaviour
{
    public float fadeDuration = 10f;
    private SpriteRenderer sr;
    private float elapsed = 0f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= fadeDuration)
        {
            float alpha = Mathf.Lerp(0.5f, 0f, (elapsed - fadeDuration) / fadeDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

            if (alpha <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}

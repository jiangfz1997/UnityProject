using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class PlatformTintController : MonoBehaviour
{
    [Header("Tint ")]
    public Color tintColor = new Color(1.1f, 1.1f, 1.1f, 1f); 


    public bool autoApply = true;

    private Color previousColor;

    private void Update()
    {
        if (!Application.isPlaying && autoApply && previousColor != tintColor)
        {
            ApplyTint();
        }
    }

    [ContextMenu("Apply Tint Now")]
    public void ApplyTint()
    {
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();

        foreach (var tilemap in tilemaps)
        {
            tilemap.color = tintColor;
        }

        previousColor = tintColor;
    }

    [ContextMenu("Reset to White")]
    public void ResetTint()
    {
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();

        foreach (var tilemap in tilemaps)
        {
            tilemap.color = Color.white;
        }

        previousColor = Color.white;
        tintColor = Color.white;
    }
}

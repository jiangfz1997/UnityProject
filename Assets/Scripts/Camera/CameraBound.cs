using UnityEngine;
using System;

public class CameraBounds : MonoBehaviour
{
    public static event Action<Bounds> OnBoundsChanged;

    private void Awake()
    {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        Debug.Log("Bounds: " + bounds);
        OnBoundsChanged?.Invoke(bounds);
    }
}

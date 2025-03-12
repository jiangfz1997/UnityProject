using UnityEngine;

[CreateAssetMenu(fileName = "NewPlatformData", menuName = "Platform/PlatformData")]
public class PlatformData : ScriptableObject
{
    // platform move speed
    public float speed = 2.0f;

    // platform waypoints
    //public Vector3[] waypoints;
}

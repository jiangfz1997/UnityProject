using UnityEngine;

[CreateAssetMenu(fileName = "MonsterLoopSFXConfig", menuName = "Audio/MonsterLoopSFXConfig")]
public class MonsterLoopSFXConfig : ScriptableObject
{
    public AudioClip[] walkClips;
    public float walkMinInterval = 0.5f;
    public float walkMaxInterval = 1.2f;

    public AudioClip[] groanClips;
    public float groanMinInterval = 3f;
    public float groanMaxInterval = 8f;
}

using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSFXConfig", menuName = "Audio/Player SFX Config")]
public class PlayerSFXConfig : ScriptableObject
{
    public AudioClip[] footstepClips;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip[] attackClips;
    public AudioClip[] hurtClips;
    public AudioClip deathClip;
    public AudioClip chargeClip;
    public AudioClip dashClip;
    public AudioClip healClip;
    public AudioClip dreakPotionClip;
}

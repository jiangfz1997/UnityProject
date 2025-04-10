using UnityEngine;

using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "MonsterSoundData", menuName = "Audio/Monster Sound Data")]
public class MonsterSoundData : ScriptableObject
{
    public AssetReferenceT<AudioClip> hurtSound;

    public AssetReferenceT<AudioClip> deathSound;

    public AssetReferenceT<AudioClip> attackSound;

    public AssetReferenceT<AudioClip> movingSound;
}
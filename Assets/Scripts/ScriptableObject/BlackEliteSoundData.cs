using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "BlackEliteSoundData", menuName = "Audio/Black Elite Sound Data")]
public class BlackEliteSoundData : MonsterSoundData
{
    public AssetReferenceT<AudioClip> dashSound;
    public AssetReferenceT<AudioClip> swordSound1;
    public AssetReferenceT<AudioClip> swordSound2;
    public AssetReferenceT<AudioClip> shieldSound;


}

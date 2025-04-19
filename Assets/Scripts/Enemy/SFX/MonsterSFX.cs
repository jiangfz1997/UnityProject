using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MonsterSFX : MonoBehaviour
{
    public MonsterSoundData soundData; 

    protected AudioSource audioSource;
    protected AudioClip hurtClip;
    protected AudioClip deathClip;
    protected AudioClip attackClip;
    protected AudioClip idleClip;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        LoadClips();
    }

    //protected virtual async void LoadClips()
    //{
    //    if (soundData == null)
    //    {
    //        Debug.LogWarning($"MonsterSFX on {gameObject.name} has no soundData.");
    //        return;
    //    }

    //    if (soundData.hurtSound != null && soundData.hurtSound.RuntimeKeyIsValid())
    //    {
    //        Debug.Log($"hurtSound key: {soundData.hurtSound.RuntimeKey}");
    //        var op = soundData.hurtSound.LoadAssetAsync<AudioClip>();
    //        await op.Task;
    //        if (op.Status == AsyncOperationStatus.Succeeded)
    //            hurtClip = op.Result;
    //    }

    //    if (soundData.deathSound != null && soundData.deathSound.RuntimeKeyIsValid())
    //    {
    //        var op = soundData.deathSound.LoadAssetAsync<AudioClip>();
    //        await op.Task;
    //        if (op.Status == AsyncOperationStatus.Succeeded)
    //            deathClip = op.Result;
    //    }

    //    if (soundData.attackSound != null && soundData.attackSound.RuntimeKeyIsValid())
    //    {
    //        var op = soundData.attackSound.LoadAssetAsync<AudioClip>();
    //        await op.Task;
    //        if (op.Status == AsyncOperationStatus.Succeeded)
    //            attackClip = op.Result;
    //    }

    //    if (soundData.movingSound != null && soundData.movingSound.RuntimeKeyIsValid())
    //    {
    //        var op = soundData.movingSound.LoadAssetAsync<AudioClip>();
    //        await op.Task;
    //        if (op.Status == AsyncOperationStatus.Succeeded)
    //            idleClip = op.Result;
    //    }
    //}

    protected virtual async void LoadClips()
    {
        if (soundData == null) return;

        hurtClip = await LoadClip(soundData.hurtSound, "Hurt");
        deathClip = await LoadClip(soundData.deathSound, "Death");
        attackClip = await LoadClip(soundData.attackSound, "Attack");
        idleClip = await LoadClip(soundData.movingSound, "Moving");
    }

    private async Task<AudioClip> LoadClip(AssetReference reference, string tag)
    {
        if (reference == null || !reference.RuntimeKeyIsValid())
        {
            Debug.LogWarning($"{tag} sound reference is null or invalid.");
            return null;
        }

        if (!reference.OperationHandle.IsValid())
        {
            var op = reference.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                return op.Result;

            Debug.LogError($"{tag} sound failed to load: {op.OperationException}");
            return null;
        }
        else
        {
            return reference.Asset as AudioClip;
        }
    }
    public void PlayHurtSound()
    {
        if (hurtClip != null)
            audioSource.PlayOneShot(hurtClip);
    }

    public void PlayDeathSound()
    {
        if (deathClip != null)
            audioSource.PlayOneShot(deathClip);
    }

    public void PlayAttackSound()
    {
        if (attackClip != null)
            audioSource.PlayOneShot(attackClip);
    }

    public void PlayIdleSound()
    {
        if (idleClip != null)
            audioSource.PlayOneShot(idleClip);
    }
    void OnDestroy()
    {
        if (soundData != null)
        {
            if (soundData.hurtSound != null) soundData.hurtSound.ReleaseAsset();
            if (soundData.deathSound != null) soundData.deathSound.ReleaseAsset();
        }
    }
}

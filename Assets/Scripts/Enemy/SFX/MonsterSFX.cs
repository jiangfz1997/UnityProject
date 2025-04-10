using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MonsterSFX : MonoBehaviour
{
    public MonsterSoundData soundData; 

    private AudioSource audioSource;
    private AudioClip hurtClip;
    private AudioClip deathClip;
    private AudioClip attackClip;
    private AudioClip idleClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        LoadClips();
    }

    private async void LoadClips()
    {
        if (soundData == null)
        {
            Debug.LogWarning($"MonsterSFX on {gameObject.name} has no soundData.");
            return;
        }

        if (soundData.hurtSound != null)
        {
            Debug.Log($"hurtSound key: {soundData.hurtSound.RuntimeKey}");
            var op = soundData.hurtSound.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                hurtClip = op.Result;
        }

        if (soundData.deathSound != null)
        {
            var op = soundData.deathSound.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                deathClip = op.Result;
        }

        if (soundData.attackSound != null)
        {
            var op = soundData.attackSound.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                attackClip = op.Result;
        }

        if (soundData.movingSound != null)
        {
            var op = soundData.movingSound.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                idleClip = op.Result;
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

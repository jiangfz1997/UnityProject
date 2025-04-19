using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BlackEliteSFX : MonsterSFX
{
    public BlackEliteSoundData blackEliteSoundData;
    private AudioClip dashClip;
    private AudioClip swordClip1;
    private AudioClip swordClip2;
    private AudioClip shieldClip;

    protected override void Awake()
    {
        soundData = blackEliteSoundData;
        base.Awake();
    }
    protected override async void LoadClips() 
    {
        base.LoadClips();

        if (blackEliteSoundData.dashSound != null)
        {
            var op = blackEliteSoundData.dashSound.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                dashClip = op.Result;
        }

        if ( blackEliteSoundData.swordSound1 != null)
        {
            var op = blackEliteSoundData.swordSound1.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                swordClip1 = op.Result;
        }

        if (blackEliteSoundData.swordSound2 != null)
        {
            var op = blackEliteSoundData.swordSound2.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                swordClip2 = op.Result;
        }

        if (blackEliteSoundData.shieldSound != null)
        {
            var op = blackEliteSoundData.shieldSound.LoadAssetAsync<AudioClip>();
            await op.Task;
            if (op.Status == AsyncOperationStatus.Succeeded)
                shieldClip = op.Result;
        }


    }

    public void PlayDashSound()
    {
        if (dashClip != null)
        {
            audioSource.PlayOneShot(dashClip);
        }
    }

    public void PlaySwordSound1()
    {
        if (swordClip1 != null)
        {
            audioSource.PlayOneShot(swordClip1);
        }
    }

    public void PlaySwordSound2()
    {
        if (swordClip2 != null)
        {
            audioSource.PlayOneShot(swordClip2);
        }
    }

    public void PlayShieldSound()
    {
        if (shieldClip != null)
        {
            audioSource.PlayOneShot(shieldClip);
        }

    }



    }

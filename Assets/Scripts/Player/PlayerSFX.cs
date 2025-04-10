using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSFX : MonoBehaviour
{
    public PlayerSFXConfig config;
    private AudioSource audioSource;
    private float lastHurtSoundTime = -10f;
    public float hurtCooldown = 1f; 

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        if (config.footstepClips.Length == 0) return;
        var clip = config.footstepClips[Random.Range(0, config.footstepClips.Length)];
        audioSource.PlayOneShot(clip, 1.0f);
    }
    public void PlayAttackSound()
    {
        if (config == null || config.attackClips == null || config.attackClips.Length == 0) return;

        var clip = config.attackClips[Random.Range(0, config.attackClips.Length)];
        if (clip != null)
            audioSource.PlayOneShot(clip, 1.0f);
    }


    public void TestSound()
    {
        Debug.Log("TestSound triggered by animation event.");
    }

    public void PlayJump() => PlayClip(config.jumpClip);

    public void PlayHurtSound()
    {
        if (Time.time - lastHurtSoundTime < hurtCooldown) return;
        lastHurtSoundTime = Time.time;
        if (config.hurtClips.Length == 0) return;
        var clip = config.hurtClips[Random.Range(0, config.hurtClips.Length)];
        if (clip != null)
            audioSource.PlayOneShot(clip, 1.0f);
    }
    public void PlayDeath() => PlayClip(config.deathClip);

    public void PlayLanded() => PlayClip(config.landClip);

    public void PlayChargeSound() => PlayClip(config.chargeClip);

    public void PlayDashSound() => PlayClip(config.dashClip);
    private void PlayClip(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, 1.0f);
    }
}

using UnityEngine;
using System.Collections;
public class MonsterLoopSFXPlayer : MonoBehaviour
{
    public MonsterLoopSFXConfig config;

    private AudioSource audioSource;

    private Coroutine walkLoop;
    private Coroutine groanLoop;
    private Coroutine breathLoop;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.playOnAwake = false;
    }

    public void StartAllLoopSounds()
    {
        if (config == null) return;
        if (config.walkClips.Length > 0)
            walkLoop = StartCoroutine(LoopSound(config.walkClips, config.walkMinInterval, config.walkMaxInterval));

        if (config.groanClips.Length > 0)
            groanLoop = StartCoroutine(LoopSound(config.groanClips, config.groanMinInterval, config.groanMaxInterval));

    }

    public void StopAllLoopSounds()
    {
        if (walkLoop != null) StopCoroutine(walkLoop);
        if (groanLoop != null) StopCoroutine(groanLoop);
        if (breathLoop != null) StopCoroutine(breathLoop);
    }

    private IEnumerator LoopSound(AudioClip[] clips, float min, float max)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(min, max));

            if (clips.Length > 0)
            {
                var clip = clips[Random.Range(0, clips.Length)];
                audioSource.PlayOneShot(clip, Random.Range(1f, 2f));
            }
        }
    }
}

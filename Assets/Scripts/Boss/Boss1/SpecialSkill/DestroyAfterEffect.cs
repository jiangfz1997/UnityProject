using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }
    
    void Update()
    {
        bool allStopped = true;
        
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.isPlaying)
            {
                allStopped = false;
                break;
            }
        }
        
        if (allStopped)
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class Trap : MonoBehaviour
{
    public TrapType type;
    public bool isMovable;
    public AudioClip trapSFX;
    protected AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
    public virtual void PlaySound() 
    {
        if (trapSFX != null)
        {
            audioSource.PlayOneShot(trapSFX);
        }
    }
}

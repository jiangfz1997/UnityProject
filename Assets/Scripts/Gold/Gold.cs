using UnityEngine;

public class Gold : MonoBehaviour
{
    private int goldAmount;
    private bool isCollected;
    public AudioClip dropSFX;
    public AudioClip collectSFX;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;

    public void Initialize(int amount=10)
    {
        goldAmount = amount;
        isCollected = false;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.CollectGold(goldAmount);
            isCollected = true;
            spriteRenderer.enabled = false;
            collider2D.enabled = false;
            audioSource.PlayOneShot(collectSFX);
            if (audioSource != null && collectSFX != null)
            {
                Debug.Log("Playing collect sound");
                audioSource.PlayOneShot(collectSFX);
            }
            Destroy(gameObject, collectSFX.length);
        }
        else if (collision.CompareTag("Ground"))
        {
            if (audioSource != null && dropSFX != null)
            {
                audioSource.PlayOneShot(dropSFX);
                dropSFX = null; // Prevents the sound from playing again
            }
        }
    }
}

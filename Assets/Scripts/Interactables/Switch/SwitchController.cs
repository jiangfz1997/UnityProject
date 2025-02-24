using UnityEngine;

public class SwitchController : MonoBehaviour, IInteractable
{
    public GameObject eKeyPrompt;
    public GameObject gate;
    //private bool isPlayerNearby = false;
    //private bool isGateOpen = false;
    private bool isActivated = false;
    private SpriteRenderer spriteRenderer;
    public void Interact()
    {
        if (isActivated) 
        {
            return;
        }
        isActivated = !isActivated; // **ÇÐ»»×´Ì¬**

        spriteRenderer.flipX = isActivated;
        eKeyPrompt.SetActive(false);

        // **´¥·¢ Gate ÊÂ¼þ**
        if (gate != null)
        {
            gate.GetComponent<GateController>().OpenDoor();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (eKeyPrompt)
        {
            eKeyPrompt.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            eKeyPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            eKeyPrompt.SetActive(false);
        }
    }
}

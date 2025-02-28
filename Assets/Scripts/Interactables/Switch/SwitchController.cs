using UnityEngine;

public class SwitchController : MonoBehaviour, IInteractable
{
    public GameObject eKeyPrompt;
    public GameObject gate;
    //private bool isPlayerNearby = false;
    //private bool isGateOpen = false;
    [SerializeField] private bool isActivated = false;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite avatarImage;

    public void Interact()
    {
        if (isActivated)
        {
            return;
        }
        isActivated = !isActivated; // **ÇÐ»»×´Ì¬**

        spriteRenderer.flipX = isActivated;
        eKeyPrompt.SetActive(false);

        DialogueTrigger();

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

    void DialogueTrigger()
    {
        DialogueManager.Instance.StartDialogue(new DialogueManager.DialogueLine[] {
            new DialogueManager.DialogueLine
            {
                speakerName = "You",
                dialogueContent = "Sounds like a heavy door has opened somewhere.",
                avatarSprite = avatarImage
            }
        });
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

using UnityEngine;

public class SwitchController : MonoBehaviour, IInteractable
{
    //public GameObject eKeyPrompt;
    //public GameObject gate;
    //private bool isPlayerNearby = false;
    //private bool isGateOpen = false;
    [SerializeField] private bool isActivated = false;
    private SpriteRenderer spriteRenderer;
    public GameObject[] objectsToActivate;
    [SerializeField] private Sprite avatarImage;

    public void Interact()
    {
        if (isActivated)
        {
            return;
        }
        isActivated = !isActivated; // **切换状态**

        spriteRenderer.flipX = isActivated;
        //eKeyPrompt.SetActive(false);

        DialogueTrigger();

        // **触发 Gate 事件**
        //if (gate != null)
        //{
        //    gate.GetComponent<GateController>().OpenDoor();
        //}
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                IActivatable activatable = obj.GetComponent<IActivatable>();
                if (activatable != null)
                {
                    activatable.Activate(); // 触发不同物体的行为
                }
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //if (eKeyPrompt)
        //{
        //    eKeyPrompt.SetActive(false);
        //}
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
        //if (other.CompareTag("Player") && !isActivated)
        //{
        //    eKeyPrompt.SetActive(true);
        //}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    eKeyPrompt.SetActive(false);
        //}
    }
}

using UnityEngine;

public class AfterPhantomBattle: MonoBehaviour
{
    private PhantomFSM phantomFSM;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private Sprite avatarImage;
    [SerializeField] private GameObject Script;
    private bool hasTriggeredDialogue = false;

    private void Awake()
    {
        phantomFSM = GetComponent<PhantomFSM>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        phantomFSM.OnPhantomDeath += HandlePhantomDeath;
    }

    private void HandlePhantomDeath()
    {
        if(!hasTriggeredDialogue)
        {
            hasTriggeredDialogue = true;
        
            SetOpacity();
            DialogueTrigger();

        }
    }

    private void SetOpacity()
    {
        Color color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0.5f);
    }

    private void OnDialogueFinished()
    {
        animator.SetTrigger("Die");
        Script.SetActive(true);
        
    }

    private void DialogueTrigger()
    {
        DialogueEventManager.Instance.OnDialogueFinished += OnDialogueFinished;

        DialogueManager.Instance.StartDialogue(new DialogueManager.DialogueLine[] {
            new DialogueManager.DialogueLine
            {
                speakerName = "The Phantom",
                dialogueContent = "A disfigured face, a ruined voice... What else can I do? Why must you shatter my dream?",
                avatarSprite = avatarImage
            },
            new DialogueManager.DialogueLine
            {
                speakerName = "The Phantom",
                dialogueContent = "You carry the scent of Christine... Are you going to leave me too?",
                avatarSprite = avatarImage
            }
        });
    }

    public void DieAnimationEnd()
    {
        gameObject.SetActive(false);
        CameraController.Instance.SizeRecovery(1f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        DialogueEventManager.Instance.OnDialogueFinished -= OnDialogueFinished;

        if (phantomFSM != null)
        {
            phantomFSM.OnPhantomDeath -= HandlePhantomDeath;
        }
    }
}
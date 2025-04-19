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

    public bool moveCamera = false;
    public Transform[] moveCameraTarget;
    public float focusTime=3f;
    public float transformCamTime = 1f;
    public float cameraSize = 10f;
    [SerializeField] private Sprite avatarImage;
    public AudioClip switchSFX;
    protected AudioSource audioSource;

    public void Interact()
    {
        if (moveCamera && moveCameraTarget.Length > 0)
        {
            for (int i = 0; i < moveCameraTarget.Length; i++)
            {
                Transform focusPoint = moveCameraTarget[i];
                Vector3 target = focusPoint.position;
                CameraController.Instance.FocusOn(focusPoint, focusTime, transformCamTime, cameraSize);
            }
           
        }
        
        if (isActivated)
        {
            return;
        }
        isActivated = !isActivated;

        if (audioSource != null && switchSFX != null)
        {
            audioSource.PlayOneShot(switchSFX);
        }
        spriteRenderer.flipX = isActivated;
        //eKeyPrompt.SetActive(false);

        DialogueTrigger();


        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                IActivatable activatable = obj.GetComponent<IActivatable>();
                if (activatable != null)
                {
                    activatable.Activate(); 
                }
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

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

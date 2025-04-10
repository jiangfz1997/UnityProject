using UnityEngine;

public class TemporaryDialogue2 : MonoBehaviour
{
    [SerializeField] private Sprite avatarImage;
    private bool hasTriggeredDialogue = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("BOSS2 Collision detected with: " + collision.gameObject.name);
        if (collision.CompareTag("Player") && !hasTriggeredDialogue)
        {
            hasTriggeredDialogue = true;
            DialogueTrigger();

        }
    }

    private void DialogueTrigger()
    {
        DialogueManager.Instance.StartDialogue(new DialogueManager.DialogueLine[] {
            new DialogueManager.DialogueLine
            {
                speakerName = "The Phantom",
                dialogueContent = "Finally, we meet.",
                avatarSprite = avatarImage
            },
            new DialogueManager.DialogueLine
            {
                speakerName = "The Phantom",
                dialogueContent = "It seems like...",
                avatarSprite = avatarImage
            },
            new DialogueManager.DialogueLine
            {
                speakerName = "System",
                dialogueContent = "This level is unfinished. Please interact with the mirror on the right to enter the next level.",
                avatarSprite = avatarImage
            }
        });
    }
}

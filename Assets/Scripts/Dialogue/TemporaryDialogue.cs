using UnityEngine;

public class TemporaryDialogue : MonoBehaviour
{
    [SerializeField] private Sprite avatarImage;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            DialogueTrigger();
            Destroy(gameObject);
        }
    }

    private void DialogueTrigger()
    {
        DialogueManager.Instance.StartDialogue(new DialogueManager.DialogueLine[] {
            new DialogueManager.DialogueLine
            {
                speakerName = "System",
                dialogueContent = "This level is unfinished. Please look for the exit directly to the upper right.",
                avatarSprite = avatarImage
            }
        });
    }
}

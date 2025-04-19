using UnityEngine;

public class TemporaryDialogue2 : MonoBehaviour
{
    [SerializeField] private Sprite avatarImage;
    private bool hasTriggeredDialogue = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("BOSS2 Collision detected with: " + collision.gameObject.name);
        if (collision.CompareTag("Player") && !hasTriggeredDialogue)
        {
            hasTriggeredDialogue = true;

            CameraTrigger();
            DialogueTrigger();

        }
    }

    private void CameraTrigger()
    {
        StartCoroutine(CameraController.Instance.Zoom(10f, 1f));
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
        //     new DialogueManager.DialogueLine
        //     {
        //         speakerName = "The Phantom",
        //         dialogueContent = "It seems like you really want to ESCAPE this castle...",
        //         avatarSprite = avatarImage
        //     },
        //     new DialogueManager.DialogueLine
        //     {
        //         speakerName = "The Phantom",
        //         dialogueContent = "Haha, but why? Only here do you possess these special powers, the very powers that allow you to stand before me.",
        //         avatarSprite = avatarImage
        //     },
        //     new DialogueManager.DialogueLine
        //     {
        //         speakerName = "The Phantom",
        //         dialogueContent = "Everyone lives in this beautiful dream, gaining their unique abilities. Aren't you the same?",
        //         avatarSprite = avatarImage
        //     },
        //     new DialogueManager.DialogueLine
        //     {
        //         speakerName = "The Phantom",
        //         dialogueContent = "So why do you want to leave? Why destroy my world? Without this place, what do I have left?",
        //         avatarSprite = avatarImage
        //     },
        //     new DialogueManager.DialogueLine
        //     {
        //         speakerName = "The Phantom",
        //         dialogueContent = "Enough words, impostor. The final act is about to begin — Sing for me, let me hear your wails.",
        //         avatarSprite = avatarImage
        //     },
        });
    }
}

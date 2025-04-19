using System.Collections;
using UnityEngine;

public class Dialogue_Boss1_before : MonoBehaviour
{
    public Sprite avatarImage;
    private bool hasTriggeredDialogue = false;

    // private void Start()
    // {
    //     dialogueManager = FindObjectOfType<DialogueManager>();
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggeredDialogue)
        {
            hasTriggeredDialogue = true;
            CameraTrigger();
            StartDialogue();

        }
    }

    private void CameraTrigger()
    {
        StartCoroutine(CameraController.Instance.Zoom(10f, 1f));
    }

    private void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(new DialogueManager.DialogueLine[] {
                new DialogueManager.DialogueLine
                {
                    speakerName = "Conductor",
                    dialogueContent = "I've been expecting you, visitor from beyond the castle walls.",
                    avatarSprite = avatarImage
                },
                // new DialogueManager.DialogueLine
                // {
                //     speakerName = "Conductor",
                //     dialogueContent = "Did you know? At some point, I gained the ability to hear the melodies of every soul.",
                //     avatarSprite = avatarImage
                // },
                // new DialogueManager.DialogueLine
                // {
                //     speakerName = "Conductor",
                //     dialogueContent = "From the moment you stepped into this castle, I took notice of yours. The sound of your soul��",
                //     avatarSprite = avatarImage
                // },
                // new DialogueManager.DialogueLine
                // {
                //     speakerName = "Conductor",
                //     dialogueContent = "It sounds... absolutely delicious.",
                //     avatarSprite = avatarImage
                // }
            });

    }

}

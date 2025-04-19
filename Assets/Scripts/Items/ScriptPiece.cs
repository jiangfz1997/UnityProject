using UnityEngine;

public class ScriptPiece : Item
{
    [SerializeField] private int scriptId = 0;
    [SerializeField] private Sprite avatarImage;

    private bool triggerDialogue = false;
    public void Start()
    {
        Debug.Log($"ScriptPiece Start: ID {scriptId}");
        if (CollectionManager.Instance.IsScriptCollected(scriptId))
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnUse(Character character)
    {

        base.OnUse(character);
        if (character is Player player)
        {
            CollectionManager.Instance.CollectScript(scriptId);
            // Debug.Log($"ScriptPiece picked up! ID: {scriptId}");
            if (!triggerDialogue)
            {
                DialogueTrigger();
                triggerDialogue = true;
            }
            Destroy(gameObject);
        }
    }

    private void DialogueTrigger()
    {
        DialogueManager.Instance.StartDialogue(new DialogueManager.DialogueLine[] {
            new DialogueManager.DialogueLine
            {
                speakerName = "You",
                dialogueContent = "This must be a script piece! I can read it in collection panel.",
                avatarSprite = avatarImage
            }
        });
    }

}

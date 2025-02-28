using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("UI���")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image avatar;

    [Header("�Ի�����")]
    [SerializeField] private float typeWriterSpeed = 0.1f;

    private Queue<DialogueLine> dialogueLines;

    private bool isDisplayingLine = false;
    private bool isDialogueFinished = false;

    // �Ի������ݽṹ
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public string dialogueContent;
        public Sprite avatarSprite;
    }

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �ڳ����л�ʱ����
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        dialogueLines = new Queue<DialogueLine>();
        dialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (dialogueBox.activeSelf && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (isDisplayingLine)
            {
                // // ���������ʾ�Ի��У�������ʾ������
                // StopAllCoroutines();
                // dialogueText.text = dialogueLines.Peek().dialogueContent;
                // isDisplayingLine = false;
            }
            else if (dialogueLines.Count > 0)
            {
                DisplayNextLine();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        dialogueLines.Clear();
        foreach (DialogueLine line in lines)
        {
            dialogueLines.Enqueue(line);
        }

        dialogueBox.SetActive(true);
        isDialogueFinished = false;
        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = dialogueLines.Dequeue();
        nameText.text = currentLine.speakerName;
        avatar.sprite = currentLine.avatarSprite;

        isDisplayingLine = true;
        StartCoroutine(TypeDialogue(currentLine.dialogueContent));
    }

    // ���ֻ�Ч��
    private IEnumerator TypeDialogue(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeWriterSpeed);
        }
        isDisplayingLine = false;
    }

    private void EndDialogue()
    {
        Debug.Log("End of conversation.");
        dialogueBox.SetActive(false);
        isDialogueFinished = true;
    }

    public bool IsDialogueFinished()
    {
        return isDialogueFinished;
    }
}

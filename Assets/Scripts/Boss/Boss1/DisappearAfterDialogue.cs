using UnityEngine;
using System.Collections;

public class DisappearAfterDialogue : MonoBehaviour
{

    public GameObject oldNPC;
    public GameObject newNPC;
    public GameObject platform;


    private bool BattleStart = false;

    public bool IfBattleStart
    {
        get { return BattleStart; }
    }

    private float fadeTime = 2.0f;
    private SpriteRenderer oldspriteRenderer;
    private SpriteRenderer newSpriteRenderer;


    private void Awake()
    {
        oldspriteRenderer = oldNPC.GetComponent<SpriteRenderer>();
        newSpriteRenderer = newNPC.GetComponent<SpriteRenderer>();

        if (newNPC != null)
        {
            Debug.Log("Existing sprite found");
            if (newSpriteRenderer != null)
            {
                // 初始设置为不可见
                Color color = newSpriteRenderer.color;
                newSpriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
            }
            platform.SetActive(false);
            newNPC.SetActive(false);
        }
        else
        {
            Debug.Log("Existing Boss sprite not found");
        }
    }

    private void OnEnable()
    {
        if (DialogueEventManager.Instance != null)
        {
            DialogueEventManager.Instance.OnDialogueFinished += OnDialogueFinished;
        }
    }

    private void OnDialogueFinished()
    {

        if (!BattleStart)
        {
            StartCoroutine(TransitionSequence());
            BattleStart = true;
        }
    }

    private void OnDisable()
    {
        if (DialogueEventManager.Instance != null)
        {
            DialogueEventManager.Instance.OnDialogueFinished -= OnDialogueFinished;
        }
    }

    private IEnumerator TransitionSequence()
    {
        yield return StartCoroutine(FadeOut());

        // if (CameraController.Instance != null)
        // {
        //     Vector3 targetPos = new Vector3(0, 0, 0);
        //     float targetSize = 5f;
        //     float duration = 2f;

        //     yield return StartCoroutine(CameraController.Instance.MoveAndZoom(targetPos, targetSize, duration));
        // }



        if (newNPC != null && newSpriteRenderer != null)
        {
            newNPC.SetActive(true);
            yield return StartCoroutine(FadeInNewSprite());
        }

    }

    private IEnumerator FadeOut()
    {
        Color originalColor = oldspriteRenderer.color;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
            oldspriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 隐藏
        oldspriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        oldNPC.SetActive(false);
    }

    private IEnumerator FadeInNewSprite()
    {
        Color originalColor = newSpriteRenderer.color;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
            newSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 确保不透明
        newSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        newNPC.SetActive(true);
        platform.SetActive(true);
    }
}
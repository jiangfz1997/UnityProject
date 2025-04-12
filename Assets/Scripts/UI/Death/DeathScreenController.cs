 using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    public CanvasGroup blackFade;     
    public CanvasGroup deathText;        
    public GameObject buttonGroup;       

    public float delayBeforeFade = 1f;   
    public float fadeDuration = 1.5f; 
    

    private void Start()
    {
        blackFade.alpha = 0;
        deathText.alpha = 0;
        buttonGroup.SetActive(false);
        blackFade.blocksRaycasts = false; 
    }

    public void ShowDeathScreen()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(delayBeforeFade);
        blackFade.blocksRaycasts = true;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / fadeDuration);

            blackFade.alpha = progress;
            deathText.alpha = progress;

            yield return null;
        }

        blackFade.alpha = 1;
        deathText.alpha = 1;


        yield return new WaitForSeconds(0.5f);

        buttonGroup.SetActive(true);
    }

    public void ResetDeathScreen()
    {
        StartCoroutine(ResetSequence());
    }

    private IEnumerator ResetSequence()
    {
        // Step 1: �������ֺͰ�ť��buttonGroup��
        float fadeDuration = 1f;
        float t = 0;

        // ��ȡ��ǰ͸���ȣ��Է���;���ã�
        float startTextAlpha = deathText.alpha;

        CanvasGroup buttonCanvas = buttonGroup.GetComponent<CanvasGroup>();
        if (buttonCanvas == null)
        {
            buttonCanvas = buttonGroup.AddComponent<CanvasGroup>();
            buttonCanvas.alpha = 1f; // Ĭ�ϸ�����ʼֵ
        }
        float startButtonAlpha = buttonCanvas.alpha;
        // ��� buttonGroup û�� CanvasGroup �����һ������ֹ����
        if (buttonCanvas == null)
        {
            buttonCanvas = buttonGroup.AddComponent<CanvasGroup>();
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / fadeDuration);
            deathText.alpha = Mathf.Lerp(startTextAlpha, 0, progress);
            buttonCanvas.alpha = Mathf.Lerp(startButtonAlpha, 0, progress);
            yield return null;
        }

        // Step 2: ��ȫ���ð�ť��
        buttonGroup.SetActive(false);
        buttonGroup.GetComponent<CanvasGroup>().alpha = 255; 

        // Step 3: �ȴ� 1 ��
        yield return new WaitForSeconds(0.5f);

        // Step 4: ������Ļ
        t = 0;
        float startBlackAlpha = blackFade.alpha;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / fadeDuration);
            blackFade.alpha = Mathf.Lerp(startBlackAlpha, 0, progress);
            yield return null;
        }

        // Step 5: ��ɺ�ر� Raycast
        blackFade.blocksRaycasts = false;
    }
}

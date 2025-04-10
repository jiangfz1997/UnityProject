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

    
}

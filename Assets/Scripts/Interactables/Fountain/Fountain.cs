using UnityEngine;
using System.Collections;
public class Fountain : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        Player.Instance.RestoreFullHP();
        //GameObject effectObj = Player.Instance.GetComponentInChildren<Transform>().Find("Effect/RestoreHealth")?.gameObject;

        //if (effectObj != null)
        //{
        //    effectObj.SetActive(false);
        //    effectObj.SetActive(true);
        //    StartCoroutine(HideEffectAfterDelay(effectObj, 1f));
        //}
    }

    //private IEnumerator HideEffectAfterDelay(GameObject go, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    go.SetActive(false);
    //}
}

using UnityEngine;

public class ElementReactionAnimation : MonoBehaviour
{
    public void OnEffectAnimationComplete()
    {
        gameObject.SetActive(false); 
    }
}

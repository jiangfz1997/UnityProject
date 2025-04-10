using UnityEngine;

public class Statue : MonoBehaviour, IInteractable
{
    public GameObject shopUI;
    public void Interact()
    {
        Debug.Log("Interacting with statue");
        if (shopUI != null)
        {
            bool isActive = shopUI.activeSelf;
            shopUI.SetActive(!isActive); 
        }
        else
        {
            Debug.LogError("Shop UI reference is missing!");
        }
    }
}


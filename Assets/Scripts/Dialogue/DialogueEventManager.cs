using System;
using UnityEngine;

public class DialogueEventManager : MonoBehaviour
{
    public static DialogueEventManager Instance;

    public event Action OnDialogueStarted;
    public event Action OnDialogueFinished;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
            
        DontDestroyOnLoad(gameObject);
    }

    public void TriggerDialogueStarted()
    {
        Debug.Log("Start dialogue");
        OnDialogueStarted?.Invoke();
    }

    public void TriggerDialogueFinished()
    {
        Debug.Log("End of dialogue");
        OnDialogueFinished?.Invoke();
    }
}
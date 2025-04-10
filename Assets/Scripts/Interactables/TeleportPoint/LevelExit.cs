using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : TeleportPoint
{
    [SerializeField] private int currentLevelIndex; 

    public override void Interact()
    {
        Debug.Log($"Completing level {currentLevelIndex}");
        LevelManager.Instance.CompleteLevel(currentLevelIndex);
    }
}
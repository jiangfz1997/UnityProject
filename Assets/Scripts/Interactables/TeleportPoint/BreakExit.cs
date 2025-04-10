using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakExit : TeleportPoint
{
    // if user must do something before exit breakroom
    [SerializeField] private bool isNextLevelExit = true;

    public override void Interact()
    {
        // if (!isNextLevelExit)
        // {
        //     Debug.Log("Exiting rejected");
        // }
        Debug.Log("Exiting rest room");      
        LevelManager.Instance.LoadNextLevel(0);
    }
}
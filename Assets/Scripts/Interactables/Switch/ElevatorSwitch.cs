using UnityEngine;

public class ElevatorSwitch : MonoBehaviour, IInteractable
{
    public Elevator elevator;
    private bool isSwitchActivated = false;

    public void Interact()
    {
        if (!isSwitchActivated)
        {
            Debug.Log("E pressed elevator moving");
            isSwitchActivated = true;
            elevator.ActivateElevator(this); 
        }
    }

    public void ResetSwitch()
    {
        isSwitchActivated = false;
        Debug.Log("switch reset");
    }

}

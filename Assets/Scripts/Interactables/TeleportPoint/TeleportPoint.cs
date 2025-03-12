using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public Vector3 teleportPosition;
    public GameSceneSO sceneToLoad;
    public SceneLoadEventSO loadEventSO;
    public void Interact()
    {
        Debug.Log("Teleporting");
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, teleportPosition, true);

    }


}


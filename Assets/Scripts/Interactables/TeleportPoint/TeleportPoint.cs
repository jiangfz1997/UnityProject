using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable, IActivatable
{
    public Vector3 teleportPosition;
    public GameSceneSO sceneToLoad;
    public SceneLoadEventSO loadEventSO;

    public void Activate()
    {
        gameObject.SetActive(true);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        foreach (Collider2D collider in colliders)
        {
            Sign sign = collider.GetComponent<Sign>();
            if (sign != null)
            {
                sign.CheckForInteractable(GetComponent<Collider2D>());
            }
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Teleporting");
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, teleportPosition, true,true);

    }


}


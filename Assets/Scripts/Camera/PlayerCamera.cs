using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Bounds cameraBounds;

    private void OnEnable()
    {
        CameraBounds.OnBoundsChanged += SetBounds;
    }

    private void OnDisable()
    {
        CameraBounds.OnBoundsChanged -= SetBounds;
    }
    private void SetBounds(Bounds newBounds)
    {
        cameraBounds = newBounds;
        Debug.Log($"[PlayerCamera] 收到 OnBoundsChanged 事件，更新 Bounds: {newBounds}");
    }

    private void LateUpdate()
    {
        if (cameraBounds.size != Vector3.zero)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.x = Mathf.Clamp(targetPosition.x, cameraBounds.min.x, cameraBounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, cameraBounds.min.y, cameraBounds.max.y);
            transform.position = targetPosition;
        }
    }
}

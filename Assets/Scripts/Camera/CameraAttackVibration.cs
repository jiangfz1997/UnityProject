using Unity.Cinemachine;
using UnityEngine;

public class CameraAttackVibration : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void TriggerImpulse()
    {
        impulseSource.GenerateImpulse();
    }
}

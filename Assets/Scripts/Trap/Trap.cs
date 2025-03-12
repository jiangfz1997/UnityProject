using UnityEngine;

public class Trap : MonoBehaviour
{
    public TrapType type;
    public bool isMovable;

    
    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
}

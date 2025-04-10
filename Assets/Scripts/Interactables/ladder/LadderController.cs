using UnityEngine;

public class LadderTileController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag("LadderDetect"))
        {
            Player.Instance.SetCanClimb(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LadderDetect"))
        {
            Player.Instance.SetCanClimb(false);
        }
    }
}

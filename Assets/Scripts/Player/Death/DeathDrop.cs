using UnityEngine;

public class DeathDrop : MonoBehaviour
{
    public int goldAmount;
    private float spawnTime;
    private bool collected = false;

    private void Start()
    {
        spawnTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time - spawnTime < 1.5f || collected) return; 

        if (other.CompareTag("Player"))
        {
            collected = true;
            Debug.Log($"[DeathDrop] Player collected {goldAmount} gold!");
            Player.Instance.RetriveGold(goldAmount);
            
            DeathDropManager.Instance.ClearDrop();
            Destroy(gameObject);
        }
    }
}

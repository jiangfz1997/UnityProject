using UnityEngine;

public class Gold : MonoBehaviour
{
    private int goldAmount;
    private bool isCollected;

    public void Initialize(int amount=10)
    {
        goldAmount = amount;
        isCollected = false;
    }

    

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.collectGold(goldAmount);
            isCollected = true;
            Destroy(gameObject);
        }
    }
}

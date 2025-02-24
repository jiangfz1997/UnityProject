using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnPickup(Character character) 
    {
        Debug.Log("Item picked up: " + gameObject.name);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Item triggered: " + other.name);
        if (other.CompareTag("Ground"))
        {
            rb.linearVelocity = Vector2.zero; // 停止移动
            rb.gravityScale = 0; // 关闭重力
            rb.bodyType = RigidbodyType2D.Kinematic; // 让血瓶不会被推开
        }
        else if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                OnPickup(player);
            }
        }
    }


}

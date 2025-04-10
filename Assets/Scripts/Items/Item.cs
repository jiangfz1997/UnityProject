using UnityEngine;


public class Item : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected Rigidbody2D rb;
    public bool storable = false;
    public Sprite icon;
    public int quantity = 1;
    public string itemName;
    public int price;
    public int id = -1;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual Item Clone()
    {
        // Note: This method is not used in the current project
        GameObject cloneObject = new GameObject("ClonedItem_" + itemName);
        Item clone = cloneObject.AddComponent<Item>();

        clone.itemName = this.itemName;
        clone.icon = this.icon;
        clone.quantity = this.quantity;

        cloneObject.hideFlags = HideFlags.HideAndDontSave;

        return clone;
    }

    public virtual void OnPickup(Character character) 
    {
        Debug.Log("Use item!!");
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        else if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                if (storable)
                {
                    player.StoreItem(this, (success) =>
                    {
                        if (success)
                        {
                         
                            Destroy(gameObject); 
                        }
                        else
                        {
                            Debug.Log("Backpack is full!");
                            // TODO: NOTIFICATION ON UI, BAG IS FULL
                        }
                    });
                }
                else
                {
                    OnPickup(player);
                }
            }
        }
    }


}

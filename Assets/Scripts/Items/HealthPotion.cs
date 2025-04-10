using UnityEngine;

public class HealthPotion : Item
{
    public int healAmount = 50;
    // TODO: Different healAmount among different health potion?

    public override void OnPickup(Character character)
    {

        //base.OnPickup(character);
        if (character is Player player)
        {
            player.Heal(healAmount);
            Debug.Log($"HealthPotion picked up! Restored {healAmount} HP.");
        }
        quantity--;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 

        if (collision.collider.CompareTag("Ground")) // 🚀 Ensure it detects the ground
        {
            rb.linearVelocity = Vector2.zero; // 🚀 Stop movement
            rb.gravityScale = 0; // 🚀 Disable gravity
            rb.bodyType = RigidbodyType2D.Kinematic; // 🚀 Make it no longer affected by physics

            // 🚀 Ignore all future collisions with other physics objects (monsters, players, etc.)
            Collider2D potionCollider = GetComponent<Collider2D>();
            Collider2D[] allColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);

            foreach (Collider2D col in allColliders)
            {
                if (col != potionCollider) // 🚀 Ignore collision with everything except itself
                {
                    Physics2D.IgnoreCollision(potionCollider, col, true);
                }
            }
        }
    }
}

using UnityEngine;

public class AttackUpPotion : Item
{
    public float attackUpDuration = 100f;
    public float attackUpPercent = 0.2f; 
    // TODO: Different healAmount among different health potion?

    public override void OnUse(Character character)
    {

        //base.OnUse(character);
        if (character is Player player)
        {
            player.ApplyBuff(BuffType.AttackUp, attackUpDuration, attackUpPercent);
            Debug.Log($"[Potion] {player.name} used Attack Up Potion! Attack increased by {attackUpPercent * 100}% for {attackUpDuration} seconds.");
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

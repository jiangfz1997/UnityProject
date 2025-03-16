using UnityEngine;

public class PlayerAttack : Attack
{
    public float knockbackForce;


    protected override void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("!!!!!!!PlayerAttack ´¥·¢ {collision.name}!!!!!!");
        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            //character.SetDamageHandler(new ConfigurableDamage(true, true, knockbackForce)); 
            character.TakeDamage(transform, damage, knockbackForce, damageType);
        }
    }
    


}

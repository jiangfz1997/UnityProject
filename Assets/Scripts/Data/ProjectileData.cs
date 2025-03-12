using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Game/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public string projectileName;
    public GameObject prefab;
    public float speed;
    public float damage;
    public float knockbackForce;
    public DamageType damageType;
}

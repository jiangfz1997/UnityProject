using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ProjectileDatabase", menuName = "Game/Projectile Database")]
public class ProjectileDatabase : ScriptableObject
{
    public List<ProjectileData> projectiles;

    public ProjectileData GetProjectile(string name)
    {
        return projectiles.Find(p => p.projectileName == name);
    }
}
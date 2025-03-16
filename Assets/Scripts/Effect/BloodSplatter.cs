using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    public GameObject bloodDecalPrefab; // **血迹预制体**

    private void OnParticleCollision(GameObject other)
    {
        // **获取碰撞点**
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int numCollisionEvents = Mathf.Min(GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents),3);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 hitPoint = collisionEvents[i].intersection;
            hitPoint.z = 0; // **保证在 2D 平面**
            hitPoint.y -= Random.Range(1f, 2f); // **随机偏移量**
            GameObject bloodDecal = Instantiate(bloodDecalPrefab, hitPoint, Quaternion.identity);
            bloodDecal.transform.SetParent(other.transform); // **让血迹附着在 Tilemap**
        }
    }
}

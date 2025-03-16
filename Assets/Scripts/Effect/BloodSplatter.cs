using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    public GameObject bloodDecalPrefab; // **Ѫ��Ԥ����**

    private void OnParticleCollision(GameObject other)
    {
        // **��ȡ��ײ��**
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int numCollisionEvents = Mathf.Min(GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents),3);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 hitPoint = collisionEvents[i].intersection;
            hitPoint.z = 0; // **��֤�� 2D ƽ��**
            hitPoint.y -= Random.Range(1f, 2f); // **���ƫ����**
            GameObject bloodDecal = Instantiate(bloodDecalPrefab, hitPoint, Quaternion.identity);
            bloodDecal.transform.SetParent(other.transform); // **��Ѫ�������� Tilemap**
        }
    }
}

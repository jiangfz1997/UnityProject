using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager Instance;
    public GameObject bloodParticlePrefab; 

    private void Awake()
    {
        Instance = this; 
    }

    public void SpawnBlood(Vector3 position)
    {
        GameObject bloodEffect = Instantiate(bloodParticlePrefab, position, Quaternion.identity);
        bloodEffect.GetComponent<ParticleSystem>().Play();
        Destroy(bloodEffect, 2f); 
    }
}

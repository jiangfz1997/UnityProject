using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserNoteController : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private GameObject laserWarningPrefab; 
    [SerializeField] private GameObject laserLinePrefab; 
    [SerializeField] private float warningDuration = 2f; 
    [SerializeField] private float laserDuration = 2f; 
    [SerializeField] private float damageAmount = 10f; 
    [SerializeField] private float knockbackForce = 2f; 
    
    
    void Start()
    {
        StartCoroutine(LaserSequence());
    }
    
    private IEnumerator LaserSequence()
    {
        // ��ʾ��
        GameObject warningLaser = CreateLaser(laserWarningPrefab);

        yield return new WaitForSeconds(warningDuration);

        Destroy(warningLaser);
        
        // �˺���
        GameObject damageLaser = CreateLaser(laserLinePrefab);

        LaserDamager damager = damageLaser.GetComponent<LaserDamager>();

        damager.Initialize(damageAmount);

        yield return new WaitForSeconds(laserDuration);

        Destroy(damageLaser);
        Destroy(gameObject);
    }
    
    private GameObject CreateLaser(GameObject prefab)
    {
        Vector2 notePosition = transform.position;
        
        float laserLength = notePosition.y + 12.7F;

        // Debug.Log("Laser Pos" + notePosition);

        GameObject laser = Instantiate(prefab, notePosition, Quaternion.identity);

        Vector3 newScale = laser.transform.localScale;
        newScale.y = laserLength;
        laser.transform.localScale = newScale;
        
        return laser;
    }
}

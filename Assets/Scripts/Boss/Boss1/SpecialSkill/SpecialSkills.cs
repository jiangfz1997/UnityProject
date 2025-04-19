using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpecialSkills : MonoBehaviour
{
    [SerializeField] private GameObject explosiveNotePrefab;
    [SerializeField] private int noteCount1 = 5;
    [SerializeField] private int explosiveTime = 3;
    [SerializeField] private float spawnRadius = 2f;

    [SerializeField] private GameObject laserNotePrefab;
    [SerializeField] private int noteCount2 = 5;
    [SerializeField] private int laserTime = 2;
    [SerializeField] private float ceilingHeight = 10f;
    [SerializeField] private float minX = -8f; // ���ɷ�Χ��СX����
    [SerializeField] private float maxX = 8f; // ���ɷ�Χ���X����


    public void ExplosiveNotes()
    {
        GenerateExplosiveNotes();
        for (int t = 1; t < explosiveTime; t++)
        {
            Invoke("GenerateExplosiveNotes", 5f * t);
        }

    }

    private void GenerateExplosiveNotes()
    {
        for (int i = 0; i < noteCount1; i++)
        {
            // ��������λ��
            float angle = i * (360f / noteCount1);
            Vector2 spawnPos = (Vector2)transform.position +
                            (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right * spawnRadius);

            Instantiate(explosiveNotePrefab, spawnPos, Quaternion.identity);
        }
    }

    public void LaserNotes()
    {
        GenerateLaserNotes();
        for (int t = 1; t < laserTime; t++)
        {
            Invoke("GenerateLaserNotes", 5f * t);
        }

    }

    private void GenerateLaserNotes() 
    {
        List<Vector2> spawnPositions = new List<Vector2>();
        
        // ���ɲ��ص������λ��
        for (int i = 0; i < noteCount2; i++)
        {
            Vector2 newPos = GetRandomPosition();
            float minDistance = 1.5f;
            bool validPosition = false;
            
            int attempts = 0;
            while (!validPosition && attempts < 10)
            {
                validPosition = true;
                foreach (Vector2 pos in spawnPositions)
                {
                    if (Vector2.Distance(newPos, pos) < minDistance)
                    {
                        validPosition = false;
                        newPos = GetRandomPosition();
                        break;
                    }
                }
                attempts++;
            }
            
            spawnPositions.Add(newPos);

            GameObject note = Instantiate(laserNotePrefab, newPos, Quaternion.identity);

        }

    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(UnityEngine.Random.Range(minX, maxX), ceilingHeight);
    }

    public void SharpShield()
    {

    }


}
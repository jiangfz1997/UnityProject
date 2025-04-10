using UnityEngine;

public class BloodManager : MonoBehaviour
{
    private static BloodManager _instance;
    public GameObject bloodParticlePrefab;

    public static BloodManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���Դӳ������ҵ����е� BloodManager
                _instance = FindFirstObjectByType<BloodManager>();

                // ���������û�� BloodManager���Ŵ����µ� GameObject�����Ƽ���
                if (_instance == null)
                {
                    GameObject go = new GameObject("BloodManager");
                    _instance = go.AddComponent<BloodManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // ȷ���������ظ�
        }
    }

    public void SpawnBlood(Vector3 position)
    {
        if (bloodParticlePrefab == null)
        {
            Debug.LogError("bloodParticlePrefab is null! Please assign it in the Inspector.");
            return;
        }

        GameObject bloodEffect = Instantiate(bloodParticlePrefab, position, Quaternion.identity);
        bloodEffect.GetComponent<ParticleSystem>().Play();
        Destroy(bloodEffect, 2f);
    }
}

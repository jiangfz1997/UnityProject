using UnityEngine;

public class AfterBattle: MonoBehaviour
{
    private BossFSM bossFSM;

    [SerializeField] private GameObject Script;


    private void Awake()
    {
        bossFSM = GetComponent<BossFSM>();
        bossFSM.OnBossDeath += HandleBossDeath;
    }

    private void HandleBossDeath()
    {
        Script.SetActive(true);
        CameraController.Instance.SizeRecovery(1f);
        Destroy(gameObject, 0.5f);
    }

    private void OnDestroy()
    {
        if (bossFSM != null)
        {
            bossFSM.OnBossDeath -= HandleBossDeath;
        }
    }
}
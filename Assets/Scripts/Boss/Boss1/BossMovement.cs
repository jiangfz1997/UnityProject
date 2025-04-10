using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Vector2 areaSize = new Vector2(5, 5); // Boss移动的区域大小
    public float speed = 1.4f;

    private Vector2 targetPosition; 

    public DisappearAfterDialogue Trigger;

    void Start()
    {
        SetNewTargetPosition();
    }

    void Update()
    {
        if(Trigger.IfBattleStart)
            randomMove();
    }

    void randomMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 如果接近目标位置，设置一个新位置
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        targetPosition = new Vector2(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            Random.Range(-areaSize.y / 2, areaSize.y / 2)
        );
    }
}

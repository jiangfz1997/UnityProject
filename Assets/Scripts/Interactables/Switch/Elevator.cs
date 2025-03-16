using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform bottomPoint; // 电梯最低点
    public Transform topPoint; // 电梯最高点
    public Transform elevatorPlatform; // 电梯本身
    public float speed = 2f; // 电梯移动速度
    public ElevatorSwitch BottomSwitch; // 底部开关（需在 Inspector 手动赋值）
    public ElevatorSwitch TopSwitch; // 顶部开关（需在 Inspector 手动赋值）

    private bool isActive = false; // 电梯是否激活
    private bool playerOnElevator = false; // 玩家是否在电梯上
    private Transform targetPosition; // 目标点
    private GameObject player; // 玩家对象
    private PlayerController playerController; // 记录玩家控制器
    private ElevatorSwitch currentSwitch; // 当前触发的开关

    void Start()
    {
        targetPosition = bottomPoint; // 初始位置
        //初始化时，将 bottomPoint 设为电梯的初始位置，确保返回正确
        bottomPoint.position = new Vector2(elevatorPlatform.position.x, elevatorPlatform.position.y);
    }

    void Update()
    {
        if (isActive && playerOnElevator)
        {
            MoveElevator();
        }
    }

    public void ActivateElevator(ElevatorSwitch switchTrigger)
    {
        //if (playerOnElevator) // 只有玩家在电梯上，才启动
        //{


        //    if (player != null)
        //    {
        //        playerController = player.GetComponent<PlayerController>(); // 获取玩家控制器
        //        if (playerController != null)
        //        {
        //            playerController.DisableInput(); // 禁用玩家输入
        //        }
        //    }
        //}
        Debug.Log("elevator position=" + elevatorPlatform.position.y);
        Debug.Log("bottom position=" + bottomPoint.position.y);
        Debug.Log("top position=" + topPoint.position.y);
        isActive = true;
        currentSwitch = switchTrigger; // 记录哪个开关触发了电梯
                                       //targetPosition = (elevatorPlatform.position.y <= bottomPoint.position.y) ? topPoint : bottomPoint;
                                       // 强制目标位置的 X 坐标和电梯 X 坐标一致，防止斜着移动

        //if (Mathf.Approximately(elevatorPlatform.position.y, bottomPoint.position.y))
        //{
        //    targetPosition = topPoint; // 电梯完全在底部，目标设为顶部
        //}
        //else
        //{
        //    targetPosition = bottomPoint; // 其他情况，目标设为底部
        //}
        if (currentSwitch == BottomSwitch)
        {
            targetPosition = topPoint; // 如果按下底部开关，电梯上升
        }
        else if (currentSwitch == TopSwitch)
        {
            targetPosition = bottomPoint; // 如果按下顶部开关，电梯下降
        }
        targetPosition.position = new Vector2(elevatorPlatform.position.x, targetPosition.position.y);
        
        Debug.Log("targetPosition=" + targetPosition.position);
        Debug.Log("currentSwitch=" + currentSwitch);
        Debug.Log("switch on, wawiting for player");
    }

    private void MoveElevator()
    {
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>(); // 获取玩家控制器
            if (playerController != null)
            {
                playerController.DisableInput(); // 禁用玩家输入
            }
        }
        Debug.Log("elevatorPlatform.position=" + elevatorPlatform.position);
        Debug.Log("targetPosition.position=" + targetPosition.position);
        Debug.Log("elevator is moving.current：" + elevatorPlatform.position + "，target：" + targetPosition.position);

        //elevatorPlatform.position = Vector2.MoveTowards(elevatorPlatform.position, targetPosition.position, speed * Time.deltaTime);
        // 确保电梯只在 Y 轴上移动
        elevatorPlatform.position = new Vector2(
            elevatorPlatform.position.x,
            Mathf.MoveTowards(elevatorPlatform.position.y, targetPosition.position.y, speed * Time.deltaTime)
        );

        if (Mathf.Abs(elevatorPlatform.position.y - targetPosition.position.y) < 0.01f)
        {
            elevatorPlatform.position = new Vector2(elevatorPlatform.position.x, targetPosition.position.y); //确保最终位置精准
            isActive = false;
            if (playerController != null)
            {
                playerController.EnableInput(); // 恢复玩家输入
            }

            if (currentSwitch != null)
            {
                currentSwitch.ResetSwitch(); // 复位开关
            }
            Debug.Log("arrived target position");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnElevator = true;
            player = other.gameObject;
            other.transform.SetParent(elevatorPlatform);
            Debug.Log("player is in elevator");
            //if (isActive && playerOnElevator)
            //{
               // MoveElevator();
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if (other.transform.parent == elevatorPlatform) // 只有当玩家还是电梯子对象时，才解除父子关系
        //{
        //    other.transform.SetParent(null);
        //}

        //playerOnElevator = false;
        //player = null;
        //Debug.Log("player leaves elevator");
        //if (other.CompareTag("Player"))
        //{
        //    playerOnElevator = false;
        //    player = null;
        //    other.transform.SetParent(null);
        //    Debug.Log("player leaves elevator");
        //}
        if (other.CompareTag("Player"))
        {
            playerOnElevator = false;
            player = null;

            if (other.transform.parent == elevatorPlatform)
            {
                other.transform.SetParent(null); // 解绑，防止玩家受电梯影响
            }

            //确保玩家跳下电梯后能恢复行动
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.EnableInput();
            }

            Debug.Log("player leaves elevator. action on");
        }
    }
}

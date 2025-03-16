using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform bottomPoint; // ������͵�
    public Transform topPoint; // ������ߵ�
    public Transform elevatorPlatform; // ���ݱ���
    public float speed = 2f; // �����ƶ��ٶ�
    public ElevatorSwitch BottomSwitch; // �ײ����أ����� Inspector �ֶ���ֵ��
    public ElevatorSwitch TopSwitch; // �������أ����� Inspector �ֶ���ֵ��

    private bool isActive = false; // �����Ƿ񼤻�
    private bool playerOnElevator = false; // ����Ƿ��ڵ�����
    private Transform targetPosition; // Ŀ���
    private GameObject player; // ��Ҷ���
    private PlayerController playerController; // ��¼��ҿ�����
    private ElevatorSwitch currentSwitch; // ��ǰ�����Ŀ���

    void Start()
    {
        targetPosition = bottomPoint; // ��ʼλ��
        //��ʼ��ʱ���� bottomPoint ��Ϊ���ݵĳ�ʼλ�ã�ȷ��������ȷ
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
        //if (playerOnElevator) // ֻ������ڵ����ϣ�������
        //{


        //    if (player != null)
        //    {
        //        playerController = player.GetComponent<PlayerController>(); // ��ȡ��ҿ�����
        //        if (playerController != null)
        //        {
        //            playerController.DisableInput(); // �����������
        //        }
        //    }
        //}
        Debug.Log("elevator position=" + elevatorPlatform.position.y);
        Debug.Log("bottom position=" + bottomPoint.position.y);
        Debug.Log("top position=" + topPoint.position.y);
        isActive = true;
        currentSwitch = switchTrigger; // ��¼�ĸ����ش����˵���
                                       //targetPosition = (elevatorPlatform.position.y <= bottomPoint.position.y) ? topPoint : bottomPoint;
                                       // ǿ��Ŀ��λ�õ� X ����͵��� X ����һ�£���ֹб���ƶ�

        //if (Mathf.Approximately(elevatorPlatform.position.y, bottomPoint.position.y))
        //{
        //    targetPosition = topPoint; // ������ȫ�ڵײ���Ŀ����Ϊ����
        //}
        //else
        //{
        //    targetPosition = bottomPoint; // ���������Ŀ����Ϊ�ײ�
        //}
        if (currentSwitch == BottomSwitch)
        {
            targetPosition = topPoint; // ������µײ����أ���������
        }
        else if (currentSwitch == TopSwitch)
        {
            targetPosition = bottomPoint; // ������¶������أ������½�
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
            playerController = player.GetComponent<PlayerController>(); // ��ȡ��ҿ�����
            if (playerController != null)
            {
                playerController.DisableInput(); // �����������
            }
        }
        Debug.Log("elevatorPlatform.position=" + elevatorPlatform.position);
        Debug.Log("targetPosition.position=" + targetPosition.position);
        Debug.Log("elevator is moving.current��" + elevatorPlatform.position + "��target��" + targetPosition.position);

        //elevatorPlatform.position = Vector2.MoveTowards(elevatorPlatform.position, targetPosition.position, speed * Time.deltaTime);
        // ȷ������ֻ�� Y �����ƶ�
        elevatorPlatform.position = new Vector2(
            elevatorPlatform.position.x,
            Mathf.MoveTowards(elevatorPlatform.position.y, targetPosition.position.y, speed * Time.deltaTime)
        );

        if (Mathf.Abs(elevatorPlatform.position.y - targetPosition.position.y) < 0.01f)
        {
            elevatorPlatform.position = new Vector2(elevatorPlatform.position.x, targetPosition.position.y); //ȷ������λ�þ�׼
            isActive = false;
            if (playerController != null)
            {
                playerController.EnableInput(); // �ָ��������
            }

            if (currentSwitch != null)
            {
                currentSwitch.ResetSwitch(); // ��λ����
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
        //if (other.transform.parent == elevatorPlatform) // ֻ�е���һ��ǵ����Ӷ���ʱ���Ž�����ӹ�ϵ
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
                other.transform.SetParent(null); // ��󣬷�ֹ����ܵ���Ӱ��
            }

            //ȷ��������µ��ݺ��ָܻ��ж�
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.EnableInput();
            }

            Debug.Log("player leaves elevator. action on");
        }
    }
}

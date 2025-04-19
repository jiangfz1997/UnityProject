using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform bottomPoint; 
    public Transform topPoint; 
    public Transform elevatorPlatform; 
    public float speed = 2f; 
    public ElevatorSwitch BottomSwitch; 
    public ElevatorSwitch TopSwitch; 

    private bool isActive = false;
    private bool playerOnElevator = false;
    private Transform targetPosition;
    private GameObject player; 
    private PlayerController playerController; 
    private ElevatorSwitch currentSwitch; 

    void Start()
    {
        targetPosition = bottomPoint; 

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
        
        
        Debug.Log("elevator position=" + elevatorPlatform.position.y);
        Debug.Log("bottom position=" + bottomPoint.position.y);
        Debug.Log("top position=" + topPoint.position.y);
        isActive = true;
        currentSwitch = switchTrigger;
                                       
        if (currentSwitch == BottomSwitch)
        {
            targetPosition = topPoint; 
        }
        else if (currentSwitch == TopSwitch)
        {
            targetPosition = bottomPoint;
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
            playerController = player.GetComponent<PlayerController>(); 
            if (playerController != null)
            {
                playerController.DisableInput(); 
            }
        }
        //Debug.Log("elevatorPlatform.position=" + elevatorPlatform.position);
        //Debug.Log("targetPosition.position=" + targetPosition.position);
        //Debug.Log("elevator is moving.current：" + elevatorPlatform.position + "，target：" + targetPosition.position);

        //elevatorPlatform.position = Vector2.MoveTowards(elevatorPlatform.position, targetPosition.position, speed * Time.deltaTime);
  
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
                playerController.EnableInput(); 
            }

            if (currentSwitch != null)
            {
                currentSwitch.ResetSwitch(); 
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
            //other.transform.SetParent(elevatorPlatform);
            Debug.Log("player is in elevator");
            //if (isActive && playerOnElevator)
            //{
               // MoveElevator();
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if (other.transform.parent == elevatorPlatform) 
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
                other.transform.SetParent(null);
            }

            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.EnableInput();
            }

            Debug.Log("player leaves elevator. action on");
        }
    }
}

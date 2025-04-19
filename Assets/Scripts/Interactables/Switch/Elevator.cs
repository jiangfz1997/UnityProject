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
    private bool isShaking = false;
    private float shakeDuration = 2f;
    private float shakeTimer = 0f;
    private Vector3 initialPlatformPosition;
    private Vector3 lastPlatformPosition;
    [SerializeField] private AudioSource moveAudioSource;
    void Start()
    {
        targetPosition = topPoint;
        lastPlatformPosition = elevatorPlatform.position;
        moveAudioSource = GetComponent<AudioSource>();
        topPoint.position = new Vector2(elevatorPlatform.position.x, elevatorPlatform.position.y);
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            MoveElevator();
            if (playerOnElevator && player != null)
            {
                Vector3 delta = elevatorPlatform.position - lastPlatformPosition;
                player.transform.position += delta; 
            }

            lastPlatformPosition = elevatorPlatform.position;
        }
    }
    private IEnumerator DelayStartElevator()
    {
        yield return new WaitForSeconds(0.3f);
        ActivateElevator();
    }
    //public void ActivateElevator(ElevatorSwitch switchTrigger)
    //{


    //    //Debug.Log("elevator position=" + elevatorPlatform.position.y);
    //    //Debug.Log("bottom position=" + bottomPoint.position.y);
    //    //Debug.Log("top position=" + topPoint.position.y);
    //    isActive = true;
    //    currentSwitch = switchTrigger;

    //    if (currentSwitch == BottomSwitch)
    //    {
    //        targetPosition = topPoint;
    //    }
    //    else if (currentSwitch == TopSwitch)
    //    {
    //        targetPosition = bottomPoint;
    //    }
    //    targetPosition.position = new Vector2(elevatorPlatform.position.x, targetPosition.position.y);

    //    //Debug.Log("targetPosition=" + targetPosition.position);
    //    //Debug.Log("currentSwitch=" + currentSwitch);
    //    //Debug.Log("switch on, wawiting for player");
    //}

    public void ActivateElevator(ElevatorSwitch switchTrigger=null)
    {

        if (isActive) return; 

        if(switchTrigger != null)
            currentSwitch = switchTrigger;

        if (playerOnElevator)
        {
            
            targetPosition = IsAtPosition(bottomPoint.position) ? topPoint : bottomPoint;
            Debug.Log("🚀 Player on elevator, moving to opposite floor");
        }
        else
        {
            targetPosition = (currentSwitch == BottomSwitch) ? bottomPoint : topPoint;
            Debug.Log("📞 Summoning elevator to current floor");
        }

        isActive = true; // 启动移动
        if (moveAudioSource != null && !moveAudioSource.isPlaying)
        {
            //moveAudioSource.Play();
            StartCoroutine(FadeIn(moveAudioSource, 0.5f));
        }

    }

    private IEnumerator FadeIn(AudioSource source, float duration)
    {
        float timer = 0f;
        source.volume = 0f;
        source.Play();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }

        source.volume = 1f;
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // 重置音量
    }

    private bool IsAtPosition(Vector3 targetPos)
    {
        return Mathf.Abs(elevatorPlatform.position.y - targetPos.y) < 0.01f;
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

            if (moveAudioSource != null && moveAudioSource.isPlaying)
            {
                //moveAudioSource.Stop();
                StartCoroutine(FadeOut(moveAudioSource, 0.5f)); 
            }
            Debug.Log("arrived target position");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnElevator = true;
            //isActive = true;
            player = other.gameObject;
            //other.transform.SetParent(elevatorPlatform);
            Debug.Log("player is in elevator");
            StartCoroutine(DelayStartElevator());           
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

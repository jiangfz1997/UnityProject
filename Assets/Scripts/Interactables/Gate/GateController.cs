using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour,IActivatable
{
    public Transform gateBar;
    public float openHeight = 2f;  // height of the gate when it's open
    public float speed = 2f;  // speed of the gate
    public float shakeIntensity = 0.05f; // shake intensity
    public int shakeCount = 5;  // shake count
    public float shakeDuration = 0.02f; // shake duration in seconds
    public bool isTouchOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;
    private GameObject LevelExitPoint;

    public AudioClip openSFX;
    protected AudioSource audioSource;

    void Start()
    {
        closedPosition = gateBar.position;
        openPosition = closedPosition + new Vector3(0, openHeight, 0);
        audioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        LevelExitPoint = transform.Find("LevelExitPoint")?.gameObject;

        if (LevelExitPoint != null)
        {
            LevelExitPoint.SetActive(false); 
        }
        else
        {
            Debug.LogWarning("LevelExitPoint not found under " + gameObject.name);
        }
    }

    void Update()
    {
        if (isOpening)
        {
            gateBar.position = Vector3.Lerp(gateBar.position, openPosition, Time.deltaTime * speed);
        }
    }

    public void OpenDoor()
    {
        if (!isOpening)
        {
            isOpening = true;

            StartCoroutine(ShakeAndOpen());
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //return;
        if (isTouchOpen && other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }
    IEnumerator ShakeAndOpen()
    {
        if (audioSource != null && openSFX != null)
        {
            audioSource.PlayOneShot(openSFX);
        }
        else
        {
            Debug.LogWarning("AudioSource or Open SFX is not assigned.");
        }
        if (LevelExitPoint != null)
        {
            LevelExitPoint.SetActive(true);
            Debug.Log("Teleport Point Activated!");
        }
        for (int i = 0; i < shakeCount; i++)
        {
            gateBar.position += new Vector3(shakeIntensity, 0, 0);
            yield return new WaitForSeconds(shakeDuration);
            gateBar.position -= new Vector3(shakeIntensity, 0, 0);
            yield return new WaitForSeconds(shakeDuration);
        }

        while (Vector3.Distance(gateBar.position, openPosition) > 0.01f)
        {
            gateBar.position = Vector3.Lerp(gateBar.position, openPosition, Time.deltaTime * speed);
            yield return null;
        }
        
    }

    public void Activate()
    {
        if (!isOpening)
        {
            isOpening = true;
            StartCoroutine(ShakeAndOpen());
        }
    }
}

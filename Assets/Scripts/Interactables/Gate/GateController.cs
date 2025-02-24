using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour
{
    public Transform gateBar;
    public float openHeight = 2f;  // height of the gate when it's open
    public float speed = 2f;  // speed of the gate
    public float shakeIntensity = 0.05f; // shake intensity
    public int shakeCount = 5;  // shake count
    public float shakeDuration = 0.02f; // shake duration in seconds

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;

    void Start()
    {
        closedPosition = gateBar.position;
        openPosition = closedPosition + new Vector3(0, openHeight, 0);
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
        return;
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }
    IEnumerator ShakeAndOpen()
    {
        
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
}

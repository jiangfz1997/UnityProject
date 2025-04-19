using System;
using UnityEngine;

public class CameraTriggerExample
{

    public CameraController cameraController;
    public Transform playerTransform;
    public Transform targetTransform;
    public float targetSize = 5f;
    public float duration = 1f;

    private void Start()
    {
        // Assuming you have a reference to the CameraController and playerTransform
        cameraController = CameraController.Instance;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         Vector3 targetPos = new Vector3(targetTransform.position.x, targetTransform.position.y, 0);
    //         cameraController.StartCoroutine(cameraController.MoveAndZoom(targetPos, targetSize, duration));
    //         cameraController.StartCoroutine(cameraController.FollowPlayer(duration));
    //         // GameObject.SetActive(false);
    //     }
    // }


}

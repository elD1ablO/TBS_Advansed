using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    float moveSpeed = 5f;
    float rotationSpeed = 100f;

    Vector3 targetFollowOffset;
    CinemachineTransposer cinemachineTransposer;

    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    void Update()
    {
        Movement();
        Rotation();
        Zoom();
    }

    void Zoom()
    {
        
        float zoomIncreaseAmount = 1f;
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;        

        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

    void Rotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    void Movement()
    {
        Vector3 inputMoveDirection = InputManager.Instance.GetCameraMoveVector();
       
        Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}

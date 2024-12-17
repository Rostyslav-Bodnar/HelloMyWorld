using Cinemachine;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float moveSpeed = 25f;
    private float edgeScrollSize = 20f;


    [SerializeField] private bool useEdgeScroll = false;

    private CameraRotationController cameraRotationController;
    private CameraZoomController zoomController;

    private void Awake()
    {
        cameraRotationController = new CameraRotationController();
        zoomController = new CameraZoomController(cinemachineVirtualCamera);
    }

    private void Update()
    {
        HandleCameraMoving();
        cameraRotationController.HandleCameraRotation(transform);
        zoomController.HandleCameraZoom_MoveForward();
    }

    private void HandleCameraMoving()
    {
        Vector3 InputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) InputDirection.z += 1f;
        if (Input.GetKey(KeyCode.S)) InputDirection.z -= 1f;
        if (Input.GetKey(KeyCode.A)) InputDirection.x -= 1f;
        if (Input.GetKey(KeyCode.D)) InputDirection.x += 1f;

        if(useEdgeScroll)
        {
            if (Input.mousePosition.x < edgeScrollSize) InputDirection.x -= 1f;
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) InputDirection.x += 1f;
            if (Input.mousePosition.y < edgeScrollSize) InputDirection.z -= 1f;
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) InputDirection.z += 1f;
        }
        
        Vector3 moveDirection = transform.forward * InputDirection.z + transform.right * InputDirection.x;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}

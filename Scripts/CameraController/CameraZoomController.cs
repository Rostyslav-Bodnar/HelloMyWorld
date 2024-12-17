using Cinemachine;
using UnityEngine;

public class CameraZoomController
{

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float zoomSpeed = 10f;

    private float targetFieldOfView = 60;
    private float minFieldOfView = 10;
    private float maxFieldOfView = 60;

    private float minFollowOffset = 5f;
    private float maxFollowOffset = 50f;
    private Vector3 followOffset;

    public CameraZoomController(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        this.cinemachineVirtualCamera = cinemachineVirtualCamera;
        followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

    }

    public void HandleCameraZoom_FieldOfView(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView -= 5f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFieldOfView += 5f;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, minFieldOfView, maxFieldOfView);
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
    }
    public void HandleCameraZoom_MoveForward()
    {
        Vector3 zoomDirection = followOffset.normalized;
        float zoomAmount = 3f;
        if (Input.mouseScrollDelta.y > 0)
        {
            followOffset -= zoomDirection * zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOffset += zoomDirection * zoomAmount;
        }
        if (followOffset.magnitude < minFollowOffset)
        {
            followOffset = zoomDirection * minFollowOffset;
        }
        if (followOffset.magnitude > maxFollowOffset)
        {
            followOffset = zoomDirection * maxFollowOffset;
        }

        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset,
            followOffset, Time.deltaTime * zoomSpeed);

    }

}

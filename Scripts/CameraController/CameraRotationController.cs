using UnityEngine;

public class CameraRotationController
{
    private float rotateSpeed = 100f;

    public void HandleCameraRotation(Transform cameraController)
    {
        float rotateDirection = 0f;

        if (Input.GetKey(KeyCode.Q)) rotateDirection += 1f;
        if (Input.GetKey(KeyCode.E)) rotateDirection -= 1f;

        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            rotateDirection = Mathf.Lerp(rotateDirection, -mouseX * rotateSpeed, 0.1f);
        }

        cameraController.eulerAngles += new Vector3(0, rotateDirection * rotateSpeed * Time.deltaTime, 0);
    }

}

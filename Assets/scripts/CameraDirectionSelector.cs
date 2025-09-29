using UnityEngine;

public class CameraDirectionSelector : MonoBehaviour
{
    public Camera NorthCamera;
    public Camera EastCamera;
    public Camera SouthCamera;
    public Camera WestCamera;
    private int currentCamera = 0;
    private int rotationAngle = 0;

    void DisableAllCameras()
    {
        NorthCamera.enabled = false;
        EastCamera.enabled = false;
        SouthCamera.enabled = false;
        WestCamera.enabled = false;
    }

    void SetCameraDirection()
    {
        switch (currentCamera)
        {
            case 0:
                DisableAllCameras();
                NorthCamera.enabled = true;
                break;
            case 1:
                DisableAllCameras();
                EastCamera.enabled = true;
                break;
            case 2:
                DisableAllCameras();
                SouthCamera.enabled = true;
                break;
            case 3:
                DisableAllCameras();
                WestCamera.enabled = true;
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetCameraDirection();
    }

    void rotatePlayer()
    {
        if (rotationAngle >= 360)
        {
            rotationAngle = rotationAngle - 360;
        }
        if (rotationAngle < 0)
        {
            rotationAngle = rotationAngle + 360;
        }

        transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentCamera = currentCamera + 1;
            if (currentCamera > 3)
            {
                currentCamera = 0;
            }
            SetCameraDirection();

            rotationAngle = rotationAngle + 90;
            rotatePlayer();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentCamera = currentCamera - 1;
            if (currentCamera < 0)
            {
                currentCamera = 3;
            }
            SetCameraDirection();

            rotationAngle = rotationAngle - 90;
            rotatePlayer();

        }
    }
}

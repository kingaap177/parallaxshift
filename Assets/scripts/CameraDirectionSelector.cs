using System.Threading.Tasks;
using UnityEngine;

public class CameraDirectionSelector : MonoBehaviour
{
    public Camera NorthCamera;
    public Camera EastCamera;
    public Camera SouthCamera;
    public Camera WestCamera;
    private int currentCamera = 0;
    private int rotationAngle = 0;

    public int DelayBetweenFlips = 2000;
    private int FlipTimer = 0;

    private int rotationDegreePerFlip = 90;

    private bool isRotating = false;

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

    void Start()
    {
        SetCameraDirection();
    }

    void RotatePlayer()
    {
        if (rotationAngle >= 360)
        {
            rotationAngle -= 360;
        }
        if (rotationAngle < 0)
        {
            rotationAngle += 360;
        }

        transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
    }

    void Update()
    {
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentCamera = (currentCamera + 1) % 4;
            _ = RotateCameraAsync(-rotationDegreePerFlip);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentCamera = (currentCamera - 1 + 4) % 4;
            _ = RotateCameraAsync(rotationDegreePerFlip);
        }
    }

    async Task DelayBetweenswitchingCameras()
    {
        while (FlipTimer < DelayBetweenFlips)
        {
            FlipTimer += 100;
            await Task.Delay(100);
        }
        FlipTimer = 0;
    }

    async Task RotateCameraAsync(int rotationDelta)
    {
        isRotating = true;

        SetCameraDirection();

        rotationAngle += rotationDelta;
        RotatePlayer();

        await DelayBetweenswitchingCameras();

        isRotating = false;
    }
}

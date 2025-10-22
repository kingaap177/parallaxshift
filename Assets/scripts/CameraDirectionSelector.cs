using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unified.UniversalBlur.Runtime;
using System.Collections;

public class CameraDirectionSelector : MonoBehaviour
{
    private UniversalRenderPipelineAsset urpAsset;

    public Camera NorthCamera;
    public Camera EastCamera;
    public Camera SouthCamera;
    public Camera WestCamera;
    public int currentCamera = 0;
    private int rotationAngle = 0;

    private int currentRotation = 0;

    public UnityEngine.UI.Image BlurImage; // Add this field to reference the BlurImage

    public int DelayBetweenFlips = 2000;
    //private int FlipTimer = 0;

    private int rotationDegreePerFlip = 90;

    private bool isRotating = false;

    void DisableAllCameras()
    {
        NorthCamera.enabled = false;
        EastCamera.enabled = false;
        SouthCamera.enabled = false;
        WestCamera.enabled = false;
    }

    void SetBlurImageActive(bool isActive)
    {
        if (BlurImage != null)
        {
            BlurImage.gameObject.SetActive(isActive);
        }
    }

    async Task SetCameraDirection()
    {
        SetBlurImageActive(true);

        await DelayBetweenswitchingCameras();

        DisableAllCameras();

        switch (currentCamera)
        {
            case 0:
                NorthCamera.enabled = true; 
                break;
            case 1:
                EastCamera.enabled = true;
                break;
            case 2:
                SouthCamera.enabled = true;
                break;
            case 3:
                WestCamera.enabled = true;
                break;
        }

        await DelayBetweenswitchingCameras();

        SetBlurImageActive(false);
    }

    void Start()
    {
        DisableAllCameras();
        NorthCamera.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    async Task RotatePlayer(int rotationAmount)
    {
        rotationAngle = Mathf.RoundToInt(transform.rotation.eulerAngles.y) + rotationAmount;

        if (rotationAngle >= 360)
        {
            rotationAngle -= 360;
        }
        if (rotationAngle < 0)
        {
            rotationAngle += 360;
        }

        await DelayBetweenswitchingCameras();

        transform.rotation = Quaternion.Euler(0, rotationAngle, 0);

        await DelayBetweenswitchingCameras();
    }

    void Update()
    {
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentCamera = currentCamera + 1;
            if (currentCamera > 3)
            {
                currentCamera = 0;
            }
            SetCameraDirection();

            RotatePlayer(90);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentCamera = currentCamera - 1;
            if (currentCamera < 0)
            {
                currentCamera = 3;
            }
            SetCameraDirection();

            RotatePlayer(-90);
        }
    }

    async Task DelayBetweenswitchingCameras()
    {
        await Task.Delay(DelayBetweenFlips / 2);
    }
}

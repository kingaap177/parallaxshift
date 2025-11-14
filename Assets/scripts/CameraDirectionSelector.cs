using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unified.UniversalBlur.Runtime;
using System.Collections;

public class CameraDirectionSelector : MonoBehaviour
{
    public Camera MainCamera;

    // Four waypoints (North, East, South, West) set in the inspector.
    // Each waypoint is a Transform specifying desired camera world position and rotation.
    public Transform[] CameraPoints = new Transform[4];

    // Duration of the transition in seconds
    public float TransitionDuration = 1.0f;

    // Optional blur UI Image to show during transitions
    public UnityEngine.UI.Image BlurImage;


    public int currentCamera = 0;

    public bool isTransitioning = false;

    public float distanceFromPlayer = 5.0f;
    public float heightDifferencesFromPlayer;

    void Start()
    {
        // Validate CameraPoints length
        if (CameraPoints == null || CameraPoints.Length < 4)
        {
            Debug.LogWarning("CameraDirectionSelector: Please assign 4 CameraPoints (North, East, South, West).");
        }

        if (MainCamera == null)
        {
            // Fallback to Camera.main
            MainCamera = Camera.main;
        }

        // Initialize camera position/rotation to currentCamera waypoint if available
        if (MainCamera != null && CameraPoints != null && CameraPoints.Length > 0 && CameraPoints[currentCamera] != null)
        {
            MainCamera.transform.position = CameraPoints[currentCamera].position;
            MainCamera.transform.rotation = CameraPoints[currentCamera].rotation;
        }
    }

    void SetBlurImageActive(bool isActive)
    {
        if (BlurImage != null)
        {
            BlurImage.gameObject.SetActive(isActive);
        }
    }

    void Update()
    {
        cameraFollowPlayer();

        float xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || xInput != 0)
        {
            return;
        }

        if (isTransitioning) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentCamera = (currentCamera + 1) % 4;
            StartCoroutine(TransitionTo(currentCamera, +90));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentCamera = (currentCamera - 1);
            if (currentCamera < 0) currentCamera = 3;
            StartCoroutine(TransitionTo(currentCamera, -90));
        }
    }

    IEnumerator TransitionTo(int targetIndex, int playerRotationDelta)
    {
        if (isTransitioning) yield break;
        if (MainCamera == null || CameraPoints == null || CameraPoints.Length < 4) yield break;
        if (CameraPoints[targetIndex] == null) yield break;

        isTransitioning = true;
        SetBlurImageActive(true);

        // Start both coroutines in parallel
        IEnumerator moveCam = MoveCameraSmooth(CameraPoints[targetIndex], TransitionDuration);
        IEnumerator rotatePlayer = RotatePlayerSmooth(playerRotationDelta, TransitionDuration);

        // Start both
        StartCoroutine(moveCam);
        StartCoroutine(rotatePlayer);

        // Wait for duration
        float elapsed = 0f;
        while (elapsed < TransitionDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final transform snap to avoid tiny inaccuracies
        MainCamera.transform.position = CameraPoints[targetIndex].position;
        MainCamera.transform.rotation = CameraPoints[targetIndex].rotation;

        SetBlurImageActive(false);
        isTransitioning = false;
    }

    IEnumerator MoveCameraSmooth(Transform target, float duration)
    {
        if (MainCamera == null || target == null)
            yield break;

        Transform camT = MainCamera.transform;

        Vector3 startPos = camT.position;
        Quaternion startRot = camT.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float factor = Mathf.Clamp01(t / duration);
            camT.position = Vector3.Lerp(startPos, endPos, factor);
            camT.rotation = Quaternion.Slerp(startRot, endRot, factor);
            yield return null;
        }

        camT.position = endPos;
        camT.rotation = endRot;
    }

    IEnumerator RotatePlayerSmooth(int deltaYDegrees, float duration)
    {
        // Rotate the GameObject this script is attached to (presumably the player).
        float startY = transform.eulerAngles.y;
        float targetY = startY + deltaYDegrees;

        // Normalize angles to avoid discontinuities
        startY = NormalizeAngle(startY);
        targetY = NormalizeAngle(targetY);

        // If crossing -180..180 boundary, ensure shortest path by adjusting target
        float delta = Mathf.DeltaAngle(startY, targetY); // shortest signed delta
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float factor = Mathf.Clamp01(t / duration);
            float currentY = startY + delta * factor;
            transform.rotation = Quaternion.Euler(0f, currentY, 0f);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, targetY, 0f);
    }

    float NormalizeAngle(float a)
    {
        while (a < 0f) a += 360f;
        while (a >= 360f) a -= 360f;
        return a;
    }

    void cameraFollowPlayer()
    {
        switch (currentCamera)
        {
            case 3:
                MainCamera.transform.position = new Vector3(transform.position.x + distanceFromPlayer, transform.position.y + heightDifferencesFromPlayer, transform.position.z);
                break;
            case 0:
                MainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + heightDifferencesFromPlayer, transform.position.z - distanceFromPlayer);
                break;
            case 1:
                MainCamera.transform.position = new Vector3(transform.position.x - distanceFromPlayer, transform.position.y + heightDifferencesFromPlayer, transform.position.z);
                break;
            case 2:
                MainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + heightDifferencesFromPlayer, transform.position.z + distanceFromPlayer);
                break;
        };
    }
}

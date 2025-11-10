using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CameraDirectionSelector CameraDirectionSelector;

    private Animator anim;
    public AudioSource footsteps;
    public AudioSource jumpSound;
    public Rigidbody rb;

    public float moveSpeed = 3.5f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 0.3f;
    private bool isGrounded;
    public LayerMask whatIsGround;

    private float xInput;
    private Vector3 moveDirection = Vector3.zero;

    private int CurrentDirection
    {
        get { return CameraDirectionSelector.currentCamera; }
    }

    private bool facingRight = true;

    private CameraDirectionSelector cameraSelector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        cameraSelector = GetComponent<CameraDirectionSelector>();
    }

    private void Update()
    {
        CollissionHandler();
        InputHandler();
        AnimationHandler();
        FlipHandler();
    }

    private void FixedUpdate()
    {
        MovementHandler();
    }

    private void AnimationHandler()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        anim.SetFloat("xVelocity", localVelocity.x);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

    }

    private void InputHandler()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        HandleFootstepSound();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

        private void HandleFootstepSound()
    {
        bool isWalking = Mathf.Abs(xInput) > 0.1f && isGrounded;

        if (isWalking)
        {
            if (!footsteps.isPlaying)
                footsteps.Play();
        }
        else
        {
            if (footsteps.isPlaying)
                footsteps.Stop();
        }
    }


    private void MovementHandler()
    {
        Transform activeCam = GetActiveCameraTransform();
        if (activeCam == null)
            return;

        Vector3 cameraRight = activeCam.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        moveDirection = cameraRight * xInput * moveSpeed;

        Vector3 velocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        rb.linearVelocity = velocity;

    }

    private Transform GetActiveCameraTransform()
    {
        if (cameraSelector.NorthCamera.enabled) return cameraSelector.NorthCamera.transform;
        if (cameraSelector.EastCamera.enabled) return cameraSelector.EastCamera.transform;
        if (cameraSelector.SouthCamera.enabled) return cameraSelector.SouthCamera.transform;
        if (cameraSelector.WestCamera.enabled) return cameraSelector.WestCamera.transform;
        return null;
    }

    private void Jump()
    {
        if (!isGrounded) return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        jumpSound.Play();
    }


    private void CollissionHandler()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    private void FlipHandler()
    {
        if (xInput > 0 && !facingRight)
            Flip();
        else if (xInput < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }
}

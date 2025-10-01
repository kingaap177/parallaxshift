using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CameraDirectionSelector CameraDirectionSelector;

    private Animator anim;

    public Rigidbody rb;

    public float moveSpeed = 3.5f;
    public float jumpForce = 10f;
    public float groundCheckDistance;
    private bool isGrounded;
    public LayerMask whatIsGround;

    private float xInput;

    private int CurrentDirection
    {
        get { return CameraDirectionSelector.currentCamera; }
    }

    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CollissionHandler();
        InputHandler();
        MovementHandler();
        AnimationHandler();
        FlipHandler();
    }

    private void AnimationHandler()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void InputHandler()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void MovementHandler()
    {
        Vector3 moveDir = Vector3.zero;

        switch (CurrentDirection)
        {
            case 0: // North
                moveDir = new Vector3(xInput, 0, 0);
                break;
            case 1: // East
                moveDir = new Vector3(0, 0, -xInput);
                break;
            case 2: // South
                moveDir = new Vector3(-xInput, 0, 0);
                break;
            case 3: // West
                moveDir = new Vector3(0, 0, xInput);
                break;
        }

        moveDir = moveDir.normalized * moveSpeed;

        rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);   
        }
    }

    void CollissionHandler()
    {
        isGrounded = Physics.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
    }

    void FlipHandler()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight == true)
        {
            Flip();
        }
    }
    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
}

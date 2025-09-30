using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    public Rigidbody rb;
    public float moveSpeed = 3.5f;
    public float jumpForce = 10f;
    public float groundCheckDistance;
    private bool isGrounded;
    public LayerMask whatIsGround;
    private float xInput;

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
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
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

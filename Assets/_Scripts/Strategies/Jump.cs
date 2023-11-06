using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump : MonoBehaviour, ISpecial
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float maxJumpHeight = 2.0f;
    [SerializeField] private float fallSpeed = 2.0f;
    [SerializeField] private float jumpDelay = 1.0f;
    [SerializeField] private float jumpCooldown = 1.0f;
    [SerializeField] private float fallThreshold = -0.1f;

    private Rigidbody rb;
    private Animator animator;

    private float startHeight;
    private float jumpDelayTimer;
    private float jumpCooldownTimer;

    private bool isGrounded;
    private bool canJump = true;
    private bool isJumping = false;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        // Check if the player is falling
        if (rb.velocity.y < fallThreshold)
        {
            // Apply fall speed
            rb.AddForce(Vector3.down * fallSpeed);

            if (!isFalling)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("Land", false);
                animator.SetBool("Fall", true);
                isFalling = true;
            }
        }

        if (transform.position.y - startHeight > maxJumpHeight)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // Apply fall speed
            rb.AddForce(Vector3.down * fallSpeed);
        }

        // Grounded 
        if (isGrounded)
        {
            if (isFalling)
            {
                animator.SetBool("Fall", false);
                animator.SetBool("Land", true);
                isFalling = false;
            }

            // Cooldown control
            if (!canJump && Time.time - jumpCooldownTimer >= jumpCooldown)
            {
                canJump = true;
            }

            // Delayed jump
            if (isJumping && Time.time - jumpDelayTimer >= jumpDelay)
            {
                startHeight = transform.position.y;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = false;
            }
        }
    }

    void ISpecial.Special()
    {
        // Jump control
        if (canJump)
        {
            animator.SetBool("Jump", true);

            jumpCooldownTimer = Time.time;
            jumpDelayTimer = Time.time;

            canJump = false;
            isJumping = true;
        }
    }

    void ISpecial.Enable(bool enabled)
    {
        this.enabled = enabled;
    }
}

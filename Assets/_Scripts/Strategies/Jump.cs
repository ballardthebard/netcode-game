using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, IJump
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float fallSpeed = 2.0f;
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float jumpDelay = 1.0f;
    [SerializeField] private float jumpCooldown = 1.0f;
    [SerializeField] private float fallThreshold = -0.1f;

    private Rigidbody rb;
    private Animator animator;

    private float jumpDelayTimer;
    private float jumpCooldownTimer;

    private bool isGrounded;
    private bool canJump = true;
    private bool isJumping = false;
    private bool isFalling = false;

    public Rigidbody Rigidbody { set => rb = value; }
    public Animator Animator { set => animator = value; }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        // Check if the player is falling
        if (rb.velocity.y < fallThreshold)
        {
            // Apply force for movement
            rb.AddForce(Vector3.down * fallSpeed);

            if (!isFalling)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("Land", false);
                animator.SetBool("Fall", true);
                isFalling = true;
            }
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
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = false;
            }
        }
    }

    void IJump.Jump()
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
}

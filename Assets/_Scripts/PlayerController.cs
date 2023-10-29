using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float maxVelocity = 5.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
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

    private void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Update()
    {
        if (!IsOwner || !Application.isFocused) return;

        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput);

        // Normalize the movement direction if not zero
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Apply force for movement
        rb.AddForce(movement * moveSpeed);

        // Limit the maximum velocity
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        // Check if the player is falling
        if (rb.velocity.y < fallThreshold)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Land", false);
            animator.SetBool("Fall", true);
            isFalling = true;
        }

        // Grounded 
        if (isGrounded)
        {
            if (isFalling)
            {
                animator.SetBool("Fall", false);
                animator.SetBool("Land", true);
            }

            // Cooldown control
            if (!canJump && Time.time - jumpCooldownTimer >= jumpCooldown)
            {
                canJump = true;
            }

            // Jump control
            if (Input.GetButtonDown("Jump") && canJump)
            {
                animator.SetBool("Jump", true);

                jumpCooldownTimer = Time.time;
                jumpDelayTimer = Time.time;

                canJump = false;
                isJumping = true;
            }

            // Delayed jump
            if (isJumping && Time.time - jumpDelayTimer >= jumpDelay)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = false;
            }
        }
    }
}

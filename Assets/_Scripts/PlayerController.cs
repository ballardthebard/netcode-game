using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private IMovement movement;
    private IJump jump;
    private IGrab grab;

    private Rigidbody rb;
    private Animator animator;

    private void Initialize()
    {
        // Get components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Get strategies
        movement = GetComponentInChildren<IMovement>();
        jump = GetComponentInChildren<IJump>();
        grab = GetComponentInChildren<IGrab>();

        // Set strategies properties
        movement.Rigidbody = rb;
        jump.Rigidbody = rb;
        jump.Animator = animator;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Update()
    {
        if (!IsOwner || !Application.isFocused) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 movementAxis = new Vector3(horizontalInput, 0.0f, verticalInput);

        movement.Move(movementAxis);
        movement.Rotate(movementAxis);

        if (Input.GetButtonDown("Jump"))
        {
            jump.Jump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            grab.IsGrabbing = true;
            animator.SetBool("Grab", true);
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            grab.IsGrabbing = false;
            animator.SetBool("Grab", false);
        }
    }
}

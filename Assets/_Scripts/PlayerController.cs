using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Transform eyesTransform;
    [SerializeField] private Grabbable defaultBody;

    // Variables
    private IMovement movement;
    private ISpecial special;
    private IGrab grab;

    private Animator animator;

    private bool hasBody;

    // Properties
    public IMovement Movement { set => movement = value; }
    public ISpecial Special { set => special = value; }

    private void Initialize()
    {
        // Get components
        animator = GetComponentInChildren<Animator>();

        // Get strategies
        movement = GetComponentInChildren<IMovement>();
        special = GetComponentInChildren<ISpecial>();
        grab = GetComponentInChildren<IGrab>();

        // Set strategies properties
        grab.PlayerController = this;
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
        Vector3 movementAxis = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;
        movement.Move(movementAxis);
        movement.Rotate(movementAxis);

        if (Input.GetButtonDown("Jump"))
        {
            special.Special();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            grab.IsGrabbing = true;
            animator.SetBool("Grab", true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grab.IsGrabbing = false;
            grab.Discard();
            animator.SetBool("Grab", false);
        }
    }

    public void ChangeBodies(Grabbable bodie)
    {
        // Disable default bodie and set bool as has body
        hasBody = true;
        defaultBody.gameObject.SetActive(false);

        bodie.Rigidbody.freezeRotation = true;
        bodie.transform.localPosition = defaultBody.transform.localPosition;
        bodie.transform.localRotation = defaultBody.transform.rotation;
        eyesTransform.localPosition = bodie.EyesTransform.localPosition;

        // Set new strategies
        movement = bodie.Movement;
        special = bodie.Special;
        special.Enable(true);
    }
}

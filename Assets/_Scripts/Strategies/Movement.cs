using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IMovement
{
    // Variables
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 movementAxis)
    {
        Debug.Log(movementAxis);
        // Apply force for movement
        rb.AddForce(movementAxis * moveSpeed, ForceMode.Acceleration);

        // Limit the maximum velocity
        if (rb.velocity.magnitude > maxVelocity)
        {
            Vector3 velocity = rb.velocity.normalized * maxVelocity;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }
    }

    public void Rotate(Vector3 rotationAxis)
    {
        // Normalize the movement direction if not zero
        if (rotationAxis != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotationAxis);
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IMovement
{
    // Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;

    public Rigidbody Rigidbody { set => rb = value; }

    public void Move(Vector3 movementAxis)
    {
        // Apply force for movement
        rb.AddForce(movementAxis * moveSpeed);

        // Limit the maximum velocity
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
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

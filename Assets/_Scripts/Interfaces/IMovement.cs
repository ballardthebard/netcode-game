using UnityEngine;

public interface IMovement
{
    Rigidbody Rigidbody { set; }

    void Move(Vector3 movementAxis);
    void Rotate(Vector3 rotationAxis);
}

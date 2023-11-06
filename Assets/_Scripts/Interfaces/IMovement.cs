using UnityEngine;

public interface IMovement
{
    void Move(Vector3 movementAxis);
    void Rotate(Vector3 rotationAxis);
}

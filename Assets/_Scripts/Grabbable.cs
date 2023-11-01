using Unity.Netcode;
using UnityEngine;

public class Grabbable : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float maxVelocity = 5.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
}

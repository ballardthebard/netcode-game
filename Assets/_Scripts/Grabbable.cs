using Unity.Netcode;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    // Variables
    [SerializeField] private float grabTrashold = 0.75f;
    [SerializeField] private Transform eyesTransform;

    private Rigidbody rb;
    private IMovement movement;
    private ISpecial special;

    // Properties
    public float GrabTrashold { get => grabTrashold; }
    public Rigidbody Rigidbody { get => rb; }
    public Transform EyesTransform { get => eyesTransform; }
    public IMovement Movement { get => movement; }
    public ISpecial Special { get => special; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<IMovement>();
        special = GetComponent<ISpecial>();
    }
}
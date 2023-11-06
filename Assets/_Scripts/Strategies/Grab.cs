using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour, IGrab
{
    [SerializeField] private float grabSpeed;
    [SerializeField] private float grabTrashold = 0.75f;

    private bool isGrabbing;
    private Dictionary<int, Grabbable> grabbables;
    private PlayerController playerController;

    // Properties
    public bool IsGrabbing { set => isGrabbing = value; }
    public PlayerController PlayerController { set => playerController = value; }

    private void Start()
    {
        grabbables = new Dictionary<int, Grabbable>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Grabbable" || !isGrabbing) return;

        int key = other.GetHashCode();

        if (!grabbables.ContainsKey(key))
        {
            grabbables.Add(key, other.GetComponent<Grabbable>());
        }

        Vector3 direction = transform.position - grabbables[key].transform.position;
        grabbables[key].Rigidbody.AddForce(direction * grabSpeed);

        if (Vector3.Distance(grabbables[key].transform.position, transform.position) < grabTrashold + grabbables[key].GrabTrashold)
        {
            playerController.ChangeBodies(grabbables[key]);
            Debug.Log("Changed Bodie");
        }
    }

    void IGrab.Grab()
    {
        isGrabbing = true;
    }

    public void Discard()
    {
    }
}

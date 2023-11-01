using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour, IGrab
{
    [SerializeField] private float grabSpeed;
    [SerializeField] private float grabTrashold = 0.5f;

    private bool isGrabbing;
    private Rigidbody grabbableRigidbody;

    public bool IsGrabbing { set => isGrabbing = value; }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Grabbable" || !isGrabbing) return;

        if (grabbableRigidbody == null)
        {
            grabbableRigidbody = other.GetComponent<Rigidbody>();
        }

        Vector3 direction = transform.position - other.transform.position;
        grabbableRigidbody.AddForce(direction * grabSpeed);

        print(Vector3.Distance(other.transform.position, transform.position));

        if (Vector3.Distance(other.transform.position, transform.position) < grabTrashold)
        {
            other.gameObject.SetActive(false);
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

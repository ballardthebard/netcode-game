using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : Grabbable
{
    [SerializeField] private float fallSpeed = 2.0f;
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float jumpDelay = 1.0f;
    [SerializeField] private float jumpCooldown = 1.0f;
}

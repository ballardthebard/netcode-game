using UnityEngine;

public interface IJump
{
    Rigidbody Rigidbody { set; }
    Animator Animator { set; }
    void Jump();
}

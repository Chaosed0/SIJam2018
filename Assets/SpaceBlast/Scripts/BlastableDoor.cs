using UnityEngine;
using System.Collections;

public class BlastableDoor : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnBlast()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}

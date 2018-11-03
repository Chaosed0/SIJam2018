using UnityEngine;
using System.Collections;

public class ViewModelFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject follow;

    [SerializeField]
    private float rotationLag = 0.2f;

    float rotationVelocity = 0f;
    Quaternion rotationOffset = Quaternion.identity;

    private void Awake()
    {
        rotationOffset = transform.localRotation;
    }

    private void Start()
    {
        transform.parent = null;
    }

    void FixedUpdate()
    {
        transform.position = follow.transform.position;
        float newAngle = Mathf.SmoothDampAngle(Quaternion.Angle(transform.rotation, follow.transform.rotation), 0f, ref rotationVelocity, rotationLag);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, follow.transform.rotation, newAngle);
    }
}

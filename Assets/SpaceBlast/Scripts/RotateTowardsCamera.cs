using UnityEngine;
using System.Collections;

public class RotateTowardsCamera : MonoBehaviour
{
    [SerializeField]
    private float smoothTime = 0.5f;

    float vel;

    void Update()
    {
        Vector3 target = (Camera.main.transform.position - transform.position).normalized;
        target.y = Mathf.Clamp(target.y, -0.1f, 0.1f);
        target = target.normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target), Time.deltaTime);
        /*
        float angle = Vector3.Angle(forward, target);
        float rotation = Mathf.SmoothDampAngle(angle, 0f, ref vel, smoothTime);

        Vector3 axis = Vector3.Cross(forward, target);
        transform.rotation = transform.rotation * Quaternion.AngleAxis(rotation, axis);
        */
    }
}

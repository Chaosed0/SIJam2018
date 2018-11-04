using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float acceleration = 1f;

    [SerializeField]
    private float bigAcceleration = 5.0f;

    [SerializeField]
    private float maxVelocity = 1000f;

    [SerializeField]
    private Transform facingSource;

    private Rigidbody rb;

    private Vector3 movement = Vector3.zero;
    private float yaw = 0.0f;
    private bool bigThrust = false;

    [System.Serializable]
    public class AccelerationChangedEvent : UnityEvent<Vector3> { }
    public AccelerationChangedEvent OnAccelerationChanged = new AccelerationChangedEvent();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetThrust(Vector3 movement, float yaw)
    {
        this.movement = movement.normalized;
        transform.rotation = transform.rotation * Quaternion.AngleAxis(yaw, Vector3.up);

        OnAccelerationChanged.Invoke(this.movement);
    }

    public void DoBigThrust()
    {
        bigThrust = true;
    }
	
	void FixedUpdate ()
    {
        rb.AddForce(acceleration * movement.y * Vector3.up +
            acceleration * movement.z * facingSource.forward +
            acceleration * movement.x * facingSource.right,
            ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

        if (bigThrust)
        {
            rb.AddForce(-bigAcceleration * facingSource.forward, ForceMode.Acceleration);
        }

        this.yaw = 0f;
        this.movement = Vector3.zero;
        this.bigThrust = false;
	}
}

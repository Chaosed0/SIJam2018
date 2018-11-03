using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    private float pitch = 0.0f;

    public void Pitch(float pitch)
    {
        this.pitch = Mathf.Clamp(this.pitch + pitch, -89, 89);

        transform.localRotation = Quaternion.AngleAxis(this.pitch, Vector3.right);
    }
}

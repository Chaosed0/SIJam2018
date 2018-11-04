using UnityEngine;
using System.Collections;
using Assets.Pixelation.Scripts;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private GameObject heatImagingCam;

    [SerializeField]
    private Blue blue;

    [SerializeField]
    private Pixelation normalPixelation;

    private float pitch = 0.0f;
    private bool ads = false;

    public void Pitch(float pitch)
    {
        this.pitch = Mathf.Clamp(this.pitch + pitch, -89, 89);

        transform.localRotation = Quaternion.AngleAxis(this.pitch, Vector3.right);
    }

    public void SetAds(bool ads)
    {
        if (this.ads != ads)
        {
            this.ads = ads;

            heatImagingCam.SetActive(ads);
            blue.enabled = ads;
            normalPixelation.enabled = !ads;
        }
    }
}

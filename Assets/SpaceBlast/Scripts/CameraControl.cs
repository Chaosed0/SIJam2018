using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Health health;

    [SerializeField]
    private GameObject heatImagingCam;

    [SerializeField]
    private Blue blue;

    [SerializeField]
    private float hitRollMin = 10.0f;

    [SerializeField]
    private float hitRollMax = 15.0f;

    [SerializeField]
    private float rollRecovery = 10f;

    [SerializeField]
    private Camera viewModelCamera;

    private float pitch = 0.0f;
    private float roll = 0.0f;
    private float hitTime = 0.0f;
    private bool ads = false;

    [HideInInspector]
    public bool scopeEnabled = false;

    private void Awake()
    {
        if (health != null)
        {
            health.OnHealthChanged.AddListener(OnHealthChanged);
            health.OnDead.AddListener(OnDeath);
        }
    }

    public void Pitch(float pitch)
    {
        this.pitch = Mathf.Clamp(this.pitch + pitch, -89, 89);
    }

    void Update()
    {
        this.roll = Mathf.Sign(this.roll) * Mathf.Max(0f, Mathf.Abs(roll) - rollRecovery * Time.deltaTime);
        transform.localRotation = Quaternion.AngleAxis(this.pitch, Vector3.right) * Quaternion.AngleAxis(this.roll, Vector3.forward);
    }

    public void SetAds(bool ads)
    {
        if (!scopeEnabled)
        {
            return;
        }

        if (this.ads != ads)
        {
            this.ads = ads;

            heatImagingCam.SetActive(ads);
            blue.enabled = ads;
        }
    }

    public void OnHealthChanged(float newHealth, float oldHealth)
    {
        if (newHealth < oldHealth)
        {
            this.roll = Random.Range(hitRollMin, hitRollMax);
        }
    }

    public void OnDeath()
    {
        viewModelCamera.enabled = false;
    }
}

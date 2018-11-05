using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject gunModel;

    [SerializeField]
    private GameObject scopeModel;

    [SerializeField]
    private GameObject killModel;

    [SerializeField]
    private Gun gun;

    [SerializeField]
    private CameraControl cameraControl;

	[SerializeField]
	private PlayableDirector win;

	[SerializeField]
	private PlayableDirector lose;

    public bool gunEnabled = false;
    public bool scopeEnabled = false;
    public bool killEnabled = false;

    public UnityEvent OnGunEnabled = new UnityEvent();
    public UnityEvent OnScopeEnabled = new UnityEvent();
    public UnityEvent OnKillEnabled = new UnityEvent();

    private void Awake()
    {
        SetGunEnabled(gunEnabled);
        SetScopeEnabled(scopeEnabled);
        SetKillEnabled(killEnabled);
    }

    public void SetGunEnabled(bool enabled)
    {
        gunModel.SetActive(enabled);
        gun.canFire = enabled;
        gunEnabled = enabled;

        if (enabled)
        {
            OnGunEnabled.Invoke();
        }
    }

    public void SetScopeEnabled(bool enabled)
    {
        scopeModel.SetActive(enabled);
        cameraControl.scopeEnabled = enabled;
        scopeEnabled = enabled;

        if (enabled)
        {
            OnScopeEnabled.Invoke();
        }
    }

    public void SetKillEnabled(bool enabled)
    {
        killModel.SetActive(enabled);
        gun.dealDamage = enabled;
        killEnabled = enabled;

        if (enabled)
        {
            OnKillEnabled.Invoke();
        }
    }

	public void Win()
	{
		win.Play ();
	}

	public void Lose()
	{
		lose.Play();
	}
}

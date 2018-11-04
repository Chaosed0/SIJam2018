using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerSound : MonoBehaviour
{
    [SerializeField]
    private Gun gun;

    [SerializeField]
    private Movement movement;

    [SerializeField]
    private SoundLibrary soundLibrary;

    [SerializeField]
    private AudioSourceGroup gunBlastAudioSource;

    [SerializeField]
    private AudioSource moveBlastJetAudioSource;

    [SerializeField]
    private AudioSource moveContinuousJetAudioSource;

    private Coroutine jetFadeCoroutine;

    private Vector3 oldAcceleration = Vector3.zero;
    private bool accelerationChanged = false;

    private void Awake()
    {
        gun.OnFire.AddListener(OnGunFire);
        movement.OnAccelerationChanged.AddListener(OnAccelerationChanged);
    }

    private void OnGunFire()
    {
        if (gun.dealDamage)
        {
            Play(gunBlastAudioSource.Get(), soundLibrary.GetSound("HeavyGunBlast"));
        }
        else
        {
            Play(gunBlastAudioSource.Get(), soundLibrary.GetSound("GunBlast"));
        }
    }

    private void OnAccelerationChanged(Vector3 newAcceleration)
    {
        accelerationChanged = true;

        if (newAcceleration.magnitude - oldAcceleration.magnitude > 0.5f)
        {
            Play(moveBlastJetAudioSource, soundLibrary.GetSound("JetBlast"));
        }

        if (newAcceleration.sqrMagnitude > Mathf.Epsilon && oldAcceleration.sqrMagnitude < Mathf.Epsilon)
        {
            Debug.Log("Start");
            moveContinuousJetAudioSource.volume = newAcceleration.magnitude * 0.4f;
            moveContinuousJetAudioSource.Play();
        }
        else if (newAcceleration.sqrMagnitude < Mathf.Epsilon && oldAcceleration.sqrMagnitude > Mathf.Epsilon)
        {
            Debug.Log("Stop");
            StartFade(moveContinuousJetAudioSource, 0.25f);
        }

        oldAcceleration = newAcceleration;
    }

    private void LateUpdate()
    {
        if (!accelerationChanged)
        {
            OnAccelerationChanged(Vector3.zero);
        }

        accelerationChanged = false;
    }

    void StartFade(AudioSource source, float time)
    {
        if (jetFadeCoroutine != null)
        {
            StopCoroutine(jetFadeCoroutine);
        }
        jetFadeCoroutine = StartCoroutine(FadeCoroutine(source, time));
    }

    IEnumerator FadeCoroutine(AudioSource source, float time)
    {
        float startTime = Time.time;
        float initialVolume = source.volume;
        while (Time.time - startTime < time)
        {
            float lerp = (Time.time - startTime) / time;
            source.volume = initialVolume * lerp;
            yield return null;
        }
        moveContinuousJetAudioSource.Stop();
        jetFadeCoroutine = null;
    }

    private void Play(AudioSource source, AudioClip clip)
    {
        source.Stop();
        source.clip = clip;
        source.Play();
    }
}

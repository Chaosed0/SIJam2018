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
    private Player player;

    [SerializeField]
    private SoundLibrary soundLibrary;

    [SerializeField]
    private AudioSourceGroup gunBlastAudioSource;

    [SerializeField]
    private AudioSource moveBlastJetAudioSource;

    [SerializeField]
    private AudioSource moveContinuousJetAudioSource;

    [SerializeField]
    private AudioSource farawayScreechPrefab;

    [SerializeField]
    private float farawayRadius = 50.0f;

    [SerializeField]
    private float ambientScreechMin = 15.0f;

    [SerializeField]
    private float ambientScreechMax = 30.0f;

    [SerializeField]
    private bool ambientScreechOn = true;

    private Coroutine jetFadeCoroutine;

    private Vector3 oldAcceleration = Vector3.zero;
    private bool accelerationChanged = false;

    private float lastAmbientScreechTime = 0f;
    private float nextAmbientScreechTime = 0f;

    private SoundLibrary.Sound heavyGunBlast;
    private SoundLibrary.Sound pickup;
    private SoundLibrary.Sound gunBlast;
    private SoundLibrary.Sound jetBlast;
    private SoundLibrary.Sound enemyFarAlert;

    private void Awake()
    {
        gun.OnFire.AddListener(OnGunFire);
        movement.OnAccelerationChanged.AddListener(OnAccelerationChanged);
        player.OnGunEnabled.AddListener(OnPickup);
        player.OnScopeEnabled.AddListener(OnPickup);
        player.OnKillEnabled.AddListener(OnPickup);

        lastAmbientScreechTime = Time.time;
        nextAmbientScreechTime = Random.Range(ambientScreechMin, ambientScreechMax);

        heavyGunBlast = soundLibrary.GetSound("HeavyGunBlast");
        pickup = soundLibrary.GetSound("Pickup");
        gunBlast = soundLibrary.GetSound("GunBlast");
        jetBlast = soundLibrary.GetSound("JetBlast");
        enemyFarAlert = soundLibrary.GetSound("EnemyFarAlert");
    }

    private void OnPickup()
    {
        Play(gunBlastAudioSource.Get(), pickup.GetRandom());
    }

    private void OnGunFire()
    {
        if (gun.dealDamage)
        {
            Play(gunBlastAudioSource.Get(), heavyGunBlast.GetRandom());
        }
        else
        {
            Play(gunBlastAudioSource.Get(), gunBlast.GetRandom());
        }
    }

    private void OnAccelerationChanged(Vector3 newAcceleration)
    {
        accelerationChanged = true;

        if (newAcceleration.magnitude - oldAcceleration.magnitude > 0.5f)
        {
            Play(moveBlastJetAudioSource, jetBlast.GetRandom());
        }

        if (newAcceleration.sqrMagnitude > Mathf.Epsilon && oldAcceleration.sqrMagnitude < Mathf.Epsilon)
        {
            moveContinuousJetAudioSource.volume = newAcceleration.magnitude * 0.25f;
            moveContinuousJetAudioSource.Play();
        }
        else if (newAcceleration.sqrMagnitude < Mathf.Epsilon && oldAcceleration.sqrMagnitude > Mathf.Epsilon)
        {
            StartFade(moveContinuousJetAudioSource, 0.25f);
        }

        oldAcceleration = newAcceleration;
    }

    private void Update()
    {
        float interval = Time.time - lastAmbientScreechTime;
        if (interval > nextAmbientScreechTime)
        {
            Vector3 position = Random.rotationUniform * (farawayRadius * Vector3.forward);
            AudioSource source = Instantiate(farawayScreechPrefab, position, Quaternion.identity);
            source.clip = enemyFarAlert.GetRandom();

            lastAmbientScreechTime = Time.time;
            nextAmbientScreechTime = Random.Range(ambientScreechMin, ambientScreechMax);
        }
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

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Attack))]
public class EnemySound : MonoBehaviour
{
    [SerializeField]
    private AudioSourceGroup enemyAudioSourceGroup;

    [SerializeField]
    private SoundLibrary soundLibrary;

    private SoundLibrary.Sound attackSound;
    private SoundLibrary.Sound startSound;
    private SoundLibrary.Sound fleeSound;

    private void Awake()
    {
        Enemy enemy = GetComponent<Enemy>();
        enemy.OnStateChanged.AddListener(OnStateChanged);

        Attack attack = GetComponent<Attack>();
        attack.OnAttack.AddListener(OnAttack);

        attackSound = soundLibrary.GetSound("EnemyAttack");
        startSound = soundLibrary.GetSound("EnemyCloseAlert");
        fleeSound = soundLibrary.GetSound("EnemyCloseAlert");
    }

    void Start()
    {
    }

    void OnStateChanged(Enemy.State state)
    {
        if (state == Enemy.State.Following)
        {
            Play(enemyAudioSourceGroup.Get(), startSound.GetRandom());
        }
        else if (state == Enemy.State.Fleeing)
        {
            Play(enemyAudioSourceGroup.Get(), fleeSound.GetRandom());
        }
    }

    void OnAttack()
    {
        Play(enemyAudioSourceGroup.Get(), attackSound.GetRandom());
    }

    private void Play(AudioSource source, AudioClip clip)
    {
        source.Stop();
        source.clip = clip;
        source.Play();
    }
}

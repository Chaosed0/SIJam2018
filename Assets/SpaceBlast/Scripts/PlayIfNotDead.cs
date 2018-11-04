using UnityEngine;
using System.Collections;

public class PlayIfNotDead : MonoBehaviour
{
    [SerializeField]
    private Health health;

    [SerializeField]
    private AudioSource audioSource;

    private bool played = false;

    public void Play()
    {
        if (played)
        {
            return;
        }

        played = true;
        if (health.GetHealth() > 0f)
        {
            audioSource.Play();
        }
    }
}

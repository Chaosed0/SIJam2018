using UnityEngine;
using System.Collections.Generic;

public class AudioSourceGroup : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> sources;

    private int currentIndex = 0;

    public AudioSource Get()
    {
        AudioSource source = sources[currentIndex];
        currentIndex = (currentIndex + 1) % sources.Count;
        return source;
    }
}

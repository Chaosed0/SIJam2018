using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class HeatSequence : MonoBehaviour
{
    [System.Serializable]
    public struct Stop
    {
        [System.Serializable]
        public struct AAA
        {
            public BlastReceiver receiver;
            public GameObject heater;
        }

        public float time;
        public List<AAA> a;
    }

    [SerializeField]
    private SoundLibrary soundLibrary;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioSource loopingSource;

    [SerializeField]
    private List<Stop> stops = new List<Stop>();

    [SerializeField]
    private LayerMask heatLayer;

    bool started = false;
    bool stopped = false;
    int currentIndex = 0;
    float lastStopTime = 0;

    public UnityEvent OnFinished = new UnityEvent();

    private SoundLibrary.Sound needsBlastSound;
    private SoundLibrary.Sound afterBlastSound;

    private HashSet<BlastReceiver> blastList = new HashSet<BlastReceiver>();

    public void StartSequence()
    {
        currentIndex = 0;
        started = true;
        lastStopTime = Time.time;

        needsBlastSound = soundLibrary.GetSound("EngineFail");
        afterBlastSound = soundLibrary.GetSound("SteamHiss");

        loopingSource.Play();
    }

    private void Update()
    {
        if (!started || stopped)
        {
            return;
        }

        Stop stop = stops[currentIndex];
        if ((Time.time - lastStopTime) >= stop.time)
        {
            blastList.Clear();
            foreach (Stop.AAA thing in stop.a)
            {
                thing.receiver.OnBlasted.AddListener(() => OnBlasted(thing));
                blastList.Add(thing.receiver);
                thing.heater.layer = layermask_to_layer(heatLayer.value);
            }

            stopped = true;
            loopingSource.Pause();

            Play(source, needsBlastSound.GetRandom());
        }
    }

    public static int layermask_to_layer(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }

    void OnBlasted(Stop.AAA thing)
    {
        thing.receiver.OnBlasted.RemoveAllListeners();
        if (blastList.Contains(thing.receiver))
        {
            blastList.Remove(thing.receiver);
        }

        thing.heater.layer = 0;
        Play(source, afterBlastSound.GetRandom());

        if (blastList.Count == 0)
        {
            currentIndex++;
            if (currentIndex < stops.Count)
            {
                stopped = false;
                lastStopTime = Time.time;
            }
            else
            {
                started = false;
                OnFinished.Invoke();
            }

            loopingSource.Play();
        }
    }

    private void Play(AudioSource source, AudioClip clip)
    {
        source.Stop();
        source.clip = clip;
        source.Play();
    }
}

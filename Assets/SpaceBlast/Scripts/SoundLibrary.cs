using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SpaceBlast/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public List<AudioClip> clips;

        public AudioClip GetRandom()
        {
            return clips[(int)Random.Range(0f, clips.Count)];
        }
    }

    [SerializeField]
    private List<Sound> library = new List<Sound>();

    public Sound GetSound(string name)
    {
        int index = library.FindIndex((x) => x.name == name);
        if (index < 0)
        {
            return null;
        }

        return library[index];
    }
}

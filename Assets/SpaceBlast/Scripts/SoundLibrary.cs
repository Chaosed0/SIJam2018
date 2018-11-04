using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SpaceBlast/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [System.Serializable]
    public struct Sound
    {
        public string name;
        public List<AudioClip> clips;
    }

    [SerializeField]
    private List<Sound> library = new List<Sound>();

    public AudioClip GetSound(string name)
    {
        int index = library.FindIndex((x) => x.name == name);
        if (index < 0)
        {
            return null;
        }

        return library[index].clips[(int)Random.Range(0f, library[index].clips.Count)];
    }
}

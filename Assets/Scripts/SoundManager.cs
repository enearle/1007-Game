using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.looping;
        }
    }

    public void Play(int index)
    {
        sounds[index].source.Play();
    }
}

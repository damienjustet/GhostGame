using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum SoundType
{
    ITEMHIT,
    DOOROPEN,
    DOORCLOSE,
    POSSESS,
    PLAYERMOVE,
    POPECURIOUS,
    POPEFIND
}

public enum MusicType
{
    LOBBY,
    TUTORIAL,
    LEVEL1,
    TITLESCREEN,
    DEAD
}

public enum AudioType
{
    MUSIC,
    ENVIRONMENT
}

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public bool canSound = false;
    public SoundList[] soundList;
    public SongList[] songList;
    [HideInInspector] public AudioSource musicSource;
    [Range(0f,1f)] public float musicVolume;

    [HideInInspector] public static List<AudioSource> environmentSources = new List<AudioSource>();

    void Awake()
    {
        // environmentSources = new List<AudioSource>();
        canSound = false;
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource s in sources)
        {
            DestroyImmediate(s);
        }
        instance = this;
    
        // Music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        
        
    }

    void Start()
    {
        EnableDistanceVolume();
        ChangeVolume(AudioType.MUSIC);
        ChangeVolume(AudioType.ENVIRONMENT);
        Global.Instance.StartMusic();
    }

    public static void PlaySoundWithSource(AudioSource audioSource, SoundType sound, float volume = 1)
    {
        if (instance.canSound)
        {
            AudioClip[] clips = instance.soundList[(int)sound].clips;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0,clips.Length)];
            float pitch = instance.soundList[(int)sound].volumeAndPitch.pitch;
            float pitchOffset = instance.soundList[(int)sound].volumeAndPitch.randomPitchOffset;
            if (pitchOffset > 0)
            {
                audioSource.pitch = UnityEngine.Random.Range(pitch - pitchOffset,pitch + pitchOffset);
            }
            audioSource.PlayOneShot(randomClip, volume); 
        }
    }

    public static void StartSong(MusicType song, float volume = 1)
    {
        AudioClip clip = instance.songList[(int)song].song;
        instance.musicSource.clip = clip;
        instance.musicSource.Play();

    }

    public void ChangeVolume(AudioType audioType)
    {
        if (audioType == AudioType.MUSIC)
        {
            instance.musicSource.volume = Global.Instance.musicVolume;
        }
        else
        {
            foreach (AudioSource source in environmentSources)
            {
                if (source != null)
                {
                    source.volume = Global.Instance.soundVolume;
                }
            }
        }
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {   
            soundList[i].name = names[i];
        }
        
        string[] songNames = Enum.GetNames(typeof(MusicType));
        Array.Resize(ref songList, songNames.Length);
        for (int i = 0; i < songNames.Length; i++)
        {
            songList[i].name = songNames[i];
        }
    }
#endif

    void EnableDistanceVolume()
    {
        foreach (AudioSource source in environmentSources)
        {
            if (source != null)
            {
                source.spatialBlend = 1;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.maxDistance = 100;
            }
        }
    }
}

[Serializable]
public struct SongList
{
    [HideInInspector] public string name;
    [SerializeField] public AudioClip song;
}

[Serializable]
public struct SoundList
{
    public AudioClip[] clips { get => sounds;}
    [HideInInspector] public string name;
    [SerializeField] public AudioClip[] sounds;
    public Sound volumeAndPitch;
}

[Serializable]
public class Sound
{

    [SerializeField, Range(0f,1f)] public float volume = 1;
    [SerializeField, Range(0f,2f)] public float pitch = 1;
    [SerializeField] public float randomPitchOffset = 0;
    [SerializeField] public bool loop;

    [HideInInspector]
    public AudioSource source;

}

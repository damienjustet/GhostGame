using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum SoundType
{
    ITEMHIT,
    POSSESS,
    PLAYERMOVE
}

public enum MusicType
{
    LOBBY,
    TUTORIAL,
    LEVEL1
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

    void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource s in sources)
        {
            DestroyImmediate(s);
        }
        instance = this;
        foreach (SoundList sList in soundList)
        {
            sList.volumeAndPitch.source = gameObject.AddComponent<AudioSource>();
            sList.volumeAndPitch.source.volume = sList.volumeAndPitch.volume;
            sList.volumeAndPitch.source.pitch = sList.volumeAndPitch.pitch;
            sList.volumeAndPitch.source.loop = sList.volumeAndPitch.loop;
        }
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        
        
    }

    void Start()
    {
        canSound = true;
        Global.Instance.StartMusic();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].clips;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0,clips.Length)];
        instance.soundList[(int)sound].volumeAndPitch.source.PlayOneShot(randomClip, volume);

    }

    public static void StartSound(SoundType sound)
    {
        if (!instance.soundList[(int)sound].volumeAndPitch.source.isPlaying)
        {
            AudioClip[] clips = instance.soundList[(int)sound].clips;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0,clips.Length)];
            instance.soundList[(int)sound].volumeAndPitch.source.clip = randomClip;
            instance.soundList[(int)sound].volumeAndPitch.source.Play();
            instance.soundList[(int)sound].volumeAndPitch.source.volume = 0;
        }
        if (instance.soundList[(int)sound].volumeAndPitch.source.volume < instance.soundList[(int)sound].volumeAndPitch.volume)
        {
            instance.soundList[(int)sound].volumeAndPitch.source.volume += 5 * Time.deltaTime;
        }
        else
        {
            instance.soundList[(int)sound].volumeAndPitch.source.volume = instance.soundList[(int)sound].volumeAndPitch.volume;
        }
        
    }
    public static void StopSound(SoundType sound)
    {
        if (instance.soundList[(int)sound].volumeAndPitch.source.isPlaying)
        {
            instance.soundList[(int)sound].volumeAndPitch.source.volume -= 5 * Time.deltaTime;
            if (instance.soundList[(int)sound].volumeAndPitch.source.volume <= 0)
            {
                instance.soundList[(int)sound].volumeAndPitch.source.Stop();
            }
        }
    }
    public static void StartSong(MusicType song, float volume = 1)
    {
        AudioClip clip = instance.songList[(int)song].song;
        instance.musicSource.clip = clip;
        instance.musicSource.Play();

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


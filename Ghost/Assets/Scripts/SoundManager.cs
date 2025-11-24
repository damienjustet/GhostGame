using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum SoundType
{
    ITEMHIT,
    POSSESS
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
    private static SoundManager instance;
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

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].clips;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0,clips.Length)];
        instance.soundList[(int)sound].volumeAndPitch.source.PlayOneShot(randomClip, volume);

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


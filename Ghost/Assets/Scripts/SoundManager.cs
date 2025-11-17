using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SoundType
{
    ITEMHIT,
    POSSESS,
    MUSIC
}

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public SoundList[] soundList;
    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        foreach (SoundList sList in soundList)
        {
            sList.volumeAndPitch.source = gameObject.AddComponent<AudioSource>();
            sList.volumeAndPitch.source.volume = sList.volumeAndPitch.volume;
            sList.volumeAndPitch.source.pitch = sList.volumeAndPitch.pitch;
            sList.volumeAndPitch.source.loop = sList.volumeAndPitch.loop;
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].clips;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0,clips.Length)];
        
        instance.soundList[(int)sound].volumeAndPitch.source.PlayOneShot(randomClip, volume);
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
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] clips { get => sounds;}
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
    
    public Sound volumeAndPitch;
}

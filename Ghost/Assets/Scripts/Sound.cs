using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{

    [SerializeField, Range(0f,1f)] public float volume;
    [SerializeField, Range(0f,2f)] public float pitch;
    [SerializeField] public bool loop;

    [HideInInspector]
    public AudioSource source;

}

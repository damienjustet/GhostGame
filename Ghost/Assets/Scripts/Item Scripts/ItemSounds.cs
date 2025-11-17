using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource source;
    
    public float volumeChangeMultiplier = 0.2f;
    public float pitchChangeMultiplier = 0.2f;
    void Start()
    {
        source = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void ObjectHit()
    {
        source.clip = sounds[Random.Range(0,sounds.Length)];
        source.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        source.PlayOneShot(source.clip);
    }
}

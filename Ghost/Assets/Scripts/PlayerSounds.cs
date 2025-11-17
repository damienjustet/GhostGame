using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource source;

    public float pitchChangeMultiplier = 0.1f;
    void Start()
    {
        source = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlaySound()
    {
        source.clip = sounds[Random.Range(0,sounds.Length)];
        source.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        source.volume = 1;
        source.PlayOneShot(source.clip);
    }
}

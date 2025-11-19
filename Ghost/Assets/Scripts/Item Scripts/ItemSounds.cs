using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource source;

    public float pitchChangeMultiplier = 0.9f;
    void Start()
    {
        source = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void ObjectHit(float speed)
    {
        source.clip = sounds[Random.Range(0,sounds.Length)];
        source.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        source.volume = Mathf.Max(speed / 5, 0.1f);
        source.PlayOneShot(source.clip);
    }
}

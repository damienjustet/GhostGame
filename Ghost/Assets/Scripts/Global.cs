using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    //This makes script public
    public static Global Instance { get; private set; }
    public bool playerLiving = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        SoundManager.PlaySound(SoundType.MUSIC);
    }

    // checks if ghost is possessed
    public bool isPossessed = false;

    //checks if the player can interact with object(This is for the player script)
    public bool interact;

    // MONEY
    public float money;
}


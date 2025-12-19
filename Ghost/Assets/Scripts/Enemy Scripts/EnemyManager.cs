using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    float time;
    public GameObject pope;
    public GameObject rat;
    public bool spawn = false;
    public bool RatSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        time += Time.deltaTime;
        if(time >= 45 && this.gameObject.name == "popeSpawn" && !spawn)
        {
            Instantiate(pope, transform);
            spawn = true;
        }
        if(time >= 30 && this.gameObject.name == "ratHole" && !RatSpawn)
        {
            Instantiate(rat, transform);
            RatSpawn = true;
            time = 0;
        }
    }


}

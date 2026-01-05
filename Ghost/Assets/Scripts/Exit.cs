using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject exit;

        void Update()
    {
        if (LevelLogic.Instance.canLeave == true)
        {
            exit.transform.position = transform.position;
        }
    }
}

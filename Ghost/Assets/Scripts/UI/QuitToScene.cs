using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class QuitToScene : MonoBehaviour
{
    public void GoTo(string scene)
    {
        Global.Instance.LoadAScene(scene);
    }
}

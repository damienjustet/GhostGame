using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class HelpINeedToFindThisObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int childeren = gameObject.transform.childCount;
        for (int i = 0; i < childeren; i++)
        {
            Light daLight = transform.GetChild(i).GetComponent<Light>();
            daLight.type = LightType.Point;
            daLight.lightmapBakeType = LightmapBakeType.Mixed;
            daLight.range = 9;
            daLight.intensity = 0.33f;
            daLight.shadowResolution = UnityEngine.Rendering.LightShadowResolution.Low;
        }
        DestroyImmediate(gameObject.GetComponent<HelpINeedToFindThisObject>());
    }

}

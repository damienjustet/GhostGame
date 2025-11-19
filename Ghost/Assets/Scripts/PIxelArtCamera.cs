using UnityEngine.UI;
using UnityEngine;

public class PixelArtCamera : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private RawImage _rawImage;

    [SerializeField] private int _cameraHeight;

    private RenderTexture _renderTexture;
    
    private Transform mainCam;

    // Start is called before the first frame update
    void Start()
    {
        UpdateRenderTexture();
        // mainCam = GameObject.Find("Main Camera").transform;
        // transform.position = mainCam.position;
        // transform.rotation = mainCam.rotation;
    }

    public void UpdateRenderTexture()
    {
        if(_renderTexture != null)
        {
            _renderTexture.Release();
        }

        float aspectRatio = (float)Screen.width / Screen.height;
        int cameraWidth = Mathf.RoundToInt(aspectRatio * _cameraHeight);    

        _renderTexture = new RenderTexture(cameraWidth, _cameraHeight, 16, RenderTextureFormat.ARGB32);
        _renderTexture.filterMode = FilterMode.Point;

        _renderTexture.Create();
        _camera.targetTexture = _renderTexture;
        _rawImage.texture = _renderTexture;

    }

    // void Update()
    // {
    //     transform.position = mainCam.position;
    //     transform.rotation = mainCam.rotation;
    // }

}

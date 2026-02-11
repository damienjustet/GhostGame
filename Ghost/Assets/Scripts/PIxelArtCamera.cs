using UnityEngine.UI;
using UnityEngine;

public class PixelArtCamera : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] public RawImage _rawImage;

    [SerializeField] private int _cameraHeight;

    [HideInInspector] public RenderTexture _renderTexture;

    
    void Awake()
    {
        UpdateRenderTexture();
    }

    public void UpdateRenderTexture()
    {
        if (gameObject.name == "Main Camera(Clone)")
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
    }

    void Update()
    {
        if (_camera.targetTexture == null)
        {
            PixelArtCamera mainCamScript = Camera.main.GetComponent<PixelArtCamera>();
            _camera.targetTexture = mainCamScript._renderTexture;
        }
    }
    

}

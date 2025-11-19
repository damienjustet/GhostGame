using UnityEngine.UI;
using UnityEngine;

public class PixelArtCamera : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private RawImage _rawImage;

    [SerializeField] private int _cameraHeight;

    [HideInInspector] public RenderTexture _renderTexture;

    // Start is called before the first frame update
    void Start()
    {
        UpdateRenderTexture();
    }

    public void UpdateRenderTexture()
    {
        if (gameObject.tag == "MainCamera")
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
            _camera.targetTexture = GameObject.Find("Main Camera").GetComponent<PixelArtCamera>()._renderTexture;
        }
    }
    

}

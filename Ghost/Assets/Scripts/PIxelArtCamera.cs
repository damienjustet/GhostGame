using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;

public class PixelArtCamera : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] public RawImage _rawImage;

    [SerializeField] private int _cameraHeight;

    [HideInInspector] public RenderTexture _renderTexture;

    private RawImage _textRawImage;
    private RenderTexture _textRenderTexture;
    private bool _isRendering = false;

    
    void Awake()
    {
        _camera.cullingMask &= ~LayerMask.GetMask("UI");
        UpdateRenderTexture();
    }

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    void Start()
    {
        // Find Text Display RawImage
        RawImage[] allRawImages = FindObjectsOfType<RawImage>(true);
        foreach (RawImage img in allRawImages)
        {
            if (img.gameObject.name == "Text Display")
            {
                _textRawImage = img;
                break;
            }
        }
        SetupTextRenderTexture();
    }

    void SetupTextRenderTexture()
    {
        if (_textRawImage == null) return;

        if (_textRenderTexture != null)
            _textRenderTexture.Release();

        _textRenderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        _textRenderTexture.filterMode = FilterMode.Bilinear;
        _textRenderTexture.Create();
        _textRawImage.texture = _textRenderTexture;
    }

    void OnEndCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        if (cam != _camera) return;
        if (_textRawImage == null || _textRenderTexture == null) return;
        if (gameObject.name != "Main Camera(Clone)") return;
        if (_isRendering) return; // Prevent recursive rendering

        _isRendering = true;

        // Save original settings
        RenderTexture originalTarget = _camera.targetTexture;
        int originalMask = _camera.cullingMask;
        CameraClearFlags originalClear = _camera.clearFlags;
        Color originalBg = _camera.backgroundColor;

        // Render UI only at full resolution
        _camera.targetTexture = _textRenderTexture;
        _camera.cullingMask = LayerMask.GetMask("UI");
        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.backgroundColor = new Color(0, 0, 0, 0);

        _camera.Render();

        // Restore original settings
        _camera.targetTexture = originalTarget;
        _camera.cullingMask = originalMask;
        _camera.clearFlags = originalClear;
        _camera.backgroundColor = originalBg;

        _isRendering = false;
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

            SetupTextRenderTexture();
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

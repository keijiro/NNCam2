using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace NNCam2 {

sealed class BodyPixOverlayController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField, Range(0, 1)] float _dithering = 0.5f;
    [SerializeField] Color _lineColor = Color.white;
    [SerializeField] float _lineThreshold = 0.5f;
    [SerializeField] float _lineContrast = 1;

    #endregion

    #region Private variables and objects

    float _backOpacity = 1;
    float _frontOpacity = 1;

    Material _material;

    #endregion

    #region Random palette generator

    void ShufflePalette()
    {
        var h1 = Random.value;
        var h2 = (h1 + 0.333f) % 1;

        var h3 = Random.value;
        var h4 = (h3 + 0.333f) % 1;

        var bg1 = Color.black;
        var bg2 = Color.HSVToRGB(h1, 1, 0.5f);
        var bg3 = Color.HSVToRGB(h2, 1, 0.8f);

        var fg1 = Color.HSVToRGB(h3, 1, 0.3f);
        var fg2 = Color.HSVToRGB(h4, 1, 1.0f);
        var fg3 = Color.white;

        var mbg = new Matrix4x4();
        var mfg = new Matrix4x4();

        mbg.SetRow(0, bg1);
        mbg.SetRow(1, bg1);
        mbg.SetRow(2, bg2);
        mbg.SetRow(3, bg3);

        mfg.SetRow(0, fg1);
        mfg.SetRow(1, fg1);
        mfg.SetRow(2, fg2);
        mfg.SetRow(3, fg3);

        _material.SetMatrix("_PaletteBG", mbg);
        _material.SetMatrix("_PaletteFG", mfg);
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        // Get a reference to the FullScreenCustomPass object.
        var volume = GetComponent<CustomPassVolume>();
        var pass = (FullScreenCustomPass)volume.customPasses[0];

        // Clone and replace the original material.
        _material = new Material(pass.fullscreenPassMaterial);
        _material.name += " (Clone)";
        pass.fullscreenPassMaterial = _material;

        // Initial random palette
        ShufflePalette();
    }

    void LateUpdate()
    {
        // Fill parameter update
        var fillParams = new Vector3(_backOpacity, _frontOpacity, _dithering);
        _material.SetVector("_FillParams", fillParams);

        // Line parameter update
        var lineParams = new Vector2(_lineThreshold, _lineContrast);
        _material.SetColor("_LineColor", _lineColor);
        _material.SetVector("_LineParams", lineParams);
    }

    #endregion
}

} // namespace NNCam2

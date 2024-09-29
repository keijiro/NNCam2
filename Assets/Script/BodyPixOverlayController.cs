using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace NNCam2 {

public sealed class BodyPixOverlayController : MonoBehaviour
{
    #region Posterize effect

    [SerializeField, Range(0, 1)] float _dithering = 0.5f;

    [field:SerializeField] public float BackgroundOpacity { get; set; }
    [field:SerializeField] public float ForegroundOpacity { get; set; }

    public void ShufflePalette()
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

    void UpdatePosterizeEffect()
      => _material.SetVector
           (ShaderID.FillParams, BackgroundOpacity, ForegroundOpacity, _dithering);

    #endregion

    #region Line effect

    [SerializeField] Color _lineColor = Color.white;
    [SerializeField] float _lineThreshold = 0.5f;
    [SerializeField] float _lineContrast = 1;

    void UpdateLineEffect()
    {
        _material.SetColor(ShaderID.LineColor, _lineColor);
        _material.SetVector(ShaderID.LineParams, _lineThreshold, _lineContrast);
    }

    #endregion

    #region Wiper effect

    [SerializeField] Color _wiperColor = Color.red;

    [field:SerializeField] public bool RandomizeWiperDirection { get; set; }
    [field:SerializeField] public bool EnableForegroundWiper { get; set; }
    [field:SerializeField] public bool EnableBackgroundWiper { get; set; } = true;

    Vector4 _wiperParams = Vector4.one;
    Vector4 _wiperCounts;
    int _nextWiper;

    Vector4 WiperVector;

    public void KickWiper()
    {
        _wiperParams[_nextWiper] = 0;
        _wiperCounts[_nextWiper] += 1;
        _nextWiper = (_nextWiper + 1) & 3;
    }

    void UpdateWiperEffect()
    {
        var dt = Time.deltaTime;
        for (var i = 0; i < 4; i++) _wiperParams[i] += dt;

        var config = RandomizeWiperDirection ? 1 : 0;
        config |= EnableBackgroundWiper ? 2 : 0;
        config |= EnableForegroundWiper ? 4 : 0;

        _material.SetInteger(ShaderID.WiperConfig, config);
        _material.SetColor(ShaderID.WiperColor, _wiperColor);
        _material.SetVector(ShaderID.WiperParams, _wiperParams);
        _material.SetVector(ShaderID.WiperCounts, _wiperCounts);
    }

    #endregion

    #region MonoBehaviour implementation

    Material _material;

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
        UpdatePosterizeEffect();
        UpdateLineEffect();
        UpdateWiperEffect();
    }

    #endregion
}

} // namespace NNCam2

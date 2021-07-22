using UnityEngine;
using Klak.TestTools;

namespace NNCam2 {

sealed class BodyPixFilterFeedback : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] ImageSource _source = null;
    [SerializeField] RenderTexture _mask = null;
    [SerializeField] Shader _shader = null;
    [SerializeField] RenderTexture _output = null;

    #endregion

    #region Effect parameters

    [SerializeField] float _feedbackLength = 3;
    [SerializeField] float _feedbackDecay = 1;
    [SerializeField] float _noiseFrequency = 1;
    [SerializeField] float _noiseSpeed = 1;
    [SerializeField] float _noiseAmount = 1;

    public float FeedbackLength
      { get => _feedbackLength; set => _feedbackLength = value; }

    public float FeedbackDecay
      { get => _feedbackDecay; set => _feedbackDecay = value; }

    public float NoiseFrequency
      { get => _noiseFrequency; set => _noiseFrequency = value; }

    public float NoiseSpeed
      { get => _noiseSpeed; set => _noiseSpeed = value; }

    public float NoiseAmount
      { get => _noiseAmount; set => _noiseAmount = value; }

    #endregion

    #region Private members

    Vector4 FeedbackParamsVector
      => new Vector4(_feedbackLength, _feedbackDecay / 100,
                     (float)_buffer.rt1.width / _buffer.rt1.height, Time.time);

    Vector3 NoiseParamsVector
      => new Vector3(_noiseFrequency, _noiseSpeed, _noiseAmount / 1000);

    (RenderTexture rt1, RenderTexture rt2) _buffer;

    Material _material;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _material = new Material(_shader);
        _buffer = (RTUtil.NewARGBHalf(_source.OutputResolution),
                   RTUtil.NewARGBHalf(_source.OutputResolution));
    }

    void OnDestroy()
    {
        Destroy(_material);
        Destroy(_buffer.rt1);
        Destroy(_buffer.rt2);
    }

    void LateUpdate()
    {
        // Effector shader
        _material.SetTexture(ShaderID.SourceTexture, _source.Texture);
        _material.SetTexture(ShaderID.FeedbackTexture, _buffer.rt1);
        _material.SetTexture(ShaderID.MaskTexture, _mask);
        _material.SetVector(ShaderID.FeedbackParams, FeedbackParamsVector);
        _material.SetVector(ShaderID.NoiseParams, NoiseParamsVector);
        _material.SetPass(0);
        Graphics.SetRenderTarget(_buffer.rt2);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);

        Graphics.Blit(_buffer.rt2, _output);

        // Double buffer swapping
        _buffer = (_buffer.rt2, _buffer.rt1);
    }

    #endregion
}

} // namespace NNCam2

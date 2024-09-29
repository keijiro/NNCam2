using UnityEngine;
using Klak.TestTools;

namespace NNCam2 {

public sealed class BodyPixFilterFeedback : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] ImageSource _source = null;
    [SerializeField] RenderTexture _mask = null;
    [SerializeField] Shader _shader = null;
    [SerializeField] RenderTexture _output = null;

    #endregion

    #region Effect parameters

    [field:SerializeField] public float FeedbackLength { get; set; } = 3;
    [field:SerializeField] public float FeedbackDecay { get; set; } = 1;
    [field:SerializeField] public float NoiseFrequency { get; set; } = 1;
    [field:SerializeField] public float NoiseSpeed { get; set; } = 1;
    [field:SerializeField] public float NoiseAmount { get; set; } = 1;

    #endregion

    #region Private members

    Vector4 FeedbackParamsVector
      => new Vector4(FeedbackLength, FeedbackDecay / 100,
                     (float)_buffer.rt1.width / _buffer.rt1.height, Time.time);

    Vector3 NoiseParamsVector
      => new Vector3(NoiseFrequency, NoiseSpeed, NoiseAmount / 1000);

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
        Graphics.SetRenderTarget(_buffer.rt2);
        _material.SetTexture(ShaderID.SourceTexture, _source.AsTexture);
        _material.SetTexture(ShaderID.FeedbackTexture, _buffer.rt1);
        _material.SetTexture(ShaderID.MaskTexture, _mask);
        _material.SetVector(ShaderID.FeedbackParams, FeedbackParamsVector);
        _material.SetVector(ShaderID.NoiseParams, NoiseParamsVector);
        _material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);

        Graphics.Blit(_buffer.rt2, _output);

        // Double buffer swapping
        _buffer = (_buffer.rt2, _buffer.rt1);
    }

    #endregion
}

} // namespace NNCam2

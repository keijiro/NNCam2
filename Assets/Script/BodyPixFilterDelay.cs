using UnityEngine;
using Klak.TestTools;

namespace NNCam2 {

public sealed class BodyPixFilterDelay : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] ImageSource _source = null;
    [SerializeField] RenderTexture _mask = null;
    [SerializeField] Shader _shader = null;
    [SerializeField] RenderTexture _output = null;

    #endregion

    #region Public property

    public float DelayAmount { get; set; }

    #endregion

    #region Private members

    const int MaxHistory = 32;

    Material _material;
    Texture2DArray _history;
    int _count;

    #endregion

    #region MonoBehaviour implementation

    void OnEnable()
      => _count = 0;

    void Start()
    {
        _material = new Material(_shader);
        _history = new Texture2DArray
          (1920, 1080, MaxHistory, TextureFormat.RGB565, false, false);
        _history.filterMode = FilterMode.Bilinear;
        _history.wrapMode = TextureWrapMode.Clamp;
    }

    void OnDestroy()
    {
        Destroy(_material);
        Destroy(_history);
    }

    void LateUpdate()
    {
        // Preprocessing
        Graphics.SetRenderTarget(_output);
        _material.SetTexture(ShaderID.SourceTexture, _source.AsTexture);
        _material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);

        // Buffering
        Graphics.ConvertTexture(_output, 0, _history, _count % MaxHistory);

        // Filter output
        Graphics.SetRenderTarget(_output);
        _material.SetTexture(ShaderID.HistoryTexture, _history);
        _material.SetTexture(ShaderID.MaskTexture, _mask);
        _material.SetInteger(ShaderID.MaxHistory, MaxHistory);
        _material.SetInteger(ShaderID.FrameIndex, _count);
        _material.SetFloat(ShaderID.DelayAmount, DelayAmount);
        _material.SetPass(1);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);

        // Frame count
        _count++;
    }

    #endregion
}

} // namespace NNCam2

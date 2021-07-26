using UnityEngine;
using Klak.TestTools;

namespace NNCam2 {

public sealed class BodyPixFilterSlitscan : MonoBehaviour
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

    const int History = 128;

    Material _material;
    Texture2DArray _buffer;
    int _count;

    #endregion

    #region MonoBehaviour implementation

    void OnEnable()
      => _count = 0;

    void Start()
    {
        _material = new Material(_shader);
        _buffer = new Texture2DArray
          (1920, 1080, History, TextureFormat.RGB565, false);
        _buffer.filterMode = FilterMode.Bilinear;
        _buffer.wrapMode = TextureWrapMode.Clamp;
    }

    void OnDestroy()
    {
        Destroy(_material);
        Destroy(_buffer);
    }

    void LateUpdate()
    {
        // Buffering
        Graphics.ConvertTexture(_source.Texture, 0, _buffer, _count % History);

        // Filter output
        Graphics.SetRenderTarget(_output);
        _material.SetTexture(ShaderID.SourceTexture, _source.Texture);
        _material.SetTexture(ShaderID.BufferTexture, _buffer);
        _material.SetTexture(ShaderID.MaskTexture, _mask);
        _material.SetInteger(ShaderID.FrameCount, _count);
        _material.SetFloat(ShaderID.DelayAmount, DelayAmount * (History - 1));
        _material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);

        // Frame count
        _count++;
    }

    #endregion
}

} // namespace NNCam2

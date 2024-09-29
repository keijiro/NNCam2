using UnityEngine;
using Klak.TestTools;

namespace NNCam2 {

public sealed class BodyPixFilterThru : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] ImageSource _source = null;
    [SerializeField] RenderTexture _mask = null;
    [SerializeField] Shader _shader = null;
    [SerializeField] RenderTexture _output = null;

    #endregion

    #region MonoBehaviour implementation

    Material _material;

    void Start()
      => _material = new Material(_shader);

    void OnDestroy()
      => Destroy(_material);

    void LateUpdate()
    {
        Graphics.SetRenderTarget(_output);
        _material.SetTexture(ShaderID.SourceTexture, _source.AsTexture);
        _material.SetTexture(ShaderID.MaskTexture, _mask);
        _material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);
    }

    #endregion
}

} // namespace NNCam2

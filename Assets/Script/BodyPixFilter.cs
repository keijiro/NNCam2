using UnityEngine;
using Klak.TestTools;
using BodyPix;

namespace NNCam2 {

sealed class BodyPixFilter : MonoBehaviour
{
    [SerializeField] ImageSource _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Shader _shader = null;
    [SerializeField] RenderTexture _filterOutput = null;
    [SerializeField] RenderTexture _maskOutput = null;

    RenderBuffer[] _mrt = new RenderBuffer[2];
    BodyPixRuntime _bodyPix;
    Material _material;

    void Start()
    {
        _bodyPix = new BodyPixRuntime(_resources, 512, 384);
        _material = new Material(_shader);
    }

    void OnDestroy()
    {
        _bodyPix.Dispose();
        Destroy(_material);
    }

    void LateUpdate()
    {
        _bodyPix.ProcessImage(_source.Texture);

        _material.SetTexture("_BodyPixTexture", _bodyPix.Mask);
        _material.SetPass(0);
        Graphics.SetRenderTarget(_maskOutput);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);

        _material.SetTexture("_SourceTexture", _source.Texture);
        _material.SetTexture("_MaskTexture", _maskOutput);
        Graphics.SetRenderTarget(_filterOutput);
        _material.SetPass(1);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);
    }
}

} // namespace NNCam2

using UnityEngine;
using Klak.TestTools;
using BodyPix;

namespace NNCam2 {

sealed class BodyPixInput : MonoBehaviour
{
    [SerializeField] ImageSource _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Shader _shader = null;
    [SerializeField] RenderTexture _output = null;

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
        Graphics.SetRenderTarget(_output);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);
    }
}

} // namespace NNCam2

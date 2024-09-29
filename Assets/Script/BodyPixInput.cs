using UnityEngine;
using Klak.TestTools;
using BodyPix;

namespace NNCam2 {

public sealed class BodyPixInput : MonoBehaviour
{
    [SerializeField] ImageSource _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Shader _shader = null;
    [SerializeField] RenderTexture _output = null;

    public GraphicsBuffer KeypointBuffer => _detector.KeypointBuffer;

    BodyDetector _detector;
    Material _material;

    void Start()
    {
        _detector = new BodyDetector(_resources, 512, 384);
        _material = new Material(_shader);
    }

    void OnDestroy()
    {
        _detector.Dispose();
        Destroy(_material);
    }

    void LateUpdate()
    {
        _detector.ProcessImage(_source.AsTexture);

        Graphics.SetRenderTarget(_output);
        _material.SetTexture(ShaderID.BodyPixTexture, _detector.MaskTexture);
        _material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 3, 1);
    }
}

} // namespace NNCam2

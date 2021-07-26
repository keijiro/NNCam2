using UnityEngine;

namespace NNCam2 {

static class ShaderID
{
    public static readonly int BodyPixTexture = Shader.PropertyToID("_BodyPixTexture");
    public static readonly int BufferCount = Shader.PropertyToID("_BufferCount");
    public static readonly int BufferTexture = Shader.PropertyToID("_BufferTexture");
    public static readonly int DelayAmount = Shader.PropertyToID("_DelayAmount");
    public static readonly int FeedbackTexture = Shader.PropertyToID("_FeedbackTexture");
    public static readonly int FeedbackParams = Shader.PropertyToID("_FeedbackParams");
    public static readonly int FillParams = Shader.PropertyToID("_FillParams");
    public static readonly int FrameCount = Shader.PropertyToID("_FrameCount");
    public static readonly int LineColor = Shader.PropertyToID("_LineColor");
    public static readonly int LineParams = Shader.PropertyToID("_LineParams");
    public static readonly int MainTexture = Shader.PropertyToID("_MainTexture");
    public static readonly int MaskTexture = Shader.PropertyToID("_MaskTexture");
    public static readonly int NoiseParams = Shader.PropertyToID("_NoiseParams");
    public static readonly int Opacity = Shader.PropertyToID("_Opacity");
    public static readonly int SourceTexture = Shader.PropertyToID("_SourceTexture");
    public static readonly int WiperColor = Shader.PropertyToID("_WiperColor");
    public static readonly int WiperConfig = Shader.PropertyToID("_WiperConfig");
    public static readonly int WiperCounts = Shader.PropertyToID("_WiperCounts");
    public static readonly int WiperParams = Shader.PropertyToID("_WiperParams");
}

static class MaterialExtensions
{
    public static void SetVector
      (this Material m, int id, float x, float y = 0, float z = 0, float w = 0)
      => m.SetVector(id, new Vector4(x, y, z, w));
}

static class RTUtil
{
    public static RenderTexture NewARGBHalf(int width, int height)
      => new RenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf);

    public static RenderTexture NewARGBHalf(Vector2Int dims)
      => NewARGBHalf(dims.x, dims.y);
}

static class ObjectUtil
{
    public static void Destroy(Object o)
    {
        if (o == null) return;
        if (Application.isPlaying)
            Object.Destroy(o);
        else
            Object.DestroyImmediate(o);
    }
}

} // namespace NNCam2

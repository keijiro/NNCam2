using UnityEngine;

namespace NNCam2 {

static class ShaderID
{
    public static readonly int BodyPixTexture = Shader.PropertyToID("_BodyPixTexture");
    public static readonly int FeedbackTexture = Shader.PropertyToID("_FeedbackTexture");
    public static readonly int FeedbackParams = Shader.PropertyToID("_FeedbackParams");
    public static readonly int MainTexture = Shader.PropertyToID("_MainTexture");
    public static readonly int MaskTexture = Shader.PropertyToID("_MaskTexture");
    public static readonly int NoiseParams = Shader.PropertyToID("_NoiseParams");
    public static readonly int SourceTexture = Shader.PropertyToID("_SourceTexture");
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

using UnityEngine;

namespace NNCam2 {

static class ShaderID
{
    public static readonly int MainTexture = Shader.PropertyToID("_MainTexture");
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

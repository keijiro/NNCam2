#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

float _TestValue;

float4 SampleColorSRGB(uint2 pcs)
{
    float4 cs = LOAD_TEXTURE2D_X_LOD(_ColorPyramidTexture, pcs, 0);
    return FastLinearToSRGB(cs);
}

float4 FullScreenPass(Varyings varyings) : SV_Target
{
    // Clip space position
    uint2 pcs = varyings.positionCS.xy;

    // Color samples in sRGB
    float4 c0 = SampleColorSRGB(pcs + uint2(0, 0));

    return float4(FastSRGBToLinear(lerp(c0.rgb, 1 - c0.rgb, c0.a)), 1);
}

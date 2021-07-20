#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

sampler2D _MainTexture;

void FullScreenPass(Varyings varyings,
                    out float4 outColor : SV_Target,
                    out float outDepth : SV_Depth)
{
    float2 uv = (varyings.positionCS.xy + float2(0.5, 0.5)) * _ScreenSize.zw;
    float3 rgb = tex2D(_MainTexture, uv).rgb;
    outColor = float4(rgb, uv.x > 0.5);
    outDepth = 0;
}

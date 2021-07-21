#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

sampler2D _MainTexture;

void FullScreenPass(Varyings varyings,
                    out float4 outColor : SV_Target,
                    out float outDepth : SV_Depth)
{
    float2 uv = (varyings.positionCS.xy + float2(0.5, 0.5)) * _ScreenSize.zw;
    outColor = tex2D(_MainTexture, uv);
    outDepth = 0;
}

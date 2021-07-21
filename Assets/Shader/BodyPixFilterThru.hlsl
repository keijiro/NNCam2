#include "Common.hlsl"
#include "Packages/jp.keijiro.bodypix/Shader/Common.hlsl"

sampler2D _SourceTexture;
sampler2D _MaskTexture;

float4 Fragment(float4 position : SV_Position,
                float2 texCoord : TEXCOORD) : SV_Target
{
    float4 c = tex2D(_SourceTexture, texCoord);
    float4 m = tex2D(_MaskTexture, texCoord);
    return float4(c.rgb, m.a);
}

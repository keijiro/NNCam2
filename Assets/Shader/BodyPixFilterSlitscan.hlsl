#include "Common.hlsl"
#include "Packages/jp.keijiro.bodypix/Shader/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

#define HISTORY 128

TEXTURE2D_ARRAY(_BufferTexture);
SAMPLER(sampler_BufferTexture);
sampler2D _SourceTexture;
sampler2D _MaskTexture;
uint _FrameCount;
float _DelayAmount;

float3 GetHistory(float2 uv, uint offset)
{
    uint i = (_FrameCount + HISTORY - offset) & (HISTORY - 1);
    return SAMPLE_TEXTURE2D_ARRAY(_BufferTexture, sampler_BufferTexture, uv, i).rgb;
}

float4 Fragment(float4 position : SV_Position,
                float2 texCoord : TEXCOORD) : SV_Target
{
    float delay = texCoord.x * _DelayAmount;
    uint offset = (uint)delay;
    float3 p1 = GetHistory(texCoord, offset + 0);
    float3 p2 = GetHistory(texCoord, offset + 1);
    float3 scan = lerp(p1, p2, frac(delay));

    float4 source = tex2D(_SourceTexture, texCoord);
    float4 mask = tex2D(_MaskTexture, texCoord);

    return float4(scan, mask.a);
}

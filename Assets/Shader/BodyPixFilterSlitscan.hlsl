#include "Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

TEXTURE2D_ARRAY(_HistoryTexture);
SAMPLER(sampler_HistoryTexture);
sampler2D _MaskTexture;
uint _MaxHistory;
uint _FrameIndex;
float _DelayAmount;

float3 GetHistory(float2 uv, uint offset)
{
    uint i = (_FrameIndex + _MaxHistory - offset) % _MaxHistory;
    i = min(i, _FrameIndex);
    return SAMPLE_TEXTURE2D_ARRAY(_HistoryTexture, sampler_HistoryTexture, uv, i).rgb;
}

float4 Fragment(float4 position : SV_Position,
                float2 texCoord : TEXCOORD) : SV_Target
{
    float delay = texCoord.x * _DelayAmount * (_MaxHistory - 1);
    uint offset = (uint)delay;
    float3 p1 = GetHistory(texCoord, offset);
    float3 p2 = GetHistory(texCoord, offset + 1);
    float3 scan = lerp(p1, p2, frac(delay));
    return float4(scan, tex2D(_MaskTexture, texCoord).a);
}

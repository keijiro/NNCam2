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
    float3 acc = 0;
    for (uint i = 0; i < 8; i++)
    {
        // Source with monochrome + contrast
        uint offs = i * _DelayAmount * (_MaxHistory - 1) / 8;
        float3 c = GetHistory(texCoord, offs);
        // Hue
        float h = i / 8.0 * 6 - 2;
        c *= saturate(float3(abs(h - 1) - 1, 2 - abs(h), 2 - abs(h - 2)));
        // Accumulation
        acc += c / 4;
    }
    return float4(acc, tex2D(_MaskTexture, texCoord).a);
}

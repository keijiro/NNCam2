#include "Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

TEXTURE2D_ARRAY(_HistoryTexture);
SAMPLER(sampler_HistoryTexture);
sampler2D _SourceTexture;
sampler2D _MaskTexture;
uint _MaxHistory;
uint _FrameIndex;
float _DelayAmount;

float3 GetHistory(float2 uv, uint offset)
{
    uint i = min((_FrameIndex + _MaxHistory - offset) % _MaxHistory, _FrameIndex);
    return SAMPLE_TEXTURE2D_ARRAY(_HistoryTexture, sampler_HistoryTexture, uv, i).rgb;
}

// Common preprocess
float4 FragmentPreprocess(float4 position : SV_Position,
                          float2 texCoord : TEXCOORD) : SV_Target
{
    return float4(FastLinearToSRGB(tex2D(_SourceTexture, texCoord).rgb), 1);
}

// Delay effect
float4 FragmentDelay(float4 position : SV_Position,
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
    acc = FastSRGBToLinear(acc);
    return float4(acc, tex2D(_MaskTexture, texCoord).a);
}

// Slitscan effect
float4 FragmentSlitscan(float4 position : SV_Position,
                        float2 texCoord : TEXCOORD) : SV_Target
{
    float delay = texCoord.x * _DelayAmount * (_MaxHistory - 1);
    uint offset = (uint)delay;
    float3 p1 = GetHistory(texCoord, offset);
    float3 p2 = GetHistory(texCoord, offset + 1);
    float3 scan = lerp(p1, p2, frac(delay));
    scan = FastSRGBToLinear(scan);
    return float4(scan, tex2D(_MaskTexture, texCoord).a);
}

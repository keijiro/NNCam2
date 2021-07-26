#include "Common.hlsl"
#include "Packages/jp.keijiro.bodypix/Shader/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

#define HISTORY 32

TEXTURE2D_ARRAY(_BufferTexture);
SAMPLER(sampler_BufferTexture);
sampler2D _MaskTexture;
float _DelayAmount;
uint _FrameCount;

float3 GetHistory(float2 uv, uint offset)
{
    uint i = (_FrameCount + HISTORY - offset) & (HISTORY - 1);
    return SAMPLE_TEXTURE2D_ARRAY(_BufferTexture, sampler_BufferTexture, uv, i).rgb;
}

float4 Fragment(float4 position : SV_Position,
                float2 texCoord : TEXCOORD) : SV_Target
{
    float3 acc = 0;

    for (uint i = 0; i < 8; i++)
    {
        // Source with monochrome + contrast
        float3 c = GetHistory(texCoord, i * _DelayAmount);

        // Hue
        float h = i / 8.0 * 6 - 2;
        c *= saturate(float3(abs(h - 1) - 1, 2 - abs(h), 2 - abs(h - 2)));

        // Accumulation
        acc += c / 4;
    }

    float4 mask = tex2D(_MaskTexture, texCoord);
    return float4(acc, mask.a);
}

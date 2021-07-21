#include "Packages/jp.keijiro.bodypix/Shader/Common.hlsl"

sampler2D _SourceTexture;

texture2D _MaskTexture;
float4 _MaskTexture_TexelSize;

float3 HueToRGB(float h)
{
    h = frac(saturate(h)) * 6 - 2;
    float3 c = saturate(float3(abs(h - 1) - 1, 2 - abs(h), 2 - abs(h - 2)));
    return c;
}

void Vertex(float4 position : POSITION,
            float2 texCoord : TEXCOORD,
            out float4 outPosition : SV_Position,
            out float2 outTexCoord : TEXCOORD)
{
    float2 p = position.xy * float2(2, -2) + float2(-1, 1);
    outPosition = float4(p, 1, 1);
    outTexCoord = texCoord;
}

float4 Fragment(float4 position : SV_Position,
                float2 texCoord : TEXCOORD) : SV_Target
{
    BodyPix_Mask mask =
      BodyPix_SampleMask(texCoord, _MaskTexture, _MaskTexture_TexelSize.zw);

    float3 acc = 0;
    for (uint part = 0; part < BODYPIX_PART_COUNT; part++)
    {
        float score = BodyPix_EvalPart(mask, part);
        score = smoothstep(0.47, 0.57, score);
        acc += HueToRGB((float)part / BODYPIX_PART_COUNT) * score;
    }

    float alpha = BodyPix_EvalSegmentation(mask);
    alpha = smoothstep(0.47, 0.57, alpha);

    float3 rgb = tex2D(_SourceTexture, texCoord).rgb;
    return float4(lerp(rgb, acc, alpha), 1 - alpha);
}

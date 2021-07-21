#include "Packages/jp.keijiro.bodypix/Shader/Common.hlsl"

sampler2D _SourceTexture;
sampler2D _MaskTexture;

texture2D _BodyPixTexture;
float4 _BodyPixTexture_TexelSize;

void Vertex(uint vid : SV_VertexID,
            out float4 outPosition : SV_Position,
            out float2 outTexCoord : TEXCOORD)
{
    float x = (vid & 1) * 2;
    float y = (vid > 1) * 2;
    outPosition = float4(x * 2 - 1, 1 - y * 2, 1, 1);
    outTexCoord = float2(x, y);
}

float4 FragmentMask(float4 position : SV_Position,
                    float2 texCoord : TEXCOORD) : SV_Target
{
    BodyPix_Mask mask = BodyPix_SampleMask
      (texCoord, _BodyPixTexture, _BodyPixTexture_TexelSize.zw);

    // Head
    float head = max(BodyPix_EvalPart(mask, 0), BodyPix_EvalPart(mask, 1));

    // Arm
    float arm = 0;
    for (uint i = 2; i < 12; i++)
        arm = max(arm, BodyPix_EvalPart(mask, i));

    // Body
    float body = 0;
    for (uint i = 12; i < 14; i++)
        body = max(body, BodyPix_EvalPart(mask, i));

    // Person
    float alpha = BodyPix_EvalSegmentation(mask);

    // Combined output
    return smoothstep(0.47, 0.57, float4(head, body, arm, 1) * alpha);
}

float4 FragmentFilter(float4 position : SV_Position,
                      float2 texCoord : TEXCOORD) : SV_Target
{
    float4 c = tex2D(_SourceTexture, texCoord);
    float4 m = tex2D(_MaskTexture, texCoord);
    return float4(c.rgb, m.a);
}

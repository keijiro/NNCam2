#include "Common.hlsl"
#include "Packages/jp.keijiro.bodypix/Shaders/Common.hlsl"

texture2D _BodyPixTexture;
float4 _BodyPixTexture_TexelSize;

float4 Fragment(float4 position : SV_Position,
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
    for (i = 12; i < 14; i++)
        body = max(body, BodyPix_EvalPart(mask, i));

    // Person
    float alpha = BodyPix_EvalSegmentation(mask);

    // Combined output
    return smoothstep(0.4, 0.6, float4(head, body, arm, 1) * alpha);
}

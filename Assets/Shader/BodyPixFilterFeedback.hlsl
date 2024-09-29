#include "Common.hlsl"
#include "Packages/jp.keijiro.bodypix/Shaders/Common.hlsl"
#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise3D.hlsl"

sampler2D _SourceTexture;
sampler2D _MaskTexture;
sampler2D _FeedbackTexture;
float4 _FeedbackParams; // length, decay, aspect, time
float3 _NoiseParams;    // frequency, speed, amount

float2 DFNoise(float2 uv, float3 freq)
{
    float time = _FeedbackParams.w;
    float3 np = float3(uv, time) * freq;
    float2 n1 = SimplexNoiseGrad(np).xy;
    return cross(float3(n1, 0), float3(0, 0, 1)).xy;
}

float2 Displacement(float2 uv)
{
    float aspect = _FeedbackParams.z;
    float2 p = uv * float2(aspect, 1);
    float2 n = DFNoise(p, _NoiseParams.xxy * -1) * _NoiseParams.z +
               DFNoise(p, _NoiseParams.xxy * +2) * _NoiseParams.z * 0.5;
    return n * float2(1, aspect);
}

float4 Fragment(float4 position : SV_Position,
                float2 texCoord : TEXCOORD) : SV_Target
{
    float3 source = tex2D(_SourceTexture, texCoord).rgb;
    float4 feedback = tex2D(_FeedbackTexture, texCoord + Displacement(texCoord));

    float mask = smoothstep(0.9, 1, tex2D(_MaskTexture, texCoord).a);

    float alpha = lerp(feedback.a * (1 - _FeedbackParams.y), _FeedbackParams.x, mask);
    float3 rgb = lerp(source, feedback.rgb, saturate(alpha) * (1 - mask));

    return float4(rgb, alpha);
}

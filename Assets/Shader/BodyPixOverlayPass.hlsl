#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

float3 SampleColorSRGB(uint2 pcs)
{
    float3 cs = LOAD_TEXTURE2D_X_LOD(_ColorPyramidTexture, pcs, 0).rgb;
    return FastLinearToSRGB(saturate(cs));
}

//
// Line effect
//

float4 _LineColor;
float2 _LineParams; // (threshold, contrast)

float4 LineEffect(float3 c0, uint2 pcs)
{
    // Additional color samples in sRGB
    float3 c1 = SampleColorSRGB(pcs + uint2(1, 1));
    float3 c2 = SampleColorSRGB(pcs + uint2(1, 0));
    float3 c3 = SampleColorSRGB(pcs + uint2(0, 1));

    // Edge detection (Roberts cross operator)
    float3 g1 = c1 - c0;
    float3 g2 = c3 - c2;
    float g = sqrt(dot(g1, g1) + dot(g2, g2));
    g = saturate((g - _LineParams.x) * _LineParams.y);

    return float4(_LineColor.rgb, g);
}

//
// Posterize effect
//

float4x4 _PaletteBG;
float4x4 _PaletteFG;
float3 _FillParams; // (back opacity, front opacity, dither strength)

float4 PosterizeEffect(float3 src, float alpha, uint2 pcs, float4 line_fx)
{
    // Bayer dither matrix
    const float4x4 bayer =
      float4x4(0, 8, 2, 10, 12, 4, 14, 6, 3, 11, 1, 9, 15, 7, 13, 5) / 16;

    // Luminance + dithering
    uint2 pcsm4 = pcs % 4;
    float dither = (bayer[pcsm4.x][pcsm4.y] - 0.5) * _FillParams.z;
    float lm = saturate(Luminance(src) + dither);

    // Background/foregraound gradients
    float4 bg = float4(_PaletteBG[(uint)(lm * 3.9999)].rgb, _FillParams.x);
    float4 fg = float4(_PaletteFG[(uint)(lm * 3.9999)].rgb, _FillParams.y);

    // Line blending to foregraound
    fg.rgb = lerp(fg.rgb, line_fx.rgb, line_fx.a);

    return lerp(bg, fg, alpha);
}

//
// Wiper effect
//

uint _WiperConfig;    // Random direction : Background : Foreground
float4 _WiperParams;
float4 _WiperCounts;
float4 _WiperColor;

// Wiping animation
float WiperEffect(float2 uv, float time, uint count, uint seed)
{
    seed += count * 16;

    // Start/end points
    float p1s = GenerateHashedRandomFloat(seed + 0) / 2;
    float p2s = GenerateHashedRandomFloat(seed + 1) / 2;
    float p3s = GenerateHashedRandomFloat(seed + 2) / 2;
    float p1e = GenerateHashedRandomFloat(seed + 3) / 2 + 0.5;
    float p2e = GenerateHashedRandomFloat(seed + 4) / 2 + 0.5;
    float p3e = GenerateHashedRandomFloat(seed + 5) / 2 + 0.5;

    // Animation on three points
    float y1 = smoothstep(p1s, p1e, time);
    float y2 = smoothstep(p2s, p2e, time);
    float y3 = smoothstep(p3s, p3e, time);

    // Direction randomization
    uint h = JenkinsHash(seed + 6) * (_WiperConfig & 1);
    if (h & 1) uv = 1 - uv;
    if (h & 2) uv = uv.yx;

    // Threshold on the current line
    float thresh = lerp(y1, y2, saturate(uv.y * 2));
    thresh = lerp(thresh, y3, saturate(uv.y * 2 - 1));

    // Thresholding
    return (uv.x < thresh) ^ (count & 1);
}

//
// Overlay fragment shader
//

float4 FullScreenPass(Varyings varyings) : SV_Target
{
    // Screen space coordinates (in pixel) -> UV
    uint2 pcs = varyings.positionCS.xy;
    float2 uv = (pcs + float2(0.5, 0.5)) * _ScreenSize.zw;

    // Source color sample
    float4 src = LOAD_TEXTURE2D_X_LOD(_ColorPyramidTexture, pcs, 0);
    float3 srgb = FastLinearToSRGB(src.rgb);

    // Line and posterize effect
    float4 post = PosterizeEffect(srgb, src.a, pcs, LineEffect(srgb, pcs));

    // Wiper effect x 4
    float w1 = WiperEffect(uv, _WiperParams.x, (uint)_WiperCounts.x, 0x73921);
    float w2 = WiperEffect(uv, _WiperParams.y, (uint)_WiperCounts.y, 0xf7f3a);
    float w3 = WiperEffect(uv, _WiperParams.z, (uint)_WiperCounts.z, 0x1118d);
    float w4 = WiperEffect(uv, _WiperParams.w, (uint)_WiperCounts.w, 0x32fa4);

    // Wiper effects composition
    float wiper = lerp(w1, 1 - w1, w2);
    wiper = lerp(wiper, 1 - wiper, w3);
    wiper = lerp(wiper, 1 - wiper, w4);
    wiper *= lerp(_WiperConfig & 2, _WiperConfig & 4, src.a);

    // Final composition
    float3 cout = lerp(src.rgb, FastSRGBToLinear(post.rgb), post.a);
    cout = lerp(cout, _WiperColor.rgb, wiper);
    return float4(cout, src.a);
}

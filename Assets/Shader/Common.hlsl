#ifndef __COMMON_HLSL__
#define __COMMON_HLSL__

void VertexFullScreenTriangle(uint vid : SV_VertexID,
                              out float4 outPosition : SV_Position,
                              out float2 outTexCoord : TEXCOORD)
{
    float x = (vid & 1) * 2;
    float y = (vid > 1) * 2;
    outPosition = float4(x * 2 - 1, 1 - y * 2, 1, 1);
    outTexCoord = float2(x, y);
}

#endif

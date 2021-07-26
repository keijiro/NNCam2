Shader "Hidden/NNCam2/BodyPix Filter Slitscan"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex VertexFullScreenTriangle
            #pragma fragment Fragment
            #include "BodyPixFilterSlitscan.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

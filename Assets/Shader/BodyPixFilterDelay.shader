Shader "Hidden/NNCam2/BodyPix Filter Delay"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex VertexFullScreenTriangle
            #pragma fragment Fragment
            #include "BodyPixFilterDelay.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

Shader "Hidden/NNCam2/BodyPix Filter"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex VertexFullScreenTriangle
            #pragma fragment Fragment
            #include "BodyPixFilterThru.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

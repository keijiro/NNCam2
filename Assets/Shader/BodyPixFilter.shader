Shader "Hidden/NNCam2/BodyPix Filter"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #include "BodyPixFilter.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

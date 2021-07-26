Shader "Hidden/NNCam2/BodyPix Filter Delay and Slitscan"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex VertexFullScreenTriangle
            #pragma fragment FragmentPreprocess
            #include "BodyPixFilterDelaySlitscan.hlsl"
            ENDHLSL
        }
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex VertexFullScreenTriangle
            #pragma fragment FragmentDelay
            #include "BodyPixFilterDelaySlitscan.hlsl"
            ENDHLSL
        }
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex VertexFullScreenTriangle
            #pragma fragment FragmentSlitscan
            #include "BodyPixFilterDelaySlitscan.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

Shader "Hidden/NNCam2/BodyPix Filter Feedback"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex VertexFullScreenTriangle
            #pragma fragment Fragment
            #include "BodyPixFilterFeedback.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

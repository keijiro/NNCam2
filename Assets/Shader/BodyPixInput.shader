Shader "Hidden/NNCam2/BodyPix Input"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex VertexFullScreenTriangle
            #pragma fragment Fragment
            #include "BodyPixInput.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

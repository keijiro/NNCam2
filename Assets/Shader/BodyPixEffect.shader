Shader "Hidden/NNCam2/BodyPix Effector"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #include "BodyPixEffect.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

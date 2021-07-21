Shader "Hidden/NNCam2/BodyPix Filter"
{
    HLSLINCLUDE
    #include "BodyPixFilter.hlsl"
    ENDHLSL

    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentMask
            ENDHLSL
        }
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment FragmentFilter
            ENDHLSL
        }
    }
    Fallback Off
}

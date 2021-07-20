Shader "FullScreen/NNCamRecolor"
{
    SubShader
    {
        Pass
        {
            Name "Recolor"
            Cull Off ZWrite Off ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FullScreenPass
            #include "NNCamRecolor.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

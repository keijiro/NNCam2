Shader "FullScreen/NNCam2/Recolor Pass"
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
            #include "RecolorPass.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

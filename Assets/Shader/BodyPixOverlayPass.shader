Shader "FullScreen/NNCam2/BodyPix Overlay Pass"
{
    SubShader
    {
        Pass
        {
            Name "BodyPix Overlay"
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FullScreenPass
            #include "BodyPixOverlayPass.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

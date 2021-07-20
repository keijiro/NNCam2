Shader "FullScreen/NNCamBackground"
{
    Properties
    {
        _MainTexture("Source", 2D) = ""
    }
    SubShader
    {
        Pass
        {
            Name "Background"
            Cull Off ZWrite Off ZTest LEqual
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FullScreenPass
            #include "NNCamBackground.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

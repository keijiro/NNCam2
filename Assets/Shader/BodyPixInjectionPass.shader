Shader "FullScreen/NNCam2/BodyPix Injection Pass"
{
    Properties
    {
        _MainTexture("Source", 2D) = ""
    }
    SubShader
    {
        Pass
        {
            Name "BodyPix Injection"
            Cull Off ZWrite Off ZTest LEqual
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FullScreenPass
            #include "BodyPixInjectionPass.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}

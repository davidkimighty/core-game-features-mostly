Shader "GameReady/TransitionSquare"
{
    Properties
    {
        [MainTexture] _MainTex("Main Text", 2D) = "white" {}
        _HoleSize("Hole Size", Range(0, 1)) = 1
        _EdgeSmooth("Edge Smooth", Range(0, 0.05)) = 0.005
        _Color("Overlay Color", Color) = (0, 0, 0, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Overlay"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "TransitionPass"
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half _HoleSize;
                half _EdgeSmooth;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float aspect = _ScreenParams.x / _ScreenParams.y;
                float2 centered = (input.uv - 0.5) * 2.0;
                centered.x *= aspect;

                float dist = max(abs(centered.x), abs(centered.y));
                float threshold = _HoleSize * max(aspect, 1.0);

                half alpha = smoothstep(
                    threshold - _EdgeSmooth,
                    threshold + _EdgeSmooth,
                    dist
                );
                return half4(_Color.rgb, _Color.a * alpha);
            }
            ENDHLSL
        }
    }
}
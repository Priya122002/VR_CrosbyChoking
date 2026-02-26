Shader "Custom/BlinkColorURP"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _BlinkColor ("Blink Color", Color) = (1,0,0,1)
        _BlinkSpeed ("Blink Speed", Float) = 2
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            float4 _BaseColor;
            float4 _BlinkColor;
            float _BlinkSpeed;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float blink = abs(sin(_Time.y * _BlinkSpeed));
                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                float4 finalColor = lerp(_BaseColor, _BlinkColor, blink);
                finalColor *= tex;

                return finalColor;
            }
            ENDHLSL
        }
    }
}
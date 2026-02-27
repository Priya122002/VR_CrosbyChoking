Shader "Custom/RopeFill"
{
    Properties
    {
        _GreenLight ("Light Green", Color) = (0.2, 1, 0.2, 1)
        _GreenDark ("Dark Green", Color) = (0, 0.4, 0, 1)

        _BlinkSpeed ("Blink Speed", Float) = 6
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            float4 _GreenLight;
            float4 _GreenDark;
            float _BlinkSpeed;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Hard blink (no smooth fade)
                float blink = step(0.5, frac(_Time.y * _BlinkSpeed));

                float4 finalColor = lerp(_GreenDark, _GreenLight, blink);

                return finalColor;
            }

            ENDHLSL
        }
    }
}
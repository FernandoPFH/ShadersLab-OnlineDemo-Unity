Shader "Unlit/GlassDistortion"
{
    Properties
    {
        _TintTex ("Tint Texture", 2D) = "white" {}
        _DistortionTex ("Distortion Texture", 2D) = "black" {}
        _TintStrength ("Tint Strength", Range(0,1)) = 0.65
        _DistortionStrength ("Distortion Strength", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _CameraOpaqueTexture;
            float4 _CameraOpaqueTexture_ST;
            sampler2D _TintTex;
            float4 _TintTex_ST;
            sampler2D _DistortionTex;
            float4 _DistortionTex_ST;
            float _TintStrength;
            float _DistortionStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv_dist : TEXCOORD0;
                float2 uv_tint : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 screen_pos : TEXCOORD4;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.uv_dist = TRANSFORM_TEX(v.uv, _DistortionTex);
                o.uv_tint = TRANSFORM_TEX(v.uv, _TintTex);
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.screen_pos = ComputeScreenPos(o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Calcula screen UV (NDC -> 0~1)
                float2 screenUV = i.screen_pos.xy / i.screen_pos.w;

                float4 offset = tex2D(_DistortionTex, i.uv_dist);
                float4 tint = tex2D(_TintTex, i.uv_tint);

                // // Aplica distorção
                screenUV += (offset.rg * 2 - 1) * _DistortionStrength;

                // Amostra a cor da cena atrás do vidro
                float4 sceneColor = tex2D(_CameraOpaqueTexture, screenUV);

                sceneColor.rgb *= tint.rgb * _TintStrength;

                return sceneColor;
            }
            ENDHLSL
        }
    }
}

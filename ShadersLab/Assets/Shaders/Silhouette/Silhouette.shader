Shader "Unlit/Silhouette"
{
    Properties
    {
        _BackgroundColor ("Background Color", Color) = (1,1,1,1)
        _ForegroundColor ("Foreground Color", Color) = (0,0,0,1)
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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            float4 _BackgroundColor;
            float4 _ForegroundColor;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screen_pos : TEXCOORD4;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.screen_pos = ComputeScreenPos(o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Calcula screen UV (NDC -> 0~1)
                float2 screenUV = i.screen_pos.xy / i.screen_pos.w;

                // Amostra a cor da cena atrás do vidro
                float depth = Linear01Depth(SampleSceneDepth(screenUV), _ZBufferParams);

                return lerp(_ForegroundColor,_BackgroundColor,depth);
            }
            ENDHLSL
        }
    }
}

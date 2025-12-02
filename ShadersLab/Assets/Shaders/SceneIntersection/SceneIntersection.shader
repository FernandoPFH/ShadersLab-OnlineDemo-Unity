Shader "Unlit/SceneIntersection"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0,0,0,1)
        _IntersectionColor ("Intersection Color", Color) = (0,0.5,1,1)
        _IntersectionPower ("Intersection Power", Range(1,10)) = 6.5
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

            float4 _BaseColor;
            float4 _IntersectionColor;
            float _IntersectionPower;

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

                float depth = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);

                float intersection = saturate(1 - (depth - i.screen_pos.w));

                return lerp(_BaseColor,_IntersectionColor,pow(intersection,_IntersectionPower));
            }
            ENDHLSL
        }
    }
}

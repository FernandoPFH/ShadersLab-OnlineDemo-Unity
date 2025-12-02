Shader "Unlit/ShadowCasting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"

        Pass
        {
            Name "Shadow Map Texture"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 shadowCoord : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.shadowCoord = GetShadowCoord(GetVertexPositionInputs(v.vertex.xyz));
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                Light light = GetMainLight(i.shadowCoord);
                float3 shadow = light.shadowAttenuation;

                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);

                col.rgb *= shadow;

                return col;
            }
            ENDHLSL
        }
    }
}

Shader "Lit/SpecularReflection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SpecularTex ("Specular Texture", 2D) = "black" {}
        _SpecularIntensity ("Specular Intensity", Range(0,1)) = 1
        _SpecularPower ("Specular Power", Range(1,128)) = 64
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            TEXTURE2D(_SpecularTex);
            SAMPLER(sampler_SpecularTex);
            float4 _SpecularTex_ST;
            float _SpecularIntensity;
            float _SpecularPower;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal_world : TEXCOORD1;
                float3 vertex_world : TEXCOORD2;
            };

            float3 SpecularShading(float3 colorRefl, float specularInt, float3 normal, float3 lightDir, float3 viewDir, float specularPow)
            {
                float3 h = normalize(lightDir + viewDir);

                return colorRefl * specularInt * pow(max(0,dot(normal,h)), specularPow);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal_world = TransformObjectToWorldNormal(v.normal);
                o.vertex_world = TransformObjectToWorld(v.vertex.xyz);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                Light mainLight = GetMainLight();

                float3 specCol = SAMPLE_TEXTURE2D(_SpecularTex, sampler_SpecularTex, i.uv).rgb * mainLight.color;

                float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex_world);

                float3 specular = SpecularShading(
                    specCol,
                    _SpecularIntensity,
                    normalize(i.normal_world),
                    mainLight.direction,
                    viewDir,
                    _SpecularPower
                );

                col.rgb += specular;

                return col;
            }
            ENDHLSL
        }
    }
}

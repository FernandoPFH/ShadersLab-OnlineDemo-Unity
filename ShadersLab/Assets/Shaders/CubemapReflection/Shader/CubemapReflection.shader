Shader "Lit/CubemapReflection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle(_UseSkyboxCubemap)] _UseSkyboxCubemap ("Use Skybox Cubemap?", Float) = 0
        _ReflectionTex ("Reflection Texture", Cube) = "white" {}
        _ReflectionIntensity ("Reflection Intensity", Range(0,1)) = 1
        _ReflectionMetallic ("Reflection Metallic", Range(0,1)) = 0
        _ReflectionDetail ("Reflection Detail", Range(1,9)) = 1
        _ReflectionExposure ("Reflection Exposure", Range(1,3)) = 1
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

            #pragma multi_compile _UseSkyboxCubemap _

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            samplerCUBE _ReflectionTex;
            float _ReflectionIntensity;
            half _ReflectionMetallic;
            float _ReflectionDetail;
            float _ReflectionExposure;

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

            float3 CubemapReflection(samplerCUBE cubemapTex, float reflectionInt, half reflectionDet, float3 normal, float3 viewDir, float reflectionExp)
            {
                float3 reflection_world = reflect(viewDir,normal);
                float4 cubemap = texCUBElod(cubemapTex, float4(reflection_world,reflectionDet));

                return reflectionInt * cubemap.rgb * (cubemap.a * reflectionExp);
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
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex_world);

                #ifdef _UseSkyboxCubemap
                    half3 reflect_world = reflect(-viewDir,i.normal_world);
                    half4 reflectionData = SAMPLE_TEXTURECUBE(unity_SpecCube0, samplerunity_SpecCube0, reflect_world);
                    half3 reflectionColor = DecodeHDREnvironment(reflectionData, unity_SpecCube0_HDR);

                    // col.rgb = lerp(col,reflectionColor,_ReflectionMetallic);
                    col.rgb *= reflectionColor + _ReflectionMetallic;
                #else
                    float3 reflection = CubemapReflection(
                        _ReflectionTex,
                        _ReflectionIntensity,
                        _ReflectionDetail,
                        normalize(i.normal_world),
                        -viewDir,
                        _ReflectionExposure
                    );

                    col.rgb *= reflection + _ReflectionMetallic;
                #endif

                return col;
            }
            ENDHLSL
        }
    }
}

Shader "Lit/Gooch"
{
    Properties
    {
        _SpecularIntensity ("Specular Intensity", Range(0,1)) = 1
        _SpecularPower ("Specular Power", Range(1,128)) = 64
        _Albedo ("Albedo", Color) = (0.8,0,0.2,1)
        _Blue ("Blue Tone", Range(0,1)) = 0.4
        _Yellow ("Yellow Tone", Range(0,1)) = 0.4
        _Beta ("Beta Factor (Warm)", Range(0,1)) = 0.6
        _Alpha ("Alpha Factor (Cool)", Range(0,1)) = 0.2
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

            float _SpecularIntensity;
            float _SpecularPower;
            float4 _Albedo;
            float _Blue;
            float _Yellow;
            float _Beta;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal_world : TEXCOORD1;
                float3 vertex_world : TEXCOORD2;
            };

            float LambertShadingUnclamped(float3 normal, float3 lightDir)
            {
                return dot(normal,lightDir);
            }

            float3 SpecularShading(float3 colorRefl, float specularInt, float3 normal, float3 lightDir, float3 viewDir, float specularPow)
            {
                float3 h = normalize(lightDir + viewDir);

                return colorRefl * specularInt * pow(max(0,dot(normal,h)), specularPow);
            }

            float3 GoochDiffuse(float blueTone, float yellowTone, float alphaFactor, float betaFactor, float3 albedoColor, float diffuse)
            {
                float3 KBlue = float3(0,0,blueTone);
                float3 KYellow = float3(yellowTone,yellowTone,0);

                float3 KCool = KBlue + alphaFactor * albedoColor;
                float3 KWarm = KYellow + betaFactor * albedoColor;

                float gooch = (1 + diffuse) / 2;

                return gooch * KWarm + (1 - gooch) * KCool;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.normal_world = TransformObjectToWorldNormal(v.normal);
                o.vertex_world = mul(unity_ObjectToWorld,v.vertex).xyz;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                Light mainLight = GetMainLight();
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex_world);

                float diffuse = LambertShadingUnclamped(
                    normalize(i.normal_world),
                    mainLight.direction
                );

                float3 goochDiffuse = GoochDiffuse(
                    _Blue,
                    _Yellow,
                    _Alpha,
                    _Beta,
                    _Albedo.rgb,
                    diffuse
                );

                float3 specular = SpecularShading(
                    mainLight.color,
                    _SpecularIntensity,
                    normalize(i.normal_world),
                    mainLight.direction,
                    viewDir,
                    _SpecularPower
                );

                return float4(goochDiffuse + specular,1);
            }
            ENDHLSL
        }
    }
}

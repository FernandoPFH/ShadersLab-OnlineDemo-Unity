Shader "Unlit/DragonBall"
{
    Properties
    {
        [Header(Reflection Settings)]
        _ReflectionMetallic ("Reflection Metallic", Range(0,1)) = 0.86
        [Header(Fresnel Settings)]
        _FresnelColor ("Fresnel Color", Color) = (1,0.62,0,1)
        _FresnelPower ("Fresnel Power", Range(1,5)) = 1.5
        _FresnelIntensity ("Fresnel Intensity", Range(0,1)) = 1
        [Header(Specular Settings)]
        _SpecularIntensity ("Specular Intensity", Range(0,1)) = 0.77
        _SpecularPower ("Specular Power", Range(1,128)) = 128
        [Header(Star Settings)]
        _StarColor ("Star Color", Color) = (0.85,0.17,0.17,1)
        _StarRadius ("Star Radius", Range(0,1)) = 0.11
        [Header(Stars Offsets)]
        [KeywordEnum(One,Two,Three,Four,Five,Six,Seven)]
        _StarNumbers ("Star Numbers", Float) = 0
        [Header(2 Stars)]
        _Stars2Offset1 ("1 Star", Vector) = (0.12,-0.12,0,0)
        _Stars2Offset2 ("2 Star", Vector) = (-0.12,0.12,0,0)
        [Header(3 Stars)]
        _Stars3Offset1 ("1 Star", Vector) = (0,-0.12,0,0)
        _Stars3Offset2 ("2 Star", Vector) = (-0.12,0.12,0,0)
        _Stars3Offset3 ("3 Star", Vector) = (0.12,0.12,0,0)
        [Header(4 Stars)]
        _Stars4Offset1 ("1 Star", Vector) = (0.12,-0.12,0,0)
        _Stars4Offset2 ("2 Star", Vector) = (-0.12,0.12,0,0)
        _Stars4Offset3 ("3 Star", Vector) = (-0.12,0.12,0,0)
        _Stars4Offset4 ("4 Star", Vector) = (0.12,0.12,0,0)
        [Header(5 Stars)]
        _Stars5Offset1 ("1 Star", Vector) = (0.24,-0.04,0,0)
        _Stars5Offset2 ("2 Star", Vector) = (0,-0.24,0,0)
        _Stars5Offset3 ("3 Star", Vector) = (-0.24,-0.04,0,0)
        _Stars5Offset4 ("4 Star", Vector) = (-0.12,0.24,0,0)
        _Stars5Offset5 ("5 Star", Vector) = (0.12,0.24,0,0)
        [Header(6 Stars)]
        _Stars6Offset1 ("1 Star", Vector) = (0,0,0,0)
        _Stars6Offset2 ("2 Star", Vector) = (0,-0.24,0,0)
        _Stars6Offset3 ("3 Star", Vector) = (0.24,-0.04,0,0)
        _Stars6Offset4 ("4 Star", Vector) = (-0.24,-0.04,0,0)
        _Stars6Offset5 ("5 Star", Vector) = (-0.12,0.24,0,0)
        _Stars6Offset6 ("6 Star", Vector) = (0.12,0.24,0,0)
        [Header(7 Stars)]
        _Stars7Offset1 ("1 Star", Vector) = (0,0,0,0)
        _Stars7Offset2 ("2 Star", Vector) = (0.24,0,0,0)
        _Stars7Offset3 ("3 Star", Vector) = (0.12,-0.24,0,0)
        _Stars7Offset4 ("4 Star", Vector) = (-0.12,-0.24,0,0)
        _Stars7Offset5 ("5 Star", Vector) = (-0.24,0,0,0)
        _Stars7Offset6 ("6 Star", Vector) = (-0.12,0.24,0,0)
        _Stars7Offset7 ("7 Star", Vector) = (0.12,0.24,0,0)
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

            half _ReflectionMetallic;
            float4 _FresnelColor;
            float _FresnelPower;
            float _FresnelIntensity;
            float4 _StarColor;
            float _StarRadius;
            float _SpecularIntensity;
            float _SpecularPower;

            float _StarNumbers;

            float4 _Stars2Offset1;
            float4 _Stars2Offset2;
            
            float4 _Stars3Offset1;
            float4 _Stars3Offset2;
            float4 _Stars3Offset3;
            
            float4 _Stars4Offset1;
            float4 _Stars4Offset2;
            float4 _Stars4Offset3;
            float4 _Stars4Offset4;
            
            float4 _Stars5Offset1;
            float4 _Stars5Offset2;
            float4 _Stars5Offset3;
            float4 _Stars5Offset4;
            float4 _Stars5Offset5;
            
            float4 _Stars6Offset1;
            float4 _Stars6Offset2;
            float4 _Stars6Offset3;
            float4 _Stars6Offset4;
            float4 _Stars6Offset5;
            float4 _Stars6Offset6;
            
            float4 _Stars7Offset1;
            float4 _Stars7Offset2;
            float4 _Stars7Offset3;
            float4 _Stars7Offset4;
            float4 _Stars7Offset5;
            float4 _Stars7Offset6;
            float4 _Stars7Offset7;

            float3 SpecularShading(float3 colorRefl, float specularInt, float3 normal, float3 lightDir, float3 viewDir, float specularPow)
            {
                float3 h = normalize(lightDir + viewDir);

                return colorRefl * specularInt * pow(max(0,dot(normal,h)), specularPow);
            }

            void GenerateTangentBitangent_float(float3 planeNormal, out float3 tangent, out float3 bitangent)
            {
                // Gere dois vetores ortogonais ao planoNormal
                tangent = normalize(cross(planeNormal, float3(0,1,0)));
                if (length(tangent) < 0.01) // caso o plano esteja apontando para cima
                    tangent = normalize(cross(planeNormal, float3(1,0,0)));

                bitangent = normalize(cross(planeNormal, tangent));
            }

            float3 RayPlaneIntersection(float3 P0, float3 V, float3 Pp, float3 N)
            {
                float denom = dot(V, N);
                if (abs(denom) < 1e-6)
                    return float3(1000, 1000, 1000); // Sem interseção ou paralelo

                float t = dot(Pp - P0, N) / denom;
                return P0 + t * V;
            }

            float sdfPentagram(float2 uv, float radius)
            {
                float3 k1 = float3(0.81,0.59,0.73);
                float2 k2 = float2(0.31,0.95);
                float2 v1 = k1.xy * float2(1,-1);
                float2 v2 = -k1.xy;
                float2 v3 = k2 * float2(1,-1);

                float2 pos = float2(abs(uv.x),uv.y);
                pos -= 2*max(dot(v1,pos),0)*v1;
                pos -= 2*max(dot(v2,pos),0)*v2;
                pos.x = abs(pos.x);
                pos.y -= radius;
                return length(pos-v3*clamp(dot(pos,v3),0,k1.z*radius)) * sign(pos.y*v3.x-pos.x*v3.y);
            }

            float CreateStarMask(float2 uv_proj)
            {
                float starMask;

                if (_StarNumbers == 0) {
                    float2 offset = float2(0, 0);
                    float sdf = sdfPentagram(uv_proj + offset, _StarRadius);
                    starMask = sdf <= 0;
                }
                else if (_StarNumbers == 1) {
                    float sdf1 = sdfPentagram(uv_proj + _Stars2Offset1.xy, _StarRadius);
                    float sdf2 = sdfPentagram(uv_proj + _Stars2Offset2.xy, _StarRadius);
                    starMask = (sdf1 <= 0) + (sdf2 <= 0);
                }
                else if (_StarNumbers == 2) {
                    float sdf1 = sdfPentagram(uv_proj + _Stars3Offset1.xy, _StarRadius);
                    float sdf2 = sdfPentagram(uv_proj + _Stars3Offset2.xy, _StarRadius);
                    float sdf3 = sdfPentagram(uv_proj + _Stars3Offset3.xy, _StarRadius);
                    starMask = (sdf1 <= 0) + (sdf2 <= 0) + (sdf3 <= 0);
                }
                else if (_StarNumbers == 3) {
                    float sdf1 = sdfPentagram(uv_proj + _Stars4Offset1.xy, _StarRadius);
                    float sdf2 = sdfPentagram(uv_proj + _Stars4Offset2.xy, _StarRadius);
                    float sdf3 = sdfPentagram(uv_proj + _Stars4Offset3.xy, _StarRadius);
                    float sdf4 = sdfPentagram(uv_proj + _Stars4Offset4.xy, _StarRadius);
                    starMask = (sdf1 <= 0) + (sdf2 <= 0) + (sdf3 <= 0) + (sdf4 <= 0);
                }
                else if (_StarNumbers == 4) {
                    float sdf1 = sdfPentagram(uv_proj + _Stars5Offset1.xy, _StarRadius);
                    float sdf2 = sdfPentagram(uv_proj + _Stars5Offset2.xy, _StarRadius);
                    float sdf3 = sdfPentagram(uv_proj + _Stars5Offset3.xy, _StarRadius);
                    float sdf4 = sdfPentagram(uv_proj + _Stars5Offset4.xy, _StarRadius);
                    float sdf5 = sdfPentagram(uv_proj + _Stars5Offset5.xy, _StarRadius);
                    starMask = (sdf1 <= 0) + (sdf2 <= 0) + (sdf3 <= 0) + (sdf4 <= 0) + (sdf5 <= 0);
                }
                else if (_StarNumbers == 5) {
                    float sdf1 = sdfPentagram(uv_proj + _Stars6Offset1.xy, _StarRadius);
                    float sdf2 = sdfPentagram(uv_proj + _Stars6Offset2.xy, _StarRadius);
                    float sdf3 = sdfPentagram(uv_proj + _Stars6Offset3.xy, _StarRadius);
                    float sdf4 = sdfPentagram(uv_proj + _Stars6Offset4.xy, _StarRadius);
                    float sdf5 = sdfPentagram(uv_proj + _Stars6Offset5.xy, _StarRadius);
                    float sdf6 = sdfPentagram(uv_proj + _Stars6Offset6.xy, _StarRadius);
                    starMask = (sdf1 <= 0) + (sdf2 <= 0) + (sdf3 <= 0) + (sdf4 <= 0) + (sdf5 <= 0) + (sdf6 <= 0);
                }
                else if (_StarNumbers == 6) {
                    float sdf1 = sdfPentagram(uv_proj + _Stars7Offset1.xy, _StarRadius);
                    float sdf2 = sdfPentagram(uv_proj + _Stars7Offset2.xy, _StarRadius);
                    float sdf3 = sdfPentagram(uv_proj + _Stars7Offset3.xy, _StarRadius);
                    float sdf4 = sdfPentagram(uv_proj + _Stars7Offset4.xy, _StarRadius);
                    float sdf5 = sdfPentagram(uv_proj + _Stars7Offset5.xy, _StarRadius);
                    float sdf6 = sdfPentagram(uv_proj + _Stars7Offset6.xy, _StarRadius);
                    float sdf7 = sdfPentagram(uv_proj + _Stars7Offset7.xy, _StarRadius);
                    starMask = (sdf1 <= 0) + (sdf2 <= 0) + (sdf3 <= 0) + (sdf4 <= 0) + (sdf5 <= 0) + (sdf6 <= 0) + (sdf7 <= 0);
                }

                return saturate(starMask);
            }

            float FresnelEffect(float3 normal, float3 viewDir, float fresnelPow)
            {
                return pow((1 - saturate(dot(normal,viewDir))),fresnelPow);
            }

            float3 CubemapReflection(samplerCUBE cubemapTex, float reflectionInt, half reflectionDet, float3 normal, float3 viewDir, float reflectionExp)
            {
                float3 reflection_world = reflect(viewDir,normal);
                float4 cubemap = texCUBElod(cubemapTex, float4(reflection_world,reflectionDet));

                return reflectionInt * cubemap.rgb * (cubemap.a * reflectionExp);
            }

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
                float3 vertex_object : TEXCOORD3;
                float3 centerPos_world : TEXCOORD4;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                o.normal_world = TransformObjectToWorldNormal(v.normal);
                o.vertex_world = TransformObjectToWorld(v.vertex.xyz);
                o.vertex_object = v.vertex.xyz;
                o.centerPos_world = mul(unity_ObjectToWorld,float4(0,0,0,1)).xyz;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = 0;

                Light mainLight = GetMainLight();
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex_world);

                half3 reflect_world = reflect(viewDir,-i.normal_world);
                half4 reflectionData = SAMPLE_TEXTURECUBE(unity_SpecCube0, samplerunity_SpecCube0, reflect_world);
                half3 reflectionColor = DecodeHDREnvironment(reflectionData, unity_SpecCube0_HDR);

                col.rgb = lerp(_FresnelColor.rgb,reflectionColor,_ReflectionMetallic);

                float fresnel = FresnelEffect(
                    i.normal_world,
                    viewDir,
                    _FresnelPower
                );

                col.rgb = lerp(col.rgb,_FresnelColor.rgb,fresnel * _FresnelIntensity);

                float3 planeNormal = normalize(_WorldSpaceCameraPos - i.centerPos_world);

                float3 intersection = RayPlaneIntersection(
                    _WorldSpaceCameraPos,
                    normalize(i.vertex_world-_WorldSpaceCameraPos),
                    i.centerPos_world,
                    planeNormal
                );

                float3 tangent, bitangent;
                GenerateTangentBitangent_float(
                    planeNormal,
                    tangent,
                    bitangent
                );

                float3 relative = intersection - i.centerPos_world;

                float2 uv_proj = float2(dot(relative, tangent), -dot(relative, bitangent));

                float starMask = CreateStarMask(uv_proj);

                col.rgb = lerp(col.rgb,_StarColor.rgb,starMask);

                float3 specular = SpecularShading(
                    mainLight.color,
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

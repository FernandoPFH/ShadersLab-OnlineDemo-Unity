Shader "Lit/HalftoneEffect"
{
    Properties
    {
        _BaseColor ("Base Color",Color) = (1,1,1,1)
        _Rotation ("Rotation", Range(0,360)) = 0
        _CellDensity ("Cell Density", Range(1,20)) = 5
        _LightIntensity ("Light Intensity", Range(0,1)) = 1
        _LightSoftness ("Light Softness", Range(0,1)) = 1
        _LightUnlitThreshold ("Light Unlit Threshold", Range(-1,1)) = 0.1
        _LightLitThreshold ("Light Lit Threshold", Range(-1,1)) = 0.9
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

            float4 _BaseColor;
            float _Rotation;
            float _CellDensity;
            float _LightIntensity;
            float _LightSoftness;
            float _LightUnlitThreshold;
            float _LightLitThreshold;

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
            };

            void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
            {
                Rotation = Rotation * (3.1415926f/180.0f);
                UV -= Center;
                float s = sin(Rotation);
                float c = cos(Rotation);
                float2x2 rMatrix = float2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix * 2 - 1;
                UV.xy = mul(UV.xy, rMatrix);
                UV += Center;
                Out = UV;
            }

            float map(float x, float in_min, float in_max, float out_min, float out_max) {
                return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
            }

            inline float2 unity_voronoi_noise_randomVector (float2 UV, float offset)
            {
                float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                UV = frac(sin(mul(UV, m)) * 46839.32);
                return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
            }

            void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
            {
                float2 g = floor(UV * CellDensity);
                float2 f = frac(UV * CellDensity);
                float t = 8.0;
                float3 res = float3(8.0, 0.0, 0.0);

                for(int y=-1; y<=1; y++)
                {
                    for(int x=-1; x<=1; x++)
                    {
                        float2 lattice = float2(x,y);
                        float2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
                        float d = distance(lattice + offset, f);
                        if(d < res.x)
                        {
                            res = float3(d, offset.x, offset.y);
                            Out = res.x;
                            Cells = res.y;
                        }
                    }
                }
            }

            float3 LambertShading(float3 colorRefl, float lightInt, float3 normal, float3 lightDir,out float dotResult)
            {
                dotResult = max(0, dot(normal,lightDir));
                return colorRefl * lightInt * dotResult;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                o.normal_world = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = _BaseColor;

                Light mainLight = GetMainLight();

                float diffuseDotResult = 0;

                float3 diffuse = LambertShading(
                    mainLight.color,
                    _LightIntensity,
                    normalize(i.normal_world),
                    mainLight.direction,
                    diffuseDotResult
                );

                float2 changedUV = 0;

                Unity_Rotate_Degrees_float(i.uv*_CellDensity, float2(0.5,0.5), _Rotation, changedUV);

                float Out = 0;
                float Cells = 0;
                Unity_Voronoi_float(changedUV,0,5,Out,Cells);

                float diffuseRemap = map(-diffuseDotResult,-1,1,_LightUnlitThreshold,_LightLitThreshold);

                col.rgb *= smoothstep(diffuseRemap,diffuseRemap+_LightSoftness,Out);

                return col;
            }
            ENDHLSL
        } 
    }
}

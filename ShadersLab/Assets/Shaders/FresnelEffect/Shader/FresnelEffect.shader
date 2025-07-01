Shader "Unlit/FresnelEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelPower ("Fresnel Power", Range(1,5)) = 1
        _FresnelIntensity ("Fresnel Intensity", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal_world : TEXCOORD1;
                float3 vertex_world : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FresnelColor;
            float _FresnelPower;
            float _FresnelIntensity;

            float FresnelEffect(float3 normal, float3 viewDir, float fresnelPow)
            {
                return pow((1 - saturate(dot(normal,viewDir))),fresnelPow);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal_world = UnityObjectToWorldNormal(v.normal);
                o.vertex_world = mul(unity_ObjectToWorld,v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex_world);

                float fresnel = FresnelEffect(
                    i.normal_world,
                    viewDir,
                    _FresnelPower
                );

                col.rgb += fresnel * _FresnelIntensity * _FresnelColor.rgb;

                return col;
            }
            ENDCG
        }
    }
}

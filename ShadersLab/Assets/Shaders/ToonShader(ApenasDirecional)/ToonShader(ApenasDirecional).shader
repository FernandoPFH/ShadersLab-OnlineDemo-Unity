Shader "Unlit/ToonShader(ApenasDirecional)"
{
    Properties
    {
        _Color ("Color", Color) = (0.24,0.65,0.97,1)
        _Threshold ("Limite", Range(-1,1)) = 0
        _Min ("Minimo", Range(0,1)) = 0.02
        _Max ("Maximo", Range(0,1)) = 0.8
        [MaterialToggle] _Inverted ("Invertido?", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags {
                "PassFlags" = "OnlyDirectional"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float3 worldNormal : NORMAL;
            };

            float4 _Color;
            float _Threshold;
            float _Min;
            float _Max;
            Float _Inverted;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(_WorldSpaceLightPos0, normal);

                float lightIntensity = NdotL * (!_Inverted) + NdotL * (-1 * _Inverted) > _Threshold ? _Max : _Min;

                return _Color * _LightColor0 * lightIntensity;
            }
            ENDCG
        }
    }
}

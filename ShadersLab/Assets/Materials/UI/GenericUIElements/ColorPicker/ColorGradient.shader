Shader "Unlit/ColorGradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Hue ("Hue",Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Hue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float3 HSVtoRGB(float3 hsv)
            {
                float H = hsv.x;
                float S = hsv.y;
                float V = hsv.z;

                float4 K = float4(1.0, 2.0/3.0, 1.0/3.0, 3.0);

                float3 p = abs(frac(H + K.xyz) * 6.0 - K.www);
                return V * lerp(K.xxx, saturate(p - K.xxx), S);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float3 HSV = float3(_Hue,i.uv);

                return float4(HSVtoRGB(HSV),1) * tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}

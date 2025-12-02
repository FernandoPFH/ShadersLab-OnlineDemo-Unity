Shader "Unlit/Holograma"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Sections ("Sections", Range(2,15)) = 12
        _Velocity ("Velocity", Range(-0.5,0.5)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
            float _Sections;
            float _Velocity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float animationOffset = _Time.y * _Velocity;
                float tanAlpha = clamp(0,abs(tan((i.uv.y + animationOffset) * _Sections)),1);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return fixed4(col.rgb,tanAlpha);
            }
            ENDCG
        }
    }
}

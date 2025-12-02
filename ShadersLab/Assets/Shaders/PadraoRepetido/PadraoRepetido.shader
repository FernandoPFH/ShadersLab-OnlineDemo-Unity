Shader "Unlit/PadraoRepetido"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [IntRange] _RepetitionsX ("Repetitions X",Range(1,10)) = 2
        [IntRange] _RepetitionsY ("Repetitions Y",Range(1,10)) = 2
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _RepetitionsX;
            int _RepetitionsY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, frac(float2(i.uv.x*_RepetitionsX,i.uv.y*_RepetitionsY)));
                return col;
            }
            ENDCG
        }
    }
}

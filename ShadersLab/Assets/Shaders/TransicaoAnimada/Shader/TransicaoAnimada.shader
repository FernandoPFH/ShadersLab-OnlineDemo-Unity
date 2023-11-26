Shader "Unlit/TransicaoAnimada"
{
    Properties
    {
        _MainTex ("Textura Transição", 2D) = "white" {}
        _color ("Cor Transição", Color) = (0,0,0,1)
        _posicao ("Posição Transição", Range(0,1.01)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _color;
            float _posicao;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex,i.uv);

                return lerp(
                    fixed4(0,0,0,0),
                    _color,
                    (col.x+col.y+col.z)/3<_posicao
                );
            }
            ENDCG
        }
    }
}

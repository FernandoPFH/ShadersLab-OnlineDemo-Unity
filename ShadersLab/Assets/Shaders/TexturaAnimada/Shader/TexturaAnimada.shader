Shader "Unlit/AnimacaoTextura"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VelocidadeMovimento_X ("VelocidadeMovimentoX", float) = 1
        _VelocidadeMovimento_Y ("VelocidadeMovimentoY", float) = 1
        _VelocidadeEscala_X ("VelocidadeEscalaX", float) = 1
        _VelocidadeEscala_Y ("VelocidadeEscalaY", float) = 1
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

            // Informações passadas para a função de vértices
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Informações passadas da função de vértices (vert) para a função de fragmentos (frag)
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _VelocidadeMovimento_X;
            float _VelocidadeMovimento_Y;
            float _VelocidadeEscala_X;
            float _VelocidadeEscala_Y;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Aplica a escala e offset da textura
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.uv.x *= .5 + sin(_Time.y * _VelocidadeEscala_X);
                o.uv.y *= .5 + sin(_Time.y * _VelocidadeEscala_Y);

                o.uv.x += _VelocidadeMovimento_X * _Time.y;
                o.uv.y += _VelocidadeMovimento_Y * _Time.y;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}

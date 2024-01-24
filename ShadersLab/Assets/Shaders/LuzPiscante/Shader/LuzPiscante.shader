Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MaskTex ("Textura Mascara", 2D) = "white" {}
        [HDR] _Cor ("Cor Luz", Color) = (1,1,1,1)
        _Vel ("Velocidade Luz", float) = 1
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

            sampler2D _MaskTex;
            fixed4 _Cor;
            float _Vel;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Converte o pixel da textura em um valor de [0,1]
                fixed4 color = tex2D(_MaskTex, i.uv);
                float grey = (color.r + color.g + color.b) /3;

                // Se o valor for maior que 0,5, terá uma cor que vai ocilar sua intensidade, se não ficará escuro
                return lerp(
                        fixed4(0,0,0,1),
                        _Cor * lerp(1,15,sin(_Time*_Vel)/2+.5),
                        step(0.5,grey)
                    );
            }
            ENDCG
        }
    }
}

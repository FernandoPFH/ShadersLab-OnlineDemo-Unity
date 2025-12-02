Shader "Unlit/Pulsante"
{
    Properties
    {
        [HDR] _Cor ("Cor", Color) = (0.900341034,0,4,1) // Permite cores em HDR
        _Amplitude ("Amplitude", float) = 0.1
        _Velocidade ("Velocidade", float) = 150.0
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
            };

            // Informações passadas da função de vértices (vert) para a função de fragmentos (frag)
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            fixed4 _Cor;
            float _Amplitude;
            float _Velocidade;

            v2f vert (appdata v)
            {
                v2f o;

                float relacaoAmplitudeTamanho = _Amplitude/length(v.vertex);
                float mudancaNoTempo = (sin(_Time.x * _Velocidade) - 0.5);

                float4 vertexModificado = v.vertex * (1 + relacaoAmplitudeTamanho * mudancaNoTempo);
                o.vertex = UnityObjectToClipPos(vertexModificado);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float mudancaNoTempo = sin(_Time.x * _Velocidade);
                return _Cor * mudancaNoTempo;
            }
            ENDCG
        }
    }
}
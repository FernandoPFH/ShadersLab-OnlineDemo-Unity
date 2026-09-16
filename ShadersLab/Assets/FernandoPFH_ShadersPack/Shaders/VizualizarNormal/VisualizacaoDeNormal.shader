Shader "Unlit/VisualizacaoDeNormal"
{
    Properties
    {
        [MaterialToggle] _temValoresNegativos("temValoresNegativos", Float) = 0 // MaterialToggle transforma em um check, ou é 1 ou é 0
        [MaterialToggle] _normalModoLocal("normalModoLocal", Float) = 0 // MaterialToggle transforma em um check, ou é 1 ou é 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Informações passadas para a função de vértices
            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
            };

            // Informações passadas da função de vértices (vert) para a função de fragmentos (frag)
            struct v2f
            {
                float4 vertex : SV_POSITION;
                half3 normal : TEXTCOORD0;
            };

            sampler2D _MainTex;
            Float _temValoresNegativos;
            Float _normalModoLocal;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);

                // Repassa o vetor normal do calculo de vértices para o de pixel
                // O vetor está relativo ao objeto, a função TransformObjectToWorldNormal tranforma em relativo ao mundo
                o.normal = lerp(
                    v.normal, // Valor se for falso
                    TransformObjectToWorldNormal(v.normal), // Valor se for verdadeiro
                    _normalModoLocal // Variavel de controle
                );

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 color = 0;

                // Transforma o valor de normal para um vetor de cor
                // O normal vai de [-1|1], a parte negativa é representada pela cor preta
                color.rgb = lerp(
                    i.normal, // Valor se for falso
                    i.normal * 0.5 + 0.5, // Valor se for verdadeiro
                    _temValoresNegativos // Variavel de controle
                );

                return color;
            }
            ENDHLSL
        }
    }
}

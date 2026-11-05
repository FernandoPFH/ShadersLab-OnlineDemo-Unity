Shader "ShaderLab/Paper Rolling"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Range(0.01,0.5)) = 0.5
        _Ref ("Ref", Range(0,1)) = 0
        _ObjectSize ("Object Size", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define PI 3.14159265359

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
            float _Radius;
            float _Ref;
            float _ObjectSize;

            v2f vert (appdata v)
            {
                // _Ref 1 = Enrolado
                // _Ref 0 = Reto

                float diameter = _Radius * 2;

                float mappedScale = (v.vertex.y + (_ObjectSize / 2))/_ObjectSize;

                float refRealY = (_Ref * _ObjectSize) - (_ObjectSize / 2);

                float raio = (_ObjectSize * diameter) / (2 * PI);

                float freq = 1 / diameter;

                float ref = (mappedScale - _Ref) * freq;
                float refRad = ref * 2 * PI;

                float centerDecay = 1 - (_Ref - mappedScale)*(1 - diameter);
                
                float y = refRealY + sin(refRad)*raio*centerDecay;
                float z = cos(refRad)*raio*centerDecay - raio;

                float4 vertex = (mappedScale <= _Ref) * float4(v.vertex.x,y,z,1) + (mappedScale > _Ref) * v.vertex;

                v2f o;
                o.vertex = UnityObjectToClipPos(vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
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

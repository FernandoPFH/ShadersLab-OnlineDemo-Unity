Shader "Unlit/ChromaticAberration"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AmplitudeX ("Amplitude X", Range(0,1)) = 0.2
        _AmplitudeY ("Amplitude Y", Range(0,1)) = 0
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
            float _AmplitudeX;
            float _AmplitudeY;

            v2f vert (appdata v)  
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 amplitude = pow(abs(float2(_AmplitudeX, _AmplitudeY) * _SinTime.w),2);

                fixed4 currentRedArtifact = tex2D(_MainTex, i.uv + amplitude);
                fixed4 currentCol = tex2D(_MainTex, i.uv);
                fixed4 currentBlueArtifact = tex2D(_MainTex, i.uv - amplitude);

                return fixed4(currentRedArtifact.r, currentCol.g, currentBlueArtifact.b,1);
            }
            ENDCG
        }
    }
}

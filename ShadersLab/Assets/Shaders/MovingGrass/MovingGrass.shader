Shader "ShaderLab/Moving Grass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	_Color ("Color", Color) = (0,1,0,1)
	_MovingVelocity ("Moving Velocity", float) = 0.2
	_YOffset ("Y Offset", float) = -1
	_XAmplitude ("X Amplitude", float) = 0.5
	_ZAmplitude ("Z Amplitude", float) = 0.5
	_Rigidness ("Rigidness", float) = 0.5
    }
    SubShader
    {
        Tags { 
	    "RenderType" = "Transparent"
	    "Queue" = "Transparent"
	}

        Pass
        {
	    Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

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
	    float4 _Color;

	    float _MovingVelocity;
	    float _YOffset;
	    float _XAmplitude;
	    float _ZAmplitude;
	    float _Rigidness;

            v2f vert (appdata v)
            {
                v2f o;
		float moviment = sin((_Time.x * _MovingVelocity)) * v.uv.y * v.uv.y / _Rigidness;
		v.vertex.x += moviment * _XAmplitude;
		v.vertex.z += moviment * _ZAmplitude;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }
            ENDCG
        }
    }
}

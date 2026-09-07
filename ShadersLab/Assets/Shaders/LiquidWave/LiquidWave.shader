Shader "ShaderLab/Liquid Wave"
{
    Properties
    {
        [HDR] _InsideColor ("Inside Color", Color) = (1,0,0,1)
        [HDR] _OutsideColor ("Outside Color", Color) = (0,0,1,1)
        _ObjectHeight ("Object Height", float) = 1
        _WaveHeight ("Wave Height", float) = 0
        _WaveVelocity ("Wave Velocity", float) = 1
        _WaveFrequency ("Wave Frequency", float) = 0.3
        _Cut ("Cut", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
	        Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 vertex_world : TEXCOORD0;
                float3 centerPos_world : TEXCOORD1;
            };

            float4 _InsideColor;
            float4 _OutsideColor;
            float _ObjectHeight;
            float _WaveHeight;
            float _WaveVelocity;
            float _WaveFrequency;
            float _Cut;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex_world = mul(unity_ObjectToWorld,v.vertex).xyz;
                o.centerPos_world = mul(unity_ObjectToWorld,float4(0,0,0,1)).xyz;
                return o;
            }

            fixed3 frag (v2f i, bool isFrontFace : SV_IsFrontFace) : SV_Target
            {
                float halfHeight = _ObjectHeight / 2;
                clip(-(i.vertex_world - i.centerPos_world).y + (sin((_Time * _WaveVelocity + (i.vertex_world.x + i.vertex_world.z) * _WaveFrequency)) + 1) * _WaveHeight + (-halfHeight + _ObjectHeight * _Cut));

                return _OutsideColor.rgb * isFrontFace + _InsideColor.rgb * (1 - isFrontFace);
            }
            ENDCG
        }
    }
}

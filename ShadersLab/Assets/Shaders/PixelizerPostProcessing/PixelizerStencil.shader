Shader "Unlit/PixelizerStencil"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	_StencilMultiplier ("Stencil Multiplier", float) = 1
	[IntRange] _StencilID ("Stencil ID", Range(0,255)) = 0
    }
    SubShader
    {
        Tags { 
	    "RenderType"="Opaque"
	}

        Pass
        {
	    ColorMask 0
	    ZWrite Off

	    Stencil
	    {
		Ref [_StencilID]
		Comp Always
		Pass Replace
	        Fail Keep
	    }

	    Tags {
	        "LightMode"="UniversalForward" 
	    }

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
            };

	    float _StencilMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex + normalize(v.vertex) * _StencilMultiplier);
                return o;
            }

            fixed3 frag (v2f i, bool isFrontFace : SV_IsFrontFace) : SV_Target
            {
                return 0;
            }
            ENDCG
        }

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i, bool isFrontFace : SV_IsFrontFace) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}

Shader "Unlit/MandelbrotFractal"
{
    Properties
    {
        [Header(Mandelbrot)]
        _Zoom ("Zoom", Float) = 1
        _OffsetX ("Offset X", Float) = -1
        _OffsetY ("Offset Y", Float) = -0.5
        _MaxInterations ("Max Interations",int) = 1000
        [Header(Animation)]
        [MaterialToggle] _EnableAnimation ("Enable Animation",Float) = 0
        _AnimationSpeed ("Animation Speed", Float) = 1 
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

            float _Zoom;
            float _OffsetX;
            float _OffsetY;
            uint _MaxInterations;
            float _EnableAnimation;
            float _AnimationSpeed;

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

            float Mandelbrot(float2 pos,uint max_interations)
            {
                float x,y = 0;

                for (uint i = 0; i < max_interations; i++) {
                    if (x*x + y*y > 4) {
                        return i/_MaxInterations;
                    }

                    float tempx = x*x - y*y + pos.x;
                    y = 2 * x * y + pos.y;
                    x = tempx;
                }

                return 1;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 limitsX = float2((0.5 + _OffsetX) - (1.2 / _Zoom),(0.5 + _OffsetX) + (1.2 / _Zoom));
                float2 limitsY = float2((0.5 + _OffsetY) - (1.12 / _Zoom),(0.5 +  + _OffsetY) + (1.12 / _Zoom));

                float2 mandelbrot_space = float2(
                    lerp(limitsX.x,limitsX.y,i.uv.x),
                    lerp(limitsY.x,limitsY.y,i.uv.y)
                );

                uint max_interations = (1 - abs(sin(_Time.y*_AnimationSpeed)) * _EnableAnimation) * _MaxInterations;

                float mandelbrot = Mandelbrot(mandelbrot_space,max_interations);
                return fixed4(mandelbrot,mandelbrot,mandelbrot,1);
            }
            ENDCG
        }
    }
}

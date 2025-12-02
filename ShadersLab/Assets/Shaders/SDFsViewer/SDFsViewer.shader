Shader "Unlit/SDFsViewer"
{
    Properties
    {
        [Header(SDF Setting)]
        [KeywordEnum(Circle,BoxExact,Segment,Rhombus,Equilateral Triangle,Pentacle,Hexagon,Octogon,Pentagram)]
        _SDFChoice ("SDF Choice", Float) = 0
        [Header(Circle Settings)]
        _CircleRadius ("Circle Radius", Range(0,1)) = 0.5
        [Header(Box Settings)]
        _BoxWidth ("Box Width", Range(0,1)) = 0.5
        _BoxHeight ("Box Height", Range(0,1)) = 0.5
        [Header(Segment Settings)]
        _SegmentAX ("Segment Point A X", Range(-1,1)) = -0.5
        _SegmentAY ("Segment Point A Y", Range(-1,1)) = 0.5
        _SegmentBX ("Segment Point B X", Range(-1,1)) = 0.5
        _SegmentBY ("Segment Point B Y", Range(-1,1)) = -0.5
        [Header(Rhombus Settings)]
        _RhombusWidth ("Rhombus Width", Range(0,1)) = 0.75
        _RhombusHeight ("Rhombus Height", Range(0,1)) = 0.5
        [Header(Equilateral Triangle Settings)]
        _EquilateralTriangleRadius ("Equilateral Triangle Radius", Range(0,1)) = 0.5
        [Header(Pentacle Settings)]
        _PentacleRadius ("Pentacle Radius", Range(0,1)) = 0.5
        [Header(Hexagon Settings)]
        _HexagonRadius ("Hexagon Radius", Range(0,1)) = 0.5
        [Header(Octogon Settings)]
        _OctogonRadius ("Octogon Radius", Range(0,1)) = 0.5
        [Header(Pentagram Settings)]
        _PentagramRadius ("Pentagram Radius", Range(0,1)) = 0.75
        [Header(Display Settings)]
        _BorderTickness ("Borde Tickness", Range(0,1)) = 0.03
        _BorderColor ("Border Color", Color) = (0,0,0,1)
        _OuterBorderTickness ("Outer Borde Tickness", Range(0,1)) = 0.2
        _OuterBorderColor ("Outer Border Color", Color) = (1,1,1,1)
        _VignetteFactor ("Vignette Factor", Range(1,10)) = 4
        _VignetteStrength ("Vignette Strength", Range(1,10)) = 8.3
        _OutsideColor ("Outside Color", Color) = (0.8,0.37,0,1)
        _InsideColor ("Inside Color", Color) = (0.25,0.77,1,1)
        _RipplesQuantity ("Ripples Quantity", Range(10,100)) = 65
        _RipplesThreshold ("Ripples Threshold", Range(0,1)) = 0.7
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

            float _SDFChoice;

            float _CircleRadius;

            float _BoxWidth;
            float _BoxHeight;

            float _SegmentAX;
            float _SegmentAY;
            float _SegmentBX;
            float _SegmentBY;

            float _RhombusWidth;
            float _RhombusHeight;

            float _EquilateralTriangleRadius;

            float _PentacleRadius;

            float _HexagonRadius;

            float _OctogonRadius;

            float _PentagramRadius;

            float _BorderTickness;
            float4 _BorderColor;
            float _OuterBorderTickness;
            float4 _OuterBorderColor;
            float _VignetteFactor;
            float _VignetteStrength;
            float4 _OutsideColor;
            float4 _InsideColor;
            float _RipplesQuantity;
            float _RipplesThreshold;

            float ndot(float2 a, float2 b)
            {
                return a.x*b.x - a.y*b.y;
            }

            float sdfCircle(float2 uv, float radius)
            {
                return length(uv) - radius;
            }

            float sdfBoxExact(float2 uv, float2 size)
            {
                float2 d = abs(uv) - size;
                return length(max(d,0)) + min(max(d.x,d.y),0);
            }

            float sdfSegment(float2 uv, float2 a, float2 b)
            {
                float2 pa = uv-a;
                float2 ba = b-a;
                float h = saturate(dot(pa,ba)/dot(ba,ba));
                return length(pa-ba*h);
            }

            float sdfRhombus(float2 uv, float2 size)
            {
                float2 uvAbs = abs(uv);
                float h = clamp(ndot(size-2*uvAbs,size)/dot(size,size),-1,1);
                float d = length(uvAbs-0.5*size*float2(1-h,1+h));
                return d * sign(uvAbs.x*size.y+uvAbs.y*size.x-size.x*size.y);
            }

            float sdfEquilateralTriangle(float2 uv, float radius)
            {
                float k = sqrt(3);
                float2 pos = float2(abs(uv.x) - radius,uv.y + radius / k);
                if (pos.x+k*pos.y > 0)
                    pos = float2(pos.x-k*pos.y,-k*pos.x-pos.y) / 2;
                pos.x -= clamp(pos.x, -2 * radius, 0);
                return -length(pos)*sign(pos.y);
            }

            float sdfPentacle(float2 uv, float radius)
            {
                float3 k = float3(0.8,0.59,0.73);
                float2 pos = float2(abs(uv.x),-uv.y);

                pos -= 2*min(dot(k.xy*float2(-1,1),pos),0)*k.xy*float2(-1,1);
                pos -= 2*min(dot(k.xy,pos),0)*k.xy;
                pos -= float2(clamp(pos.x,-radius*k.z,radius*k.z),radius);

                return length(pos)*sign(pos.y);
            }

            float sdfHexagon(float2 uv, float radius)
            {
                float3 k = float3(-0.87,0.5,0.58);
                float2 pos = abs(uv);
                pos -= 2*min(dot(k.xy,pos),0)*k.xy;
                pos -= float2(clamp(pos.x,-k.z*radius,k.z*radius),radius);
                return length(pos)*sign(pos.y);
            }

            float sdfOctogon(float2 uv, float radius)
            {
                float3 k = float3(-0.92, 0.38, 0.41);
                float2 pos = abs(uv);
                pos -= 2*min(dot(k.xy,pos),0)*k.xy;
                pos -= 2*min(dot(k.xy*float2(-1,1),pos),0)*k.xy*float2(-1,1);
                pos -= float2(clamp(pos.x,-k.z*radius,k.z*radius),radius);
                return length(pos)*sign(pos.y);
            }

            float sdfPentagram(float2 uv, float radius)
            {
                float3 k1 = float3(0.81,0.59,0.73);
                float2 k2 = float2(0.31,0.95);
                float2 v1 = k1.xy * float2(1,-1);
                float2 v2 = -k1.xy;
                float2 v3 = k2 * float2(1,-1);

                float2 pos = float2(abs(uv.x),uv.y);
                pos -= 2*max(dot(v1,pos),0)*v1;
                pos -= 2*max(dot(v2,pos),0)*v2;
                pos.x = abs(pos.x);
                pos.y -= radius;
                return length(pos-v3*clamp(dot(pos,v3),0,k1.z*radius)) * sign(pos.y*v3.x-pos.x*v3.y);
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
                // sample the texture
                fixed4 col = 1;

                float2 centeredUV = i.uv * 2 - 1;

                float sdfValue = sdfCircle(centeredUV,_CircleRadius) * (_SDFChoice == 0)
                               + sdfBoxExact(centeredUV, float2(_BoxWidth,_BoxHeight)) * (_SDFChoice == 1)
                               + sdfSegment(centeredUV, float2(_SegmentAX,_SegmentAY), float2(_SegmentBX,_SegmentBY)) * (_SDFChoice == 2)
                               + sdfRhombus(centeredUV, float2(_RhombusWidth,_RhombusHeight)) * (_SDFChoice == 3)
                               + sdfEquilateralTriangle(centeredUV, _EquilateralTriangleRadius) * (_SDFChoice == 4)
                               + sdfPentacle(centeredUV, _PentacleRadius) * (_SDFChoice == 5)
                               + sdfHexagon(centeredUV, _HexagonRadius) * (_SDFChoice == 6)
                               + sdfOctogon(centeredUV, _OctogonRadius) * (_SDFChoice == 7)
                               + sdfPentagram(centeredUV, _PentagramRadius) * (_SDFChoice == 8);

                float3 backgroundColor = lerp(_InsideColor.rgb,_OutsideColor.rgb,sdfValue > 0);
                float rippleFactor = step(abs(sin(sdfValue * _RipplesQuantity)),_RipplesThreshold);

                float vignetteMask = abs(sdfValue) <= _BorderTickness * _VignetteFactor;

                col.rgb = backgroundColor * rippleFactor * (1 - (abs(sin(sdfValue)) * _VignetteStrength * vignetteMask));

                col.rgb = lerp(col.rgb,_BorderColor,abs(sdfValue) <= _BorderTickness);
                col.rgb = lerp(col.rgb,_OuterBorderColor, abs(sdfValue) <= _BorderTickness && abs(sdfValue) > _BorderTickness * (1 - _OuterBorderTickness));

                return col;

            }
            ENDCG
        }
    }
}

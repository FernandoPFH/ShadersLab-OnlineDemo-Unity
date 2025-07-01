Shader "Unlit/TheaterCountdown"
{
    Properties
    {
        _BaseColor ("BaseColor", Color) = (0.92,0.9,0.8,1)
        [Header(Number Settings)]
        _FirstNumTex ("1 Tex", 2D) = "white" {}
        _SecondNumTex ("2 Tex", 2D) = "white" {}
        _ThirdNumTex ("3 Tex", 2D) = "white" {}
        _NumSize ("Num Size", Range(0,1)) = 0.45
        [Header(Distance Settings)]
        _NearClip ("Near Clip Distance",Float) = 0.35
        _FarAwayClip ("Far Away Clip Distance",Float) = 10
        [Header(Lines Settings)]
        _CrossLineTickness ("Cross Line Tickness", Range(0,1)) = 0.005
        [Header(Circles Settings)]
        _CircleLineTickness ("Circle Line Tickness", Range(0,1)) = 0.01
        _InnerCircleRadius ("Inner Circle Radius", range(0,1)) = 0.5
        _OuterCircleOffset ("Outer Circle Offset", range(0,1)) = 0.25
        [Header(Shadow Settings)]
        _ShadowAngleOffset ("Shadow Angle Offset", Range(0,360)) = 90
        _ShadowPower ("Shadow Power", Range(1,10)) = 6.4
        [Header(Vignette Settings)]
        _VignetteRadius ("Vignette Radius", Range(0,2)) = 1
        _VignettePower ("Vignette Power", Range(0,2)) = 1.8
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
                float3 centerPos_world : TEXCOORD1;
            };

            float4 _BaseColor;

            sampler2D _FirstNumTex;
            sampler2D _SecondNumTex;
            sampler2D _ThirdNumTex;
            float _NumSize;

            float _NearClip;
            float _FarAwayClip;

            float _CrossLineTickness;

            float _CircleLineTickness;
            float _InnerCircleRadius;
            float _OuterCircleOffset;

            float _ShadowAngleOffset;
            float _ShadowPower;

            float _VignetteRadius;
            float _VignettePower;

            float angleBetween(float2 v1, float2 v2)
            {
                // Calcular o ângulo em radianos
                float angle = atan2(v1.y, v1.x);

                // Adicionar 90 graus ao ângulo
                angle -= UNITY_PI / 2;

                // Ajustar para 0 a 2*PI
                if (angle < 0.0)
                    angle += 2.0 * UNITY_PI;

                // Rad => Deg
                return angle / (UNITY_PI / 180);
            }

            float map(float x, float in_min, float in_max, float out_min, float out_max) {
                return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
            }

            float2 map(float2 v, float2 in_min, float2 in_max, float2 out_min, float2 out_max) {
                return float2(
                    map(v.x,in_min.x,in_max.x,out_min.x,out_max.x),
                    map(v.y,in_min.y,in_max.y,out_min.y,out_max.y)
                );
            }

            float sdfSegment(float2 uv, float2 a, float2 b)
            {
                float2 pa = uv-a;
                float2 ba = b-a;
                float h = saturate(dot(pa,ba)/dot(ba,ba));
                return length(pa-ba*h);
            }

            float sdfCircle(float2 uv, float radius)
            {
                return length(uv) - radius;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.centerPos_world = mul(unity_ObjectToWorld,float4(0,0,0,1)).xyz;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _BaseColor;

                float2 centeredUV = i.uv * 2 - 1;

                float cameraDistance = length(_WorldSpaceCameraPos - i.centerPos_world);

                float perCountDistance = (_FarAwayClip - _NearClip) / 3;

                float angleCurrentCount = map((cameraDistance - _NearClip)%perCountDistance,0,perCountDistance,90,450);
                float radCurrentCount = angleCurrentCount * (UNITY_PI / 180);

                float angleShadowEffect = angleCurrentCount + _ShadowAngleOffset;
                float angleShadowEffectClamped = clamp(angleShadowEffect,90,450);

                float currentAngle = angleBetween(centeredUV, float2(cos(90 * (UNITY_PI / 180)),sin(90 * (UNITY_PI / 180))));

                float currentCount = clamp(floor((cameraDistance - _NearClip)/perCountDistance),0,2);

                float2 numberUV = map(centeredUV,-_NumSize,_NumSize,0,1);

                float4 currentCountColor = tex2D(_FirstNumTex, numberUV) * (currentCount == 0)
                                        + tex2D(_SecondNumTex, numberUV) * (currentCount == 1)
                                        + tex2D(_ThirdNumTex, numberUV) * (currentCount == 2);

                float animatedLineSDF = sdfSegment(centeredUV, float2(0,0), float2(cos(radCurrentCount),sin(radCurrentCount))*1.5);

                float innerCircleSDF = sdfCircle(centeredUV, _InnerCircleRadius);
                float outerCircleSDF = sdfCircle(centeredUV, _InnerCircleRadius+_OuterCircleOffset);
                
                float vignetteSDF = sdfCircle(centeredUV, _VignetteRadius);

                float shadowFactor = saturate((1-pow(map(currentAngle+90,angleCurrentCount,angleShadowEffect,1,0),_ShadowPower)));

                col.rgb *= lerp(shadowFactor,1,shadowFactor==0);
                col = lerp(col,fixed4(0,0,0,1),abs(i.uv.x-0.5)<=_CrossLineTickness||abs(i.uv.y-0.5)<=_CrossLineTickness);
                col = lerp(col,fixed4(0,0,0,1),abs(innerCircleSDF)<=_CircleLineTickness || abs(outerCircleSDF)<=_CircleLineTickness);
                col = lerp(col,fixed4(0,0,0,1),abs(animatedLineSDF)<=_CircleLineTickness);
                col = lerp(col,currentCountColor,length(centeredUV)<=_NumSize&&currentCountColor.a != 0);
                col = lerp(col,_BaseColor,cameraDistance > _FarAwayClip || cameraDistance < _NearClip);
                col = lerp(col,col*((2-pow(length(centeredUV),_VignettePower))),vignetteSDF >= 0);
                
                return col;
            }
            ENDCG
        }
    }
}

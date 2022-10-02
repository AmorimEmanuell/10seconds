Shader "Unlit/Background"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1
        _FogIntensity ("Fog intensity", Range(0, 0.1)) = 0.02
        _FogDistance ("Fog distance", Range(0.1, 1)) = 0.8
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
            float _Speed;
            float _FogIntensity;
            float _FogDistance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float2 uv = i.uv * 2 - 1;
                uv.x += sin(_Time.y * 0.6) * 0.2;
                uv.y += cos(_Time.y * 0.5) * 0.3;

                float a = atan2(uv.y, uv.x);
                float r = length(uv);

                float2 deformedUV = float2(0.3 / r + _Speed * _Time.y, a / 3.1415927);
                float2 uv2 = float2(uv.x, atan2(uv.y, abs(uv.x)) / 3.1415927);
                fixed4 col = tex2Dgrad(_MainTex, deformedUV, ddx(uv2), ddy(uv2)) * r;


                col.rgb = lerp(col.rgb + _FogIntensity, col.rgb, smoothstep(0, _FogDistance, r));

                return col;
            }
            ENDCG
        }
    }
}

﻿Shader "Unlit/LinearGradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
        _NoiseForce("Noise Force", Range(0, 500)) = 10
        _NoiseScale("Noise Scale", Vector) = (10, 10, 0, 0)
        _ColorFrom ("Color From", Color) = (1, 0, 1, 1)
        _ColorTo ("Color To", Color) = (1, 1, 0, 1)
        
        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15
    }
    SubShader
    {
        ZTest [unity_GUIZTestMode]
        
        Tags { "Queue" = "Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha 
        
        Stencil{
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

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
	        sampler2D _NoiseTex;
	        float _NoiseForce;
	        float2 _NoiseScale;
            float4 _ColorFrom;
            float4 _ColorTo;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float3 getNoise(float2 uv)
            {
                float2 scale = 10;
                float force = _NoiseForce;
                return tex2D(_NoiseTex, uv * _NoiseScale)/force;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 pixelPoint = fixed2(i.uv.x, i.uv.y);
                float dist = pixelPoint.x;
                dist = clamp(dist, 0, 1);
                fixed4 col = lerp(_ColorFrom, _ColorTo, dist);
                col.rgb += getNoise(i.uv);
                return col;
            }
            ENDCG
        }
    }
}

﻿Shader "Custom/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SceneTex("Scene Texture", 2D) = "white" {}
		_OutlineDepthBuffer("Outline Depth Buffer", 2D) = "black" {}
		_InnerColor("Inner Color", Color) = (1, 1, 1, 0)
		_OutlineColor("Outline Color", Color) = (1, 0, 0, 0)
		_OutlineStrength("Outline Strength", Float) = 2
	}
	SubShader
	{
		Cull off
		ZWrite Off
		ZTest Always

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

			sampler2D _CameraDepthTexture;
			sampler2D _OutlineDepthBuffer;

			sampler2D _MainTex;
			float2 _MainTex_TexelSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			half frag (v2f i) : SV_Target
			{
				const float blurIterations = 30;
				float colorAmount = 0;
				//float outlineDepthSample = 0;

				/*
				for (int j = 0; j < blurIterations; j++) 
				{
					float offset = 
					float2 samplePosition = i.uv + float2((j - .5 * (blurIterations/2.0)) * _MainTex_TexelSize.x * 6.0, 0);
					colorAmount += tex2D(_MainTex, samplePosition).r / blurIterations;
					//outlineDepthSample = max(outlineDepthSample, tex2D(_OutlineDepthBuffer, samplePosition).x);
				}
				*/

				float2 tc = i.uv;
				float hstep = 1.0;
				float vstep = 0.0;
				float radius = 8.0;
				float sum = 0.0;
				//blur radius in pixels
                float blur = radius/1600.0;     

                sum += tex2D(_MainTex, float2(tc.x - 4.0*blur*hstep, tc.y - 4.0*blur*vstep)).r * 0.0162162162;
                sum += tex2D(_MainTex, float2(tc.x - 3.0*blur*hstep, tc.y - 3.0*blur*vstep)).r * 0.0540540541;
                sum += tex2D(_MainTex, float2(tc.x - 2.0*blur*hstep, tc.y - 2.0*blur*vstep)).r * 0.1216216216;
                sum += tex2D(_MainTex, float2(tc.x - 1.0*blur*hstep, tc.y - 1.0*blur*vstep)).r * 0.1945945946;

                sum += tex2D(_MainTex, float2(tc.x, tc.y)).r * 0.2270270270;

                sum += tex2D(_MainTex, float2(tc.x + 1.0*blur*hstep, tc.y + 1.0*blur*vstep)).r * 0.1945945946;
                sum += tex2D(_MainTex, float2(tc.x + 2.0*blur*hstep, tc.y + 2.0*blur*vstep)).r * 0.1216216216;
                sum += tex2D(_MainTex, float2(tc.x + 3.0*blur*hstep, tc.y + 3.0*blur*vstep)).r * 0.0540540541;
                sum += tex2D(_MainTex, float2(tc.x + 4.0*blur*hstep, tc.y + 4.0*blur*vstep)).r * 0.0162162162;

				// Manual z test to cull against objects in front of the object being outlined
				//float sceneDepthSample = tex2D(_CameraDepthTexture, i.uv).x;
				//float depthDifference = outlineDepthSample - sceneDepthSample;

				// There's a bit of fudge here, you get flickering on the outline sometimes otherwise
				//return (depthDifference < -0.001 ? 0.0 : colorAmount);
				return sum;
			}
			ENDCG
		}

		GrabPass {}

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
			sampler2D _SceneTex;

			// This has to be named _GrabTexture due to Unity convention.
			sampler2D _GrabTexture;
			float2 _GrabTexture_TexelSize;

			float4 _InnerColor;
			float4 _OutlineColor;
			float _OutlineStrength;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float sum = 0;
				float2 grabUv = i.uv;

				//#if UNITY_UV_STARTS_AT_TOP
				//grabUv.y = 1 - grabUv.y;
				//#endif

				float2 tc = grabUv;
				float hstep = 0.0;
				float vstep = 1.0;
				float radius = 16.0;
				//blur radius in pixels
				float blur = radius/1600.0;

				sum += tex2D(_GrabTexture, float2(tc.x - 4.0*blur*hstep, tc.y - 4.0*blur*vstep)).r * 0.0162162162;
				sum += tex2D(_GrabTexture, float2(tc.x - 3.0*blur*hstep, tc.y - 3.0*blur*vstep)).r * 0.0540540541;
				sum += tex2D(_GrabTexture, float2(tc.x - 2.0*blur*hstep, tc.y - 2.0*blur*vstep)).r * 0.1216216216;
				sum += tex2D(_GrabTexture, float2(tc.x - 1.0*blur*hstep, tc.y - 1.0*blur*vstep)).r * 0.1945945946;

				sum += tex2D(_GrabTexture, float2(tc.x, tc.y)).r * 0.2270270270;

				sum += tex2D(_GrabTexture, float2(tc.x + 1.0*blur*hstep, tc.y + 1.0*blur*vstep)).r * 0.1945945946;
				sum += tex2D(_GrabTexture, float2(tc.x + 2.0*blur*hstep, tc.y + 2.0*blur*vstep)).r * 0.1216216216;
				sum += tex2D(_GrabTexture, float2(tc.x + 3.0*blur*hstep, tc.y + 3.0*blur*vstep)).r * 0.0540540541;
				sum += tex2D(_GrabTexture, float2(tc.x + 4.0*blur*hstep, tc.y + 4.0*blur*vstep)).r * 0.0162162162;

				return lerp(tex2D(_SceneTex, i.uv), lerp(_OutlineColor, _InnerColor, sum * 0.5), sum * _OutlineStrength);
			}
			ENDCG
		}
	}
}
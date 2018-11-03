Shader "Hidden/Blue"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;	

			fixed4 frag (v2f_img i) : SV_Target
			{
				float4 tex = tex2D(_MainTex, i.uv);
				tex.r = 0.0;
				tex.g = 0.0;
				tex.b = tex.b * 1.0;
				return tex;
			}
			ENDCG
		}
	}
}

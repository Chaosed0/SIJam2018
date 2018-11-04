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
				tex.b = clamp(tex.b * 0.5, 0.0, 0.1);
				return tex;
			}
			ENDCG
		}
	}
}

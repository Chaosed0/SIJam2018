Shader "Hidden/Pixelation"
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
			float2 BlockCount;
			float ColorStep;

			fixed4 frag (v2f_img i) : SV_Target
			{
				float4 tex = tex2D(_MainTex, floor(i.uv * BlockCount) / BlockCount + 1 / (BlockCount * 2));
				tex.rgb = round(tex.rgb * ColorStep) / ColorStep;
				return tex;
			}
			ENDCG
		}
	}
}

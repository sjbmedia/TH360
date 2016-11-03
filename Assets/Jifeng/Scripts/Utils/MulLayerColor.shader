Shader "Jifeng/MulLayerColor" {
	Properties {
		_InnerTex("Tex Inner Layer", 2D) = "white" {}
		_OuterTex("Tex Outter Layer", 2D) = "white"{}
		_DeepColor("Deep Color", Color) = (1, 1, 1, 1)
		_BackColor("Back Color", Color) = (1, 1, 1, 1)
		_SurfColor("Surface Color", Color) = (1, 1, 1, 1)
		_TopColor("Top Color", Color) = (1, 1, 1, 1)
		_InMaskEnable("Enable Inner Mask", Range(0, 1)) = 0
		_OutMaskEnable("Enable Outter Mask",Range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Lighting Off
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _InnerTex;
		sampler2D _OuterTex;
		float4 _DeepColor;
		float4 _BackColor;
		float4 _SurfColor;
		float4 _TopColor;
		float _InMaskEnable;
		float _OutMaskEnable;

		struct Input {
			float2 uv_InnerTex;
			float2 uv_OuterTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = _DeepColor;
			if (_InMaskEnable > 0.5f)
			{
				half4 c1 = tex2D(_InnerTex, IN.uv_InnerTex);
				if (c1.a > 0.5f)
				{
					c = _BackColor;
				}
			}
			half4 c2 = tex2D(_OuterTex, IN.uv_OuterTex);
			if (c2.a > 0.5f)
			{
				if (_OutMaskEnable > 0.5f)
				{
					c = _TopColor;
				}
				else
				{
					c = _SurfColor;
				}
			}
			o.Emission = c.rgb;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

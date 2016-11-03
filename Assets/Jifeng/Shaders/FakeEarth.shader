Shader "Jifeng/Earthlike" {
	Properties {
		[PerRendererData] _MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float uvx = IN.uv_MainTex.x - 0.9 * _Time;
			float2 uv = float2(uvx, IN.uv_MainTex.y);
			half4 c = tex2D (_MainTex, uv);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = c.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

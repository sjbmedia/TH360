Shader "Jifeng/PlayButtonSprite" {
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[HideInInspector]_CursorTex("Cursor Texutre", 2D) = "white"{}
		[HideInInspector]_CursorWid("Cursor Size", float) = 1
		[HideInInspector]_CursorHei("Cursor Height", float) = 1
		[HideInInspector]_CursorOff("Cursor Offset",float) = 0
		_CursorSpeed("Cursor move speed",float) = 1
		_Factor("factor,make it more dark", Range(0,0.9)) = 0.7
		_Percent("Current Percent", Range(0, 1)) = 0.1
		_Borderp("Border Percent", Range(0,1)) = 0
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _CursorTex;
		float _CursorWid;
		float _CursorHei;
		float _CursorOff;
		float _CursorSpeed;
		fixed _Factor;
		float _Percent;
		float _Borderp;

		struct Input
		{
			float2	uv_MainTex;
			float2	uv_CursorTex;
		};
		float inFlash(half2 uv)
		{
			float brightness = 0;

			float percent = _Percent;
		
			float xProj = uv.x;

			if (xProj < percent)
			{
				brightness = 0.3;
			}

			if (_Borderp > 0.5)
			if (uv.y < 0.1 || uv.y > 1 - 0.1)
			{
				brightness = 0.6;
			}

			return brightness;
		}


		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			if (_CursorWid > 0 && _CursorWid < 1
				&& _CursorHei > 0 && _CursorHei < 1)
			{
				float startp = _Time.x * _CursorSpeed + _CursorOff;
				startp = startp - (int)startp;
				float leftp = IN.uv_MainTex.x - startp;
				float topp = IN.uv_MainTex.y - (1 - _CursorHei) / 2;
				if (leftp >= 0 && leftp <= _CursorWid
					&& topp >= 0 && topp <= _CursorHei)
				{
					float2 uv2;
					uv2.x = leftp / _CursorWid;
					uv2.y = topp / _CursorHei;
					fixed4 arrow = tex2D(_CursorTex, uv2);
					if (arrow.a > 0)
					{
						c.rgb = c.rgb * 1.1;
					}
				}
				else if (leftp < 0 
					&& topp >= _CursorHei / 4 
					&& topp <= _CursorHei / 4 * 3)
				{
					c.rgb = c.rgb * 1.1;
				}
			}
			float brightness = inFlash(IN.uv_MainTex);
			if (brightness < 0.2)
			{
				c.rgb = c.rgb * _Factor;
			}
			else if (brightness > 0.5)
			{
			}
			else
			{

			}
			o.Emission = c.rgb;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}

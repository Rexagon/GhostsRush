// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GameUnit"
{
    Properties 
	{
		_PlayerMask("Player Mask", 2D) = "black" {}
		_FirstPlayer("First Player Texture", 2D) = "white" {}
		_SecondPlayer("Second Player Texture", 2D) = "white" {}

		[PerRendererData][Enum(First,0,Second,1)] _ColorId("Color ID", Float) = 0

		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_HighlightColor("Highlight Color", Color) = (0.94, 0.92, 0, 1)
		_HighlightAlpha("Highlight Transparency", Range(0,1)) = 0.5
		[Toggle] _Highlighted("Is Highlighted", Float) = 0
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 300
 
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
 
		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		sampler2D _PlayerMask;
		sampler2D _FirstPlayer;
		sampler2D _SecondPlayer;

		half _ColorId;
 
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		fixed4 _HighlightColor;
		half _HighlightAlpha;
		half _Highlighted;
 
		void surf (Input i, inout SurfaceOutputStandard o) 
		{
			fixed4 color;

			float mask = tex2D(_PlayerMask, i.uv_MainTex).r;
			if (mask > 0.8) {
				if (_ColorId == 0) {
					color = tex2D(_FirstPlayer, i.uv_MainTex);
				}
				else {
					color = tex2D(_SecondPlayer, i.uv_MainTex);
				}
			}
			else {
				color = tex2D(_MainTex, i.uv_MainTex);
			}

			if (_Highlighted != 0) {
				color += _HighlightAlpha * _HighlightColor;
			}

			o.Albedo = color.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = color.a;
		}
		ENDCG
	}

	FallBack "Diffuse"
}

Shader "Custom/Terrain" {
	Properties {
		[HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}
		[HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}
		[HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
		[HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
		[HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
		[HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
		[HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "white" {}
		[HideInInspector] _Color("Main Color", Color) = (1,1,1,1)
	}

	CGINCLUDE
		#pragma surface surf Lambert vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer
		#pragma multi_compile_fog
		#include "TerrainSplatmapCommon.cginc"


		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 splat_control;
			half weight;
			fixed4 mixedDiffuse;
			SplatmapMix(IN, splat_control, weight, mixedDiffuse, o.Normal);

			float fog = clamp(1.0 - pow(2.718281828459, 11 * (length(float2(-25, -25) + IN.uv_Splat0.xy) / 25 - 0.9)), 0.0, 1.0);
			o.Albedo = lerp(float3(1, 1, 1), mixedDiffuse.rgb, fog);
			o.Alpha = weight;
		}
	ENDCG

	Category {
		Tags {
			"Queue" = "Geometry-99"
			"RenderType" = "Opaque"
		}

		SubShader{
			CGPROGRAM
				#pragma target 3.0
				#pragma multi_compile __ _TERRAIN_NORMAL_MAP
			ENDCG
		}

		SubShader{
			CGPROGRAM
			ENDCG
		}
	}

	Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Diffuse-AddPass"
	Dependency "BaseMapShader" = "Diffuse"
	Dependency "Details0" = "Hidden/TerrainEngine/Details/Vertexlit"
	Dependency "Details1" = "Hidden/TerrainEngine/Details/WavingDoublePass"
	Dependency "Details2" = "Hidden/TerrainEngine/Details/BillboardWavingDoublePass"
	Dependency "Tree0" = "Hidden/TerrainEngine/BillboardTree"

	Fallback "Diffuse"
}

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SHADERNAMEENUM"
{
	Properties
	{
		_Maskclipvalue("Mask clip value (0 to enable alpha blend)", Float) = 0.5
		[NoScaleOffset]_DiffuseTexture("Diffuse Texture", 2D) = "white" {}
		[NoScaleOffset]_GstackTexture("Gstack Texture", 2D) = "white" {}
		[NoScaleOffset]_NormalMap("Normal Map", 2D) = "bump" {}
		[NoScaleOffset]_DyeSlotTexture("DyeSlot Texture", 2D) = "white" {}
		[Toggle]_EnableifusingDyeSlotTexture("Enable if using DyeSlot Texture -->", Float) = 0
		[NoScaleOffset]_Iridescence_Lookup("_Iridescence_Lookup", 2D) = "white" {}
		[NoScaleOffset]_DetailDiffuse01("DiffMap1", 2D) = "gray" {}
		[NoScaleOffset][Normal]_DetailNormal01("NormMap1", 2D) = "bump" {}
		[NoScaleOffset]_DetailDiffuse02("DiffMap1", 2D) = "white" {}
		[NoScaleOffset][Normal]_DetailNormal02("NormMap1", 2D) = "bump" {}
		[NoScaleOffset]_DetailDiffuse03("DiffMap2", 2D) = "white" {}
		[NoScaleOffset][Normal]_DetailNormal03("NormMap2", 2D) = "bump" {}
		[NoScaleOffset]_DetailDiffuse04("DiffMap2", 2D) = "white" {}
		[NoScaleOffset][Normal]_DetailNormal04("NormMap2", 2D) = "bump" {}
		[NoScaleOffset]_DetailDiffuse05("DiffMap3", 2D) = "white" {}
		[NoScaleOffset][Normal]_DetailNormal05("NormMap3", 2D) = "bump" {}
		[NoScaleOffset]_DetailDiffuse06("DiffMap3", 2D) = "white" {}
		[NoScaleOffset][Normal]_DetailNormal06("NormMap3", 2D) = "bump" {}
		_Armor_DetailDiffuseTransform("Armor_Detail Diffuse Transform", Vector) = (DiffTrans1.X, DiffTrans1.Y, DiffTrans1.Z, DiffTrans1.W)
		_Armor_DetailNormalTransform("Armor_Detail Normal Transform", Vector) = (NormTrans1.X, NormTrans1.Y, NormTrans1.Z, NormTrans1.W)
		_Cloth_DetailDiffuseTransform("Cloth_Detail Diffuse Transform", Vector) = (DiffTrans2.X, DiffTrans2.Y, DiffTrans2.Z, DiffTrans2.W)
		_Cloth_DetailNormalTransform("Cloth_Detail Normal Transform", Vector) = (NormTrans2.X, NormTrans2.Y, NormTrans2.Z, NormTrans2.W)
		_Suit_DetailDiffuseTransform("Suit_Detail Diffuse Transform", Vector) = (DiffTrans3.X, DiffTrans3.Y, DiffTrans3.Z, DiffTrans3.W)
		_Suit_DetailNormalTransform("Suit_Detail Normal Transform", Vector) = (NormTrans3.X, NormTrans3.Y, NormTrans3.Z, NormTrans3.W)
		_ArmorPrimary_Color("ArmorPrimary_Color", Color) = (CPrime1.X, CPrime1.Y, CPrime1.Z, 1.0)
		_ArmorPrimary_WearRemap("ArmorPrimary_Wear Remap", Vector) = (PrimeWearMap1.X, PrimeWearMap1.Y, PrimeWearMap1.Z, PrimeWearMap1.W)
		_ArmorPrimary_RoughnessRemap("ArmorPrimary_Roughness Remap", Vector) = (PrimeRoughMap1.X, PrimeRoughMap1.Y, PrimeRoughMap1.Z, PrimeRoughMap1.W)
		_ArmorPrimary_DetailDiffuseBlend("ArmorPrimary_Detail Diffuse Blend", Float) = PrimeMatParams1.X
		_ArmorPrimary_DetailNormalBlend("ArmorPrimary_Detail Normal Blend", Float) = PrimeMatParams1.Y
		_ArmorPrimary_DetailRoughnessBlend("ArmorPrimary_Detail Roughness Blend", Float) = PrimeMatParams1.Z
		_ArmorPrimary_Metalness("ArmorPrimary_Metalness", Float) = PrimeMatParams1.W
		_ArmorPrimary_Iridescence("ArmorPrimary_Iridescence", Float) = PrimeAdvMatParams1.X
		_ArmorPrimary_Fuzz("ArmorPrimary_Fuzz", Float) = PrimeAdvMatParams1.Y
		_ArmorPrimary_Transmission("ArmorPrimary_Transmission", Float) = PrimeAdvMatParams1.Z
		[HDR]_ArmorPrimary_Emission("ArmorPrimary_Emission", Color) = (CPrimeEmit1.X, CPrimeEmit1.Y, CPrimeEmit1.Z, 1.0)
		_WornArmorPrimary_Color("WornArmorPrimary_Color", Color) = (CPrimeWear1.X, CPrimeWear1.Y, CPrimeWear1.Z, 1.0)
		_WornArmorPrimary_RoughnessRemap("WornArmorPrimary_Roughness Remap", Vector) = (PrimeWornRoughMap1.X ,PrimeWornRoughMap1.Y ,PrimeWornRoughMap1.Z ,PrimeWornRoughMap1.W)
		_WornArmorPrimary_DetailDiffuseBlend("WornArmorPrimary_Detail Diffuse Blend", Float) = PrimeWornMatParams1.X
		_WornArmorPrimary_DetailNormalBlend("WornArmorPrimary_Detail Normal Blend", Float) = PrimeWornMatParams1.Y
		_WornArmorPrimary_DetailRoughnessBlend("WornArmorPrimary_Detail Roughness Blend", Float) = PrimeWornMatParams1.Z
		_WornArmorPrimary_Metalness("WornArmorPrimary_Metalness", Float) = PrimeWornMatParams1.W
		_ArmorSecondary_Color("ArmorSecondary_Color", Color) = (CSecon1.X, CSecon1.Y, CSecon1.Z, 1.0)
		_ArmorSecondary_WearRemap("ArmorSecondary_Wear Remap", Vector) = (SeconWearMap1.X, SeconWearMap1.Y, SeconWearMap1.Z, SeconWearMap1.W)
		_ArmorSecondary_RoughnessRemap("ArmorSecondary_Roughness Remap", Vector) = (SeconRoughMap1.X, SeconRoughMap1.Y, SeconRoughMap1.Z, SeconRoughMap1.W)
		_ArmorSecondary_DetailDiffuseBlend("ArmorSecondary_Detail Diffuse Blend", Float) = SeconMatParams1.X
		_ArmorSecondary_DetailNormalBlend("ArmorSecondary_Detail Normal Blend", Float) = SeconMatParams1.Y
		_ArmorSecondary_DetailRoughnessBlend("ArmorSecondary_Detail Roughness Blend", Float) = SeconMatParams1.Z
		_ArmorSecondary_Metalness("ArmorSecondary_Metalness", Float) = SeconMatParams1.W
		_ArmorSecondary_Iridescence("ArmorSecondary_Iridescence", Float) = SeconAdvMatParams1.X
		_ArmorSecondary_Fuzz("ArmorSecondary_Fuzz", Float) = SeconAdvMatParams1.Y
		_ArmorSecondary_Transmission("ArmorSecondary_Transmission", Float) = SeconAdvMatParams1.Z
		[HDR]_ArmorSecondary_Emission("ArmorSecondary_Emission", Color) = (CSeconEmit1.X, CSeconEmit1.Y, CSeconEmit1.Z, 1.0)
		_WornArmorSecondary_Color("WornArmorSecondary_Color", Color) = (CSeconWear1.X, CSeconWear1.Y, CSeconWear1.Z, 1.0)
		_WornArmorSecondary_RoughnessRemap("WornArmorSecondary_Roughness Remap", Vector) = (SeconWornRoughMap1.X, SeconWornRoughMap1.Y, SeconWornRoughMap1.Z, SeconWornRoughMap1.W)
		_WornArmorSecondary_DetailDiffuseBlend("WornArmorSecondary_Detail Diffuse Blend", Float) = SeconWornMatParams1.X
		_WornArmorSecondary_DetailRoughnessBlend("WornArmorSecondary_Detail Roughness Blend", Float) = SeconWornMatParams1.Y
		_WornArmorSecondary_DetailNormalBlend("WornArmorSecondary_Detail Normal Blend", Float) = SeconWornMatParams1.Z
		_WornArmorSecondary_Metalness("WornArmorSecondary_Metalness", Float) = SeconWornMatParams1.W
		_ClothPrimary_Color("ClothPrimary_Color", Color) = (CPrime2.X, CPrime2.Y, CPrime2.Z, 1.0)
		_ClothPrimary_WearRemap("ClothPrimary_Wear Remap", Vector) = (PrimeWearMap2.X, PrimeWearMap2.Y, PrimeWearMap2.Z, PrimeWearMap2.W)
		_ClothPrimary_RoughnessRemap("ClothPrimary_Roughness Remap", Vector) = (PrimeRoughMap2.X, PrimeRoughMap2.Y, PrimeRoughMap2.Z, PrimeRoughMap2.W)
		_ClothPrimary_DetailDiffuseBlend("ClothPrimary_Detail Diffuse Blend", Float) = PrimeMatParams2.X
		_ClothPrimary_DetailNormalBlend("ClothPrimary_Detail Normal Blend", Float) = PrimeMatParams2.Y
		_ClothPrimary_DetailRoughnessBlend("ClothPrimary_Detail Roughness Blend", Float) = PrimeMatParams2.Z
		_ClothPrimary_Metalness("ClothPrimary_Metalness", Float) = PrimeMatParams2.W
		_ClothPrimary_Iridescence("ClothPrimary_Iridescence", Float) = PrimeAdvMatParams2.X
		_ClothPrimary_Fuzz("ClothPrimary_Fuzz", Float) = PrimeAdvMatParams2.Y
		_ClothPrimary_Transmission("ClothPrimary_Transmission", Float) = PrimeAdvMatParams2.Z
		[HDR]_ClothPrimary_Emission("ClothPrimary_Emission", Color) = (CPrimeEmit2.X, CPrimeEmit2.Y, CPrimeEmit2.Z, 1.0)
		_WornClothPrimary_Color("WornClothPrimary_Color", Color) = (CPrimeWear2.X, CPrimeWear2.Y, CPrimeWear2.Z, 1.0)
		_WornClothPrimary_RoughnessRemap("WornClothPrimary_Roughness Remap", Vector) = (PrimeWornRoughMap2.X, PrimeWornRoughMap2.Y, PrimeWornRoughMap2.Z, PrimeWornRoughMap2.W)
		_WornClothPrimary_DetailDiffuseBlend("WornClothPrimary_Detail Diffuse Blend", Float) = PrimeWornMatParams1.X
		_WornClothPrimary_DetailNormalBlend("WornClothPrimary_Detail Normal Blend", Float) = PrimeWornMatParams1.Y
		_WornClothPrimary_DetailRoughnessBlend("WornClothPrimary_Detail Roughness Blend", Float) = PrimeWornMatParams1.Z
		_WornClothPrimary_Metalness("WornClothPrimary_Metalness", Float) = PrimeWornMatParams1.W
		_ClothSecondary_Color("ClothSecondary_Color", Color) = (CSecon2.X, CSecon2.Y, CSecon2.Z, 1.0)
		_ClothSecondary_WearRemap("ClothSecondary_Wear Remap", Vector) = (SeconWearMap2.X, SeconWearMap2.Y, SeconWearMap2.Z, SeconWearMap2.W)
		_ClothSecondary_RoughnessRemap("ClothSecondary_Roughness Remap", Vector) = (SeconRoughMap2.X, SeconRoughMap2.Y, SeconRoughMap2.Z, SeconRoughMap2.W)
		_ClothSecondary_DetailDiffuseBlend("ClothSecondary_Detail Diffuse Blend", Float) = SeconMatParams2.X
		_ClothSecondary_DetailNormalBlend("ClothSecondary_Detail Normal Blend", Float) = SeconMatParams2.Y
		_ClothSecondary_DetailRoughnessBlend("ClothSecondary_Detail Roughness Blend", Float) = SeconMatParams2.Z
		_ClothSecondary_Metalness("ClothSecondary_Metalness", Float) = SeconMatParams2.W
		_ClothSecondary_Iridescence("ClothSecondary_Iridescence", Float) = SeconAdvMatParams2.X
		_ClothSecondary_Fuzz("ClothSecondary_Fuzz", Float) = SeconAdvMatParams2.Y
		_ClothSecondary_Transmission("ClothSecondary_Transmission", Float) = SeconAdvMatParams2.Z
		[HDR]_ClothSecondary_Emission("ClothSecondary_Emission", Color) = (CSeconEmit2.X, CSeconEmit2.Y, CSeconEmit2.Z, 1.0)
		_WornClothSecondary_Color("WornClothSecondary_Color", Color) = (CSeconWear2.X, CSeconWear2.Y, CSeconWear2.Z, 1.0)
		_WornClothSecondary_RoughnessRemap("WornClothSecondary_Roughness Remap", Vector) = (SeconWornRoughMap2.X, SeconWornRoughMap2.Y, SeconWornRoughMap2.Z, SeconWornRoughMap2.W)
		_WornClothSecondary_DetailDiffuseBlend("WornClothSecondary_Detail Diffuse Blend", Float) = SeconWornMatParams2.X
		_WornClothSecondary_DetailNormalBlend("WornClothSecondary_Detail Normal Blend", Float) = SeconWornMatParams2.Y
		_WornClothSecondary_DetailRoughnessBlend("WornClothSecondary_Detail Roughness Blend", Float) = SeconWornMatParams2.Z
		_WornClothSecondary_Metalness("WornClothSecondary_Metalness", Float) = SeconWornMatParams2.W
		_SuitPrimary_Color("SuitPrimary_Color", Color) = (CPrime3.X, CPrime3.Y, CPrime3.Z, 1.0)
		_SuitPrimary_WearRemap("SuitPrimary_Wear Remap", Vector) = (PrimeWearMap3.X, PrimeWearMap3.Y, PrimeWearMap3.Z, PrimeWearMap3.W)
		_SuitPrimary_RoughnessRemap("SuitPrimary_Roughness Remap", Vector) = (PrimeRoughMap3.X, PrimeRoughMap3.Y, PrimeRoughMap3.Z, PrimeRoughMap3.W)
		_SuitPrimary_DetailDiffuseBlend("SuitPrimary_Detail Diffuse Blend", Float) = PrimeMatParams3.X
		_SuitPrimary_DetailNormalBlend("SuitPrimary_Detail Normal Blend", Float) = PrimeMatParams3.Y
		_SuitPrimary_DetailRoughnessBlend("SuitPrimary_Detail Roughness Blend", Float) = PrimeMatParams3.Z
		_SuitPrimary_Metalness("SuitPrimary_Metalness", Float) = PrimeMatParams3.W
		_SuitPrimary_Iridescence("SuitPrimary_Iridescence", Float) = PrimeAdvMatParams3.X
		_SuitPrimary_Fuzz("SuitPrimary_Fuzz", Float) = PrimeAdvMatParams3.Y
		_SuitPrimary_Transmission("SuitPrimary_Transmission", Float) = PrimeAdvMatParams3.Z
		[HDR]_SuitPrimary_Emission("SuitPrimary_Emission", Color) = (CPrimeEmit3.X, CPrimeEmit3.Y, CPrimeEmit3.Z, 1.0)
		_WornSuitPrimary_Color("WornSuitPrimary_Color", Color) = (CPrimeWear3.X, CPrimeWear3.Y, CPrimeWear3.Z, 1.0)
		_WornSuitPrimary_RoughnessRemap("WornSuitPrimary_Roughness Remap", Vector) = (PrimeWornRoughMap3.X, PrimeWornRoughMap3.Y, PrimeWornRoughMap3.Z, PrimeWornRoughMap3.W)
		_WornSuitPrimary_DetailDiffuseBlend("WornSuitPrimary_Detail Diffuse Blend", Float) = PrimeWornMatParams3.X
		_WornSuitPrimary_DetailNormalBlend("WornSuitPrimary_Detail Normal Blend", Float) = PrimeWornMatParams3.Y
		_WornSuitPrimary_DetailRoughnessBlend("WornSuitPrimary_Detail Roughness Blend", Float) = PrimeWornMatParams3.Z
		_WornSuitPrimary_Metalness("WornSuitPrimary_Metalness", Float) = PrimeWornMatParams3.W
		_SuitSecondary_Color("SuitSecondary_Color", Color) = (CSecon3.X, CSecon3.Y, CSecon3.Z, 1.0)
		_SuitSecondary_WearRemap("SuitSecondary_Wear Remap", Vector) = (SeconWearMap3.X, SeconWearMap3.Y, SeconWearMap3.Z, SeconWearMap3.W)
		_SuitSecondary_RoughnessRemap("SuitSecondary_Roughness Remap", Vector) = (SeconRoughMap3.X, SeconRoughMap3.Y, SeconRoughMap3.Z, SeconRoughMap3.W)
		_SuitSecondary_DetailDiffuseBlend("SuitSecondary_Detail Diffuse Blend", Float) = SeconMatParams3.X
		_SuitSecondary_DetailNormalBlend("SuitSecondary_Detail Normal Blend", Float) = SeconMatParams3.Y
		_SuitSecondary_DetailRoughnessBlend("SuitSecondary_Detail Roughness Blend", Float) = SeconMatParams3.Z
		_SuitSecondary_Metalness("SuitSecondary_Metalness", Float) = SeconMatParams3.W
		_SuitSecondary_Iridescence("SuitSecondary_Iridescence", Float) = SeconAdvMatParams3.X
		_SuitSecondary_Fuzz("SuitSecondary_Fuzz", Float) = SeconAdvMatParams3.Y
		_SuitSecondary_Transmission("SuitSecondary_Transmission", Float) = SeconAdvMatParams3.Z
		[HDR]_SuitSecondary_Emission("SuitSecondary_Emission", Color) = (CSeconEmit3.X, CSeconEmit3.Y, CSeconEmit3.Z, 1.0)
		_WornSuitSecondary_Color("WornSuitSecondary_Color", Color) = (CSeconWear3.X, CSeconWear3.Y, CSeconWear3.Z, 1.0)
		_WornSuitSecondary_RoughnessRemap("WornSuitSecondary_Roughness Remap", Vector) = (SeconWornRoughMap3.X, SeconWornRoughMap3.Y, SeconWornRoughMap3.Z, SeconWornRoughMap3.W)
		_WornSuitSecondary_DetailDiffuseBlend("WornSuitSecondary_Detail Diffuse Blend", Float) = SeconWornMatParams3.X
		_WornSuitSecondary_DetailNormalBlend("WornSuitSecondary_Detail Normal Blend", Float) = SeconWornMatParams3.Y
		_WornSuitSecondary_DetailRoughnessBlend("WornSuitSecondary_Detail Roughness Blend", Float) = SeconWornMatParams3.Z
		_WornSuitSecondary_Metalness("WornSuitSecondary_Metalness", Float) = SeconWornMatParams3.W
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		
		// Depth prepass
		Pass {
			ZWrite On
			ColorMask 0
		}
		
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			half2 uv_texcoord;
			half2 uv2_texcoord2;
			float4 vertexColor : COLOR;
			float3 worldPos;
			INTERNAL_DATA
			float3 worldNormal;
		};

		struct SurfaceOutputStandardSpecularCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Specular;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
		};

		uniform sampler2D _DiffuseTexture;
		uniform sampler2D _GstackTexture;
		uniform sampler2D _NormalMap;
		uniform sampler2D _DyeSlotTexture;
		UNITY_DECLARE_TEX2D(_DetailDiffuse01);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailDiffuse02);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailDiffuse03);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailDiffuse04);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailDiffuse05);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailDiffuse06);
		UNITY_DECLARE_TEX2D(_DetailNormal01);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailNormal02);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailNormal03);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailNormal04);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailNormal05);
		UNITY_DECLARE_TEX2D_NOSAMPLER(_DetailNormal06);
		uniform half4 _Armor_DetailNormalTransform;
		uniform half _EnableifusingDyeSlotTexture;
		uniform half4 _Cloth_DetailNormalTransform;
		uniform half4 _Suit_DetailNormalTransform;
		uniform half _ArmorPrimary_DetailDiffuseBlend;
		uniform half _ArmorPrimary_DetailNormalBlend;
		uniform half _ArmorPrimary_DetailRoughnessBlend;
		uniform half _ArmorPrimary_Metalness;
		uniform half _ArmorSecondary_DetailDiffuseBlend;
		uniform half _ArmorSecondary_DetailNormalBlend;
		uniform half _ArmorSecondary_DetailRoughnessBlend;
		uniform half _ArmorSecondary_Metalness;
		uniform half _ClothPrimary_DetailDiffuseBlend;
		uniform half _ClothPrimary_DetailNormalBlend;
		uniform half _ClothPrimary_DetailRoughnessBlend;
		uniform half _ClothPrimary_Metalness;
		uniform half _ClothSecondary_DetailDiffuseBlend;
		uniform half _ClothSecondary_DetailNormalBlend;
		uniform half _ClothSecondary_DetailRoughnessBlend;
		uniform half _ClothSecondary_Metalness;
		uniform half _SuitPrimary_DetailDiffuseBlend;
		uniform half _SuitPrimary_DetailNormalBlend;
		uniform half _SuitPrimary_DetailRoughnessBlend;
		uniform half _SuitPrimary_Metalness;
		uniform half _SuitSecondary_DetailDiffuseBlend;
		uniform half _SuitSecondary_DetailNormalBlend;
		uniform half _SuitSecondary_DetailRoughnessBlend;
		uniform half _SuitSecondary_Metalness;
		uniform half _WornArmorPrimary_DetailDiffuseBlend;
		uniform half _WornArmorPrimary_DetailNormalBlend;
		uniform half _WornArmorPrimary_DetailRoughnessBlend;
		uniform half _WornArmorPrimary_Metalness;
		uniform half _WornArmorSecondary_DetailDiffuseBlend;
		uniform half _WornArmorSecondary_DetailNormalBlend;
		uniform half _WornArmorSecondary_DetailRoughnessBlend;
		uniform half _WornArmorSecondary_Metalness;
		uniform half _WornClothPrimary_DetailDiffuseBlend;
		uniform half _WornClothPrimary_DetailNormalBlend;
		uniform half _WornClothPrimary_DetailRoughnessBlend;
		uniform half _WornClothPrimary_Metalness;
		uniform half _WornClothSecondary_DetailDiffuseBlend;
		uniform half _WornClothSecondary_DetailNormalBlend;
		uniform half _WornClothSecondary_DetailRoughnessBlend;
		uniform half _WornClothSecondary_Metalness;
		uniform half _WornSuitPrimary_DetailDiffuseBlend;
		uniform half _WornSuitPrimary_DetailNormalBlend;
		uniform half _WornSuitPrimary_DetailRoughnessBlend;
		uniform half _WornSuitPrimary_Metalness;
		uniform half _WornSuitSecondary_DetailDiffuseBlend;
		uniform half _WornSuitSecondary_DetailNormalBlend;
		uniform half _WornSuitSecondary_DetailRoughnessBlend;
		uniform half _WornSuitSecondary_Metalness;
		uniform half4 _ArmorPrimary_WearRemap;
		uniform half4 _ArmorSecondary_WearRemap;
		uniform half4 _ClothPrimary_WearRemap;
		uniform half4 _ClothSecondary_WearRemap;
		uniform half4 _SuitPrimary_WearRemap;
		uniform half4 _SuitSecondary_WearRemap;
		uniform half4 _ArmorPrimary_Color;
		uniform half4 _ArmorSecondary_Color;
		uniform half4 _ClothPrimary_Color;
		uniform half4 _ClothSecondary_Color;
		uniform half4 _SuitPrimary_Color;
		uniform half4 _SuitSecondary_Color;
		uniform half4 _Armor_DetailDiffuseTransform;
		uniform half4 _Cloth_DetailDiffuseTransform;
		uniform half4 _Suit_DetailDiffuseTransform;
		uniform half4 _WornArmorPrimary_Color;
		uniform half4 _WornArmorSecondary_Color;
		uniform half4 _WornClothPrimary_Color;
		uniform half4 _WornClothSecondary_Color;
		uniform half4 _WornSuitPrimary_Color;
		uniform half4 _WornSuitSecondary_Color;
		uniform sampler2D _Iridescence_Lookup;
		uniform half _ArmorPrimary_Iridescence;
		uniform half _ArmorPrimary_Fuzz;
		uniform half _ArmorPrimary_Transmission;
		uniform half _ArmorSecondary_Iridescence;
		uniform half _ArmorSecondary_Fuzz;
		uniform half _ArmorSecondary_Transmission;
		uniform half _ClothPrimary_Iridescence;
		uniform half _ClothPrimary_Fuzz;
		uniform half _ClothPrimary_Transmission;
		uniform half _ClothSecondary_Iridescence;
		uniform half _ClothSecondary_Fuzz;
		uniform half _ClothSecondary_Transmission;
		uniform half _SuitPrimary_Iridescence;
		uniform half _SuitPrimary_Fuzz;
		uniform half _SuitPrimary_Transmission;
		uniform half _SuitSecondary_Iridescence;
		uniform half _SuitSecondary_Fuzz;
		uniform half _SuitSecondary_Transmission;
		uniform half4 _ArmorPrimary_Emission;
		uniform half4 _ArmorSecondary_Emission;
		uniform half4 _ClothPrimary_Emission;
		uniform half4 _ClothSecondary_Emission;
		uniform half4 _SuitPrimary_Emission;
		uniform half4 _SuitSecondary_Emission;
		uniform half4 _ArmorPrimary_RoughnessRemap;
		uniform half4 _ArmorSecondary_RoughnessRemap;
		uniform half4 _ClothPrimary_RoughnessRemap;
		uniform half4 _ClothSecondary_RoughnessRemap;
		uniform half4 _SuitPrimary_RoughnessRemap;
		uniform half4 _SuitSecondary_RoughnessRemap;
		uniform half4 _WornArmorPrimary_RoughnessRemap;
		uniform half4 _WornArmorSecondary_RoughnessRemap;
		uniform half4 _WornClothPrimary_RoughnessRemap;
		uniform half4 _WornClothSecondary_RoughnessRemap;
		uniform half4 _WornSuitPrimary_RoughnessRemap;
		uniform half4 _WornSuitSecondary_RoughnessRemap;
		uniform half _Maskclipvalue;

		inline half4 LightingStandardSpecularCustom(SurfaceOutputStandardSpecularCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandardSpecular r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Specular = s.Specular;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandardSpecular (r, viewDir, gi) + d;
		}

		inline void LightingStandardSpecularCustom_GI(SurfaceOutputStandardSpecularCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardSpecularCustom o )
		{
			float2 uv_NormalMap545 = i.uv_texcoord;
			float3 break694 = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap545 ), -1.0 );
			float3 appendResult695 = (half3(break694.x , break694.y , 1.0));
			float4 color700 = IsGammaSpace() ? half4(0,0,1,0) : half4(0,0,1,0);
			float4 break842 = _Armor_DetailNormalTransform;
			float2 appendResult844 = (half2(break842.x , break842.y));
			float2 appendResult843 = (half2(break842.z , break842.w));
			float2 uv2_TexCoord118 = i.uv2_texcoord2 * appendResult844 + appendResult843;
			float2 uv_DyeSlotTexture708 = i.uv_texcoord;
			float4 break709 = tex2D( _DyeSlotTexture, uv_DyeSlotTexture708 );
			half ifLocalVar710 = 0;
			if( break709.r > 0.214 )
				ifLocalVar710 = 1.0;
			half ifLocalVar712 = 0;
			if( break709.g > 0.214 )
				ifLocalVar712 = 1.0;
			float clampResult718 = clamp( ( ifLocalVar710 + ifLocalVar712 ) , 0.0 , 1.0 );
			float clampResult720 = clamp( ( ifLocalVar710 - ifLocalVar712 ) , 0.0 , 1.0 );
			half ifLocalVar713 = 0;
			if( break709.b > 0.214 )
				ifLocalVar713 = 1.0;
			float temp_output_716_0 = ( 1.0 - ifLocalVar713 );
			float temp_output_714_0 = ( 1.0 - ifLocalVar710 );
			float clampResult725 = clamp( ( ( ifLocalVar712 * temp_output_716_0 ) * temp_output_714_0 ) , 0.0 , 1.0 );
			float clampResult728 = clamp( ( ( ifLocalVar710 * ifLocalVar712 ) * temp_output_716_0 ) , 0.0 , 1.0 );
			float clampResult731 = clamp( ( temp_output_714_0 * ifLocalVar713 ) , 0.0 , 1.0 );
			float clampResult734 = clamp( ( ( ifLocalVar710 * ifLocalVar712 ) * ifLocalVar713 ) , 0.0 , 1.0 );
			float temp_output_747_0 = floor( ( ( ( ( ( ( 1.0 - clampResult718 ) + ( clampResult720 * 2.0 ) ) + ( clampResult725 * 3.0 ) ) + ( clampResult728 * 4.0 ) ) + ( clampResult731 * 5.0 ) ) + ( clampResult734 * 6.0 ) ) );
			float2 appendResult749 = (half2(( temp_output_747_0 * 0.333 ) , ( ( temp_output_747_0 - 3.0 ) * 0.333 )));
			float4 break706 = lerp(i.vertexColor,half4( appendResult749, 0.0 , 0.0 ),_EnableifusingDyeSlotTexture);
			half ifLocalVar5 = 0;
			if( break706.r > 0.3334 )
				ifLocalVar5 = 1.0;
			float3 lerpResult553 = lerp( UnpackScaleNormal( UNITY_SAMPLE_TEX2D( _DetailNormal01, uv2_TexCoord118 ), -1.0 ) , UnpackScaleNormal( UNITY_SAMPLE_TEX2D_SAMPLER( _DetailNormal02, _DetailNormal01, uv2_TexCoord118 ), -1.0 ) , ifLocalVar5);
			float4 break846 = _Cloth_DetailNormalTransform;
			float2 appendResult848 = (half2(break846.x , break846.y));
			float2 appendResult847 = (half2(break846.z , break846.w));
			float2 uv2_TexCoord119 = i.uv2_texcoord2 * appendResult848 + appendResult847;
			half ifLocalVar9 = 0;
			if( break706.r > 0.998 )
				ifLocalVar9 = 1.0;
			float3 lerpResult554 = lerp( lerpResult553 , UnpackNormal( UNITY_SAMPLE_TEX2D_SAMPLER( _DetailNormal03, _DetailNormal01, uv2_TexCoord119 ) ) , ifLocalVar9);
			half ifLocalVar10 = 0;
			if( break706.g < 0.3334 )
				ifLocalVar10 = 1.0;
			half ifLocalVar11 = 0;
			if( break706.g > 0.09 )
				ifLocalVar11 = 1.0;
			float temp_output_12_0 = ( ifLocalVar10 * ifLocalVar11 );
			float3 lerpResult555 = lerp( lerpResult554 , UnpackNormal( UNITY_SAMPLE_TEX2D_SAMPLER( _DetailNormal04, _DetailNormal01, uv2_TexCoord119 ) ) , temp_output_12_0);
			float4 break850 = _Suit_DetailNormalTransform;
			float2 appendResult852 = (half2(break850.x , break850.y));
			float2 appendResult851 = (half2(break850.z , break850.w));
			float2 uv2_TexCoord120 = i.uv2_texcoord2 * appendResult852 + appendResult851;
			half ifLocalVar13 = 0;
			if( break706.g < 0.667 )
				ifLocalVar13 = 1.0;
			half ifLocalVar14 = 0;
			if( break706.g > 0.3334 )
				ifLocalVar14 = 1.0;
			float temp_output_16_0 = ( ifLocalVar13 * ifLocalVar14 );
			float3 lerpResult556 = lerp( lerpResult555 , UnpackNormal( UNITY_SAMPLE_TEX2D_SAMPLER( _DetailNormal05, _DetailNormal01, uv2_TexCoord120 ) ) , temp_output_16_0);
			half ifLocalVar15 = 0;
			if( break706.g > 0.998 )
				ifLocalVar15 = 1.0;
			float3 lerpResult557 = lerp( lerpResult556 , UnpackNormal( UNITY_SAMPLE_TEX2D_SAMPLER( _DetailNormal06, _DetailNormal01, uv2_TexCoord120 ) ) , ifLocalVar15);
			float3 break654 = lerpResult557;
			float3 appendResult692 = (half3(break654.x , break654.y , 1.0));
			float4 appendResult389 = (half4(_ArmorPrimary_DetailDiffuseBlend , _ArmorPrimary_DetailNormalBlend , _ArmorPrimary_DetailRoughnessBlend , _ArmorPrimary_Metalness));
			float4 appendResult392 = (half4(_ArmorSecondary_DetailDiffuseBlend , _ArmorSecondary_DetailNormalBlend , _ArmorSecondary_DetailRoughnessBlend , _ArmorSecondary_Metalness));
			float4 lerpResult369 = lerp( appendResult389 , appendResult392 , ifLocalVar5);
			float4 appendResult395 = (half4(_ClothPrimary_DetailDiffuseBlend , _ClothPrimary_DetailNormalBlend , _ClothPrimary_DetailRoughnessBlend , _ClothPrimary_Metalness));
			float4 lerpResult368 = lerp( lerpResult369 , appendResult395 , ifLocalVar9);
			float4 appendResult400 = (half4(_ClothSecondary_DetailDiffuseBlend , _ClothSecondary_DetailNormalBlend , _ClothSecondary_DetailRoughnessBlend , _ClothSecondary_Metalness));
			float4 lerpResult374 = lerp( lerpResult368 , appendResult400 , temp_output_12_0);
			float4 appendResult401 = (half4(_SuitPrimary_DetailDiffuseBlend , _SuitPrimary_DetailNormalBlend , _SuitPrimary_DetailRoughnessBlend , _SuitPrimary_Metalness));
			float4 lerpResult373 = lerp( lerpResult374 , appendResult401 , temp_output_16_0);
			float4 appendResult404 = (half4(_SuitSecondary_DetailDiffuseBlend , _SuitSecondary_DetailNormalBlend , _SuitSecondary_DetailRoughnessBlend , _SuitSecondary_Metalness));
			float4 lerpResult371 = lerp( lerpResult373 , appendResult404 , ifLocalVar15);
			float4 clampResult701 = clamp( lerpResult371 , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 break415 = clampResult701;
			float4 appendResult391 = (half4(_WornArmorPrimary_DetailDiffuseBlend , _WornArmorPrimary_DetailNormalBlend , _WornArmorPrimary_DetailRoughnessBlend , _WornArmorPrimary_Metalness));
			float4 appendResult394 = (half4(_WornArmorSecondary_DetailDiffuseBlend , _WornArmorSecondary_DetailNormalBlend , _WornArmorSecondary_DetailRoughnessBlend , _WornArmorSecondary_Metalness));
			float4 lerpResult408 = lerp( appendResult391 , appendResult394 , ifLocalVar5);
			float4 appendResult397 = (half4(_WornClothPrimary_DetailDiffuseBlend , _WornClothPrimary_DetailNormalBlend , _WornClothPrimary_DetailRoughnessBlend , _WornClothPrimary_Metalness));
			float4 lerpResult407 = lerp( lerpResult408 , appendResult397 , ifLocalVar9);
			float4 appendResult398 = (half4(_WornClothSecondary_DetailDiffuseBlend , _WornClothSecondary_DetailNormalBlend , _WornClothSecondary_DetailRoughnessBlend , _WornClothSecondary_Metalness));
			float4 lerpResult413 = lerp( lerpResult407 , appendResult398 , temp_output_12_0);
			float4 appendResult403 = (half4(_WornSuitPrimary_DetailDiffuseBlend , _WornSuitPrimary_DetailNormalBlend , _WornSuitPrimary_DetailRoughnessBlend , _WornSuitPrimary_Metalness));
			float4 lerpResult412 = lerp( lerpResult413 , appendResult403 , temp_output_16_0);
			float4 appendResult406 = (half4(_WornSuitSecondary_DetailDiffuseBlend , _WornSuitSecondary_DetailNormalBlend , _WornSuitSecondary_DetailRoughnessBlend , _WornSuitSecondary_Metalness));
			float4 lerpResult410 = lerp( lerpResult412 , appendResult406 , ifLocalVar15);
			float4 clampResult702 = clamp( lerpResult410 , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 break454 = clampResult702;
			float2 uv_GstackTexture2 = i.uv_texcoord;
			half4 tex2DNode2 = tex2D( _GstackTexture, uv_GstackTexture2 );
			float clampResult464 = clamp( ( ( ( 1.0 / ( 1.0 - 0.1568628 ) ) * tex2DNode2.a ) + ( 0.1568628 / ( 0.1568628 - 1.0 ) ) ) , 0.0 , 1.0 );
			float4 lerpResult362 = lerp( _ArmorPrimary_WearRemap , _ArmorSecondary_WearRemap , ifLocalVar5);
			float4 lerpResult361 = lerp( lerpResult362 , _ClothPrimary_WearRemap , ifLocalVar9);
			float4 lerpResult367 = lerp( lerpResult361 , _ClothSecondary_WearRemap , temp_output_12_0);
			float4 lerpResult366 = lerp( lerpResult367 , _SuitPrimary_WearRemap , temp_output_16_0);
			float4 lerpResult364 = lerp( lerpResult366 , _SuitSecondary_WearRemap , ifLocalVar15);
			half ifLocalVar47 = 0;
			if( tex2DNode2.a > 0.157 )
				ifLocalVar47 = 1.0;
			float4 lerpResult363 = lerp( float4( 0,0,0,0 ) , lerpResult364 , ifLocalVar47);
			float4 break466 = lerpResult363;
			float clampResult469 = clamp( ( ( clampResult464 * break466.y ) + break466.x ) , break466.z , ( break466.z + break466.w ) );
			float temp_output_470_0 = ( 1.0 - clampResult469 );
			float lerpResult788 = lerp( break415.y , break454.y , temp_output_470_0);
			float4 lerpResult699 = lerp( color700 , half4( appendResult692 , 0.0 ) , ( lerpResult788 * ifLocalVar47 ));
			float3 temp_output_542_0 = BlendNormals( appendResult695 , lerpResult699.rgb );
			o.Normal = temp_output_542_0;
			float2 uv_DiffuseTexture561 = i.uv_texcoord;
			half4 tex2DNode561 = tex2D( _DiffuseTexture, uv_DiffuseTexture561 );
			float4 lerpResult574 = lerp( _ArmorPrimary_Color , _ArmorSecondary_Color , ifLocalVar5);
			float4 lerpResult575 = lerp( lerpResult574 , _ClothPrimary_Color , ifLocalVar9);
			float4 lerpResult576 = lerp( lerpResult575 , _ClothSecondary_Color , temp_output_12_0);
			float4 lerpResult577 = lerp( lerpResult576 , _SuitPrimary_Color , temp_output_16_0);
			float4 lerpResult578 = lerp( lerpResult577 , _SuitSecondary_Color , ifLocalVar15);
			float4 clampResult590 = clamp( ( tex2DNode561 * 4.0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			half4 temp_cast_3 = (0.25).xxxx;
			float4 clampResult594 = clamp( ( tex2DNode561 - temp_cast_3 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 temp_output_595_0 = ( ( lerpResult578 * clampResult590 ) + clampResult594 );
			float4 break830 = _Armor_DetailDiffuseTransform;
			float2 appendResult831 = (half2(break830.x , break830.y));
			float2 appendResult832 = (half2(break830.z , break830.w));
			float2 uv2_TexCoord115 = i.uv2_texcoord2 * appendResult831 + appendResult832;
			half4 tex2DNode93 = UNITY_SAMPLE_TEX2D( _DetailDiffuse01, uv2_TexCoord115 );
			half4 tex2DNode94 = UNITY_SAMPLE_TEX2D_SAMPLER( _DetailDiffuse02, _DetailDiffuse01, uv2_TexCoord115 );
			float4 lerpResult684 = lerp( tex2DNode93 , tex2DNode94 , ifLocalVar5);
			float4 break834 = _Cloth_DetailDiffuseTransform;
			float2 appendResult836 = (half2(break834.x , break834.y));
			float2 appendResult835 = (half2(break834.z , break834.w));
			float2 uv2_TexCoord116 = i.uv2_texcoord2 * appendResult836 + appendResult835;
			half4 tex2DNode95 = UNITY_SAMPLE_TEX2D_SAMPLER( _DetailDiffuse03, _DetailDiffuse01, uv2_TexCoord116 );
			float4 lerpResult685 = lerp( lerpResult684 , tex2DNode95 , ifLocalVar9);
			half4 tex2DNode96 = UNITY_SAMPLE_TEX2D_SAMPLER( _DetailDiffuse04, _DetailDiffuse01, uv2_TexCoord116 );
			float4 lerpResult686 = lerp( lerpResult685 , tex2DNode96 , temp_output_12_0);
			float4 break838 = _Suit_DetailDiffuseTransform;
			float2 appendResult840 = (half2(break838.x , break838.y));
			float2 appendResult839 = (half2(break838.z , break838.w));
			float2 uv2_TexCoord117 = i.uv2_texcoord2 * appendResult840 + appendResult839;
			half4 tex2DNode97 = UNITY_SAMPLE_TEX2D_SAMPLER( _DetailDiffuse05, _DetailDiffuse01, uv2_TexCoord117 );
			float4 lerpResult687 = lerp( lerpResult686 , tex2DNode97 , temp_output_16_0);
			half4 tex2DNode560 = UNITY_SAMPLE_TEX2D_SAMPLER( _DetailDiffuse06, _DetailDiffuse01, uv2_TexCoord117 );
			float4 lerpResult688 = lerp( lerpResult687 , tex2DNode560 , ifLocalVar15);
			float4 clampResult610 = clamp( ( temp_output_595_0 * 4.0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			half4 temp_cast_4 = (0.25).xxxx;
			float4 clampResult612 = clamp( ( temp_output_595_0 - temp_cast_4 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 lerpResult624 = lerp( temp_output_595_0 , ( ( lerpResult688 * clampResult610 ) + clampResult612 ) , break415.x);
			float4 lerpResult581 = lerp( _WornArmorPrimary_Color , _WornArmorSecondary_Color , ifLocalVar5);
			float4 lerpResult582 = lerp( lerpResult581 , _WornClothPrimary_Color , ifLocalVar9);
			float4 lerpResult583 = lerp( lerpResult582 , _WornClothSecondary_Color , temp_output_12_0);
			float4 lerpResult584 = lerp( lerpResult583 , _WornSuitPrimary_Color , temp_output_16_0);
			float4 lerpResult585 = lerp( lerpResult584 , _WornSuitSecondary_Color , ifLocalVar15);
			float4 clampResult601 = clamp( ( tex2DNode561 * 4.0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			half4 temp_cast_5 = (0.25).xxxx;
			float4 clampResult603 = clamp( ( tex2DNode561 - temp_cast_5 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 temp_output_604_0 = ( ( lerpResult585 * clampResult601 ) + clampResult603 );
			float4 clampResult619 = clamp( ( temp_output_604_0 * 4.0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			half4 temp_cast_6 = (0.25).xxxx;
			float4 clampResult621 = clamp( ( temp_output_604_0 - temp_cast_6 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult625 = lerp( temp_output_604_0 , ( ( lerpResult688 * clampResult619 ) + clampResult621 ) , break454.x);
			float4 lerpResult626 = lerp( lerpResult624 , lerpResult625 , temp_output_470_0);
			float4 lerpResult627 = lerp( tex2DNode561 , lerpResult626 , ifLocalVar47);
			float3 ase_worldPos = i.worldPos;
			half3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			half3 ase_worldNormal = WorldNormalVector( i, half3( 0, 0, 1 ) );
			half3 ase_worldTangent = WorldNormalVector( i, half3( 1, 0, 0 ) );
			half3 ase_worldBitangent = WorldNormalVector( i, half3( 0, 1, 0 ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float fresnelNdotV637 = dot( mul(ase_tangentToWorldFast,temp_output_542_0), ase_worldViewDir );
			float fresnelNode637 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV637, 1.0 ) );
			float4 appendResult390 = (half4(_ArmorPrimary_Iridescence , _ArmorPrimary_Fuzz , _ArmorPrimary_Transmission , 0.0));
			float4 appendResult393 = (half4(_ArmorSecondary_Iridescence , _ArmorSecondary_Fuzz , _ArmorSecondary_Transmission , 0.0));
			float4 lerpResult376 = lerp( appendResult390 , appendResult393 , ifLocalVar5);
			float3 appendResult396 = (half3(_ClothPrimary_Iridescence , _ClothPrimary_Fuzz , _ClothPrimary_Transmission));
			float4 lerpResult375 = lerp( lerpResult376 , half4( appendResult396 , 0.0 ) , ifLocalVar9);
			float4 appendResult399 = (half4(_ClothSecondary_Iridescence , _ClothSecondary_Fuzz , _ClothSecondary_Transmission , 0.0));
			float4 lerpResult381 = lerp( lerpResult375 , appendResult399 , temp_output_12_0);
			float4 appendResult402 = (half4(_SuitPrimary_Iridescence , _SuitPrimary_Fuzz , _SuitPrimary_Transmission , 0.0));
			float4 lerpResult380 = lerp( lerpResult381 , appendResult402 , temp_output_16_0);
			float4 appendResult405 = (half4(_SuitSecondary_Iridescence , _SuitSecondary_Fuzz , _SuitSecondary_Transmission , 0.0));
			float4 lerpResult378 = lerp( lerpResult380 , appendResult405 , ifLocalVar15);
			float4 break484 = lerpResult378;
			float temp_output_499_0 = floor( break484.x );
			float3 appendResult636 = (half3(( 1.0 - fresnelNode637 ) , ( ( 128.0 - ( temp_output_499_0 + 0.5 ) ) / 128.0 ) , 1.0));
			half4 tex2DNode631 = tex2D( _Iridescence_Lookup, appendResult636.xy );
			float grayscale670 = dot(lerpResult578.rgb, float3(0.299,0.587,0.114));
			float temp_output_672_0 = ( 1.0 - grayscale670 );
			float4 lerpResult629 = lerp( float4( 0,0,0,0 ) , tex2DNode631 , temp_output_672_0);
			float clampResult504 = clamp( ( frac( ( ( temp_output_499_0 + 1.0 ) / 2.0 ) ) * 10.0 ) , 0.0 , 1.0 );
			half ifLocalVar497 = 0;
			if( break484.x > -0.0001 )
				ifLocalVar497 = 1.0;
			float temp_output_505_0 = ( clampResult504 * ifLocalVar497 );
			float4 lerpResult628 = lerp( lerpResult627 , lerpResult629 , temp_output_505_0);
			float4 lerpResult630 = lerp( lerpResult627 , lerpResult628 , ifLocalVar47);
			float clampResult46 = clamp( ( ( ( 1.0 / ( 0.12549 - 0.0 ) ) * tex2DNode2.a ) + ( 0.0 / ( 0.0 - 0.12549 ) ) ) , 0.0 , 1.0 );
			float lerpResult515 = lerp( break415.w , break454.w , temp_output_470_0);
			float lerpResult514 = lerp( clampResult46 , lerpResult515 , ifLocalVar47);
			float lerpResult524 = lerp( lerpResult514 , 1.0 , temp_output_505_0);
			float clampResult797 = clamp( break484.y , 0.0 , 1.0 );
			o.Albedo = ( ( lerpResult630 * ( 1.0 - lerpResult524 ) ) + ( fresnelNode637 * ifLocalVar47 * clampResult797 * 0.3 ) ).rgb;
			float clampResult764 = clamp( ( ( ( 1.0 / ( 1.0 - 0.157 ) ) * tex2DNode2.b ) + ( ( 0.157 - 1.0 ) / 0.157 ) ) , 0.0 , 1.0 );
			float4 lerpResult774 = lerp( _ArmorPrimary_Emission , _ArmorSecondary_Emission , ifLocalVar5);
			float4 lerpResult775 = lerp( lerpResult774 , _ClothPrimary_Emission , ifLocalVar9);
			float4 lerpResult776 = lerp( lerpResult775 , _ClothSecondary_Emission , temp_output_12_0);
			float4 lerpResult777 = lerp( lerpResult776 , _SuitPrimary_Emission , temp_output_16_0);
			float4 lerpResult778 = lerp( lerpResult777 , _SuitSecondary_Emission , ifLocalVar15);
			o.Emission = ( ( clampResult764 * lerpResult778 ) * 5.0 ).rgb;
			float lerpResult825 = lerp( 0.02 , 0.0 , clampResult46);
			half4 temp_cast_13 = (lerpResult825).xxxx;
			float4 lerpResult820 = lerp( ( tex2DNode631 * temp_output_672_0 ) , float4( 0,0,0,0 ) , lerpResult514);
			half4 temp_cast_14 = (0.02).xxxx;
			float clampResult815 = clamp( ( break484.x * -1.0 ) , 0.0 , 1.0 );
			float4 lerpResult821 = lerp( lerpResult820 , temp_cast_14 , clampResult815);
			float4 lerpResult823 = lerp( lerpResult821 , float4( 0,0,0,0 ) , temp_output_505_0);
			float4 lerpResult824 = lerp( lerpResult823 , float4( 0,0,0,0 ) , lerpResult514);
			float4 lerpResult826 = lerp( temp_cast_13 , lerpResult824 , ifLocalVar47);
			o.Specular = ( ( lerpResult630 * lerpResult524 ) + ( lerpResult826 * ( break694.z * break654.z ) ) ).rgb;
			float clampResult426 = clamp( ( tex2DNode2.g * 4.0 ) , 0.0 , 1.0 );
			float lerpResult435 = lerp( tex2DNode93.a , tex2DNode94.a , ifLocalVar5);
			float lerpResult436 = lerp( lerpResult435 , tex2DNode95.a , ifLocalVar9);
			float lerpResult437 = lerp( lerpResult436 , tex2DNode96.a , temp_output_12_0);
			float lerpResult438 = lerp( lerpResult437 , tex2DNode97.a , temp_output_16_0);
			float lerpResult440 = lerp( lerpResult438 , tex2DNode560.a , ifLocalVar15);
			float clampResult423 = clamp( ( tex2DNode2.g - 0.25 ) , 0.0 , 1.0 );
			float lerpResult451 = lerp( tex2DNode2.g , ( ( clampResult426 * lerpResult440 ) + clampResult423 ) , break415.z);
			float4 lerpResult185 = lerp( _ArmorPrimary_RoughnessRemap , _ArmorSecondary_RoughnessRemap , ifLocalVar5);
			float4 lerpResult186 = lerp( lerpResult185 , _ClothPrimary_RoughnessRemap , ifLocalVar9);
			float4 lerpResult187 = lerp( lerpResult186 , _ClothSecondary_RoughnessRemap , temp_output_12_0);
			float4 lerpResult188 = lerp( lerpResult187 , _SuitPrimary_RoughnessRemap , temp_output_16_0);
			float4 lerpResult190 = lerp( lerpResult188 , _SuitSecondary_RoughnessRemap , ifLocalVar15);
			float4 break471 = lerpResult190;
			float clampResult476 = clamp( ( ( lerpResult451 * break471.y ) + break471.x ) , break471.z , ( break471.z + break471.w ) );
			float clampResult450 = clamp( ( tex2DNode2.g * 4.0 ) , 0.0 , 1.0 );
			float clampResult447 = clamp( ( tex2DNode2.g - 0.25 ) , 0.0 , 1.0 );
			float lerpResult452 = lerp( tex2DNode2.g , ( ( clampResult450 * lerpResult440 ) + clampResult447 ) , break454.z);
			float4 lerpResult383 = lerp( _WornArmorPrimary_RoughnessRemap , _WornArmorSecondary_RoughnessRemap , ifLocalVar5);
			float4 lerpResult382 = lerp( lerpResult383 , _WornClothPrimary_RoughnessRemap , ifLocalVar9);
			float4 lerpResult388 = lerp( lerpResult382 , _WornClothSecondary_RoughnessRemap , temp_output_12_0);
			float4 lerpResult387 = lerp( lerpResult388 , _WornSuitPrimary_RoughnessRemap , temp_output_16_0);
			float4 lerpResult385 = lerp( lerpResult387 , _WornSuitSecondary_RoughnessRemap , ifLocalVar15);
			float4 break453 = lerpResult385;
			float clampResult480 = clamp( ( ( lerpResult452 * break453.y ) + break453.x ) , break453.z , ( break453.z + break453.w ) );
			float lerpResult481 = lerp( clampResult476 , clampResult480 , temp_output_470_0);
			float lerpResult482 = lerp( tex2DNode2.g , lerpResult481 , ifLocalVar47);
			float clampResult679 = clamp( lerpResult482 , 0.0 , 0.89 );
			o.Smoothness = clampResult679;
			o.Occlusion = tex2DNode2.r;
			float clampResult798 = clamp( break484.z , 0.0 , 1.0 );
			half3 temp_cast_16 = (( clampResult798 * ifLocalVar47 )).xxx;
			o.Transmission = temp_cast_16;
			o.Alpha = 1;
			//float clampResult130 = clamp( ( ( ( 1.0 / ( 0.055 - 0.02 ) ) * pow(tex2DNode2.b,1/2.2) ) + ( ( 0.02 - 0.055 ) / 0.02 ) ) , 0.0 , 1.0 );
			float clampResult130 = pow(tex2DNode2.b,1/2.2) < 0.12549019607 ? pow(tex2DNode2.b,1/2.2) : 0.12549019607;
			//float lerpResult134 = lerp( 1.0 , clampResult130 , ( i.vertexColor.b * 30.0 ));
			float lerpResult134 = i.vertexColor.b > 0.2 ? clampResult130 * 7.96875 : 1.0;
			clip( lerpResult134 - _Maskclipvalue );
			if (_Maskclipvalue == 0)
				o.Alpha = lerpResult134;
		}

		ENDCG
		CGPROGRAM
		#pragma only_renderers d3d11 
		#pragma surface surf StandardSpecularCustom alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack1.zw = customInputData.uv2_texcoord2;
				o.customPack1.zw = v.texcoord1;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv2_texcoord2 = IN.customPack1.zw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandardSpecularCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecularCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16500
3;27;1352;679;-1697.78;3914.63;2.547992;True;True
Node;AmplifyShaderEditor.CommentaryNode;786;-7256.511,-3352.299;Float;False;3032.172;1120.935;Comment;39;708;709;711;713;710;712;716;717;719;721;714;720;718;724;726;727;736;735;725;737;729;732;728;741;743;733;731;738;739;744;734;745;740;746;747;750;748;751;749;DyeSlot Converter;0.4191176,0.6394523,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;708;-7206.511,-2807.299;Float;True;Property;_DyeSlotTexture;DyeSlot Texture;3;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;709;-6854.511,-2790.299;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;711;-6774.511,-2646.299;Float;False;Constant;_Float13;Float 13;114;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;713;-6502.574,-2504.329;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.214;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;710;-6502.511,-2822.299;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.214;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;716;-6240.766,-2529.658;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;712;-6502.511,-2662.299;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.214;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;714;-6255.511,-2858.299;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;717;-5990.511,-3302.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;721;-5990.511,-3046.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;719;-5990.511,-3174.299;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;726;-5990.511,-2902.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;718;-5686.509,-3302.299;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;720;-5686.509,-3174.299;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;724;-5846.51,-3030.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;736;-5334.508,-3142.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;735;-5366.508,-3270.299;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;725;-5686.509,-3030.299;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;727;-5825.557,-2885.793;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;737;-5350.508,-3014.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;728;-5670.51,-2910.552;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;732;-5990.511,-2614.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;729;-5958.511,-2774.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;741;-5126.509,-3190.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;738;-5334.508,-2902.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;743;-5126.509,-3110.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;733;-5782.509,-2614.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;731;-5670.509,-2774.299;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;734;-5590.509,-2614.299;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;744;-5126.509,-3014.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;739;-5334.508,-2790.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;745;-5126.509,-2934.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;740;-5318.508,-2678.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;746;-5126.509,-2837.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;747;-4928.51,-2875.299;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;750;-4754.511,-2837.299;Float;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;751;-4594.513,-2831.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.333;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;748;-4753.511,-2933.299;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.333;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;4;-3852.928,-2374.859;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;749;-4391.339,-2879.534;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;787;-2122.899,-8180.208;Float;False;3312.466;3110.557;Comment;76;153;195;224;159;194;253;165;223;369;193;282;311;252;368;222;376;374;251;281;362;310;280;373;361;375;177;381;367;309;371;197;380;701;366;226;255;378;364;415;152;171;284;192;363;196;408;313;407;221;225;383;185;250;254;413;412;283;279;186;382;187;388;308;410;312;387;188;702;385;190;471;453;372;370;454;466;Material Parameters;1,1,1,1;0;0
Node;AmplifyShaderEditor.ToggleSwitchNode;752;-3508.965,-2382.688;Float;False;Property;_EnableifusingDyeSlotTexture;Enable if using DyeSlot Texture -->;4;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;706;-3096.57,-2614.037;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;193;-1574.786,-7797.509;Float;False;473.6343;315.1763;ArmorSecondary_Wear Remap;1;346;;1,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;165;-2070.528,-7794.133;Float;False;473.6343;315.1763;ArmorPrimary_Wear Remap;1;342;;1,0,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2932.402,-3279.078;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;63;-2757.536,-2888.072;Float;False;793.2109;1186.961;Slots;9;5;9;10;11;14;12;13;16;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ConditionalIfNode;5;-2403.531,-2838.072;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.3334;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;472;-68.1689,484.451;Float;False;1798.299;516.7002;Wear Remap;6;455;465;467;468;469;470;Wear Remap;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;222;-954.4841,-7796.484;Float;False;473.6343;315.1763;ClothPrimary_Wear Remap;1;350;;0,1,0,1;0;0
Node;AmplifyShaderEditor.Vector4Node;342;-2062.146,-7755.672;Float;False;Property;_ArmorPrimary_WearRemap;ArmorPrimary_Wear Remap;25;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;346;-1562.088,-7754.91;Float;False;Property;_ArmorSecondary_WearRemap;ArmorSecondary_Wear Remap;42;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;251;-455.1203,-7802.139;Float;False;473.6343;315.1763;ClothSecondary_Wear Remap;1;353;;0,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;350;-936.3066,-7750.469;Float;False;Property;_ClothPrimary_WearRemap;ClothPrimary_Wear Remap;59;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;11;-2405.312,-2371.83;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.09;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;9;-2405.919,-2683.622;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.998;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;455;-18.16892,688.5385;Float;False;943.1265;287;Wear Mask;9;464;463;462;461;460;459;458;457;456;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ConditionalIfNode;10;-2405.886,-2529.821;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.3334;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;362;-1143.919,-6076.059;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;195;-1573.109,-7472.822;Float;False;473.6343;315.1763;ArmorSecondary_Material Params;5;219;209;208;207;392;;1,1,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;456;3.832052,850.5334;Float;False;Constant;_Float30;Float 30;9;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;197;-1569.545,-6494.698;Float;False;470.6343;315.1763;ArmorSecondary_Worn Material Parameters;5;217;216;214;213;394;;0.5019608,0.5019608,0,1;0;0
Node;AmplifyShaderEditor.Vector4Node;353;-438.2123,-7745.687;Float;False;Property;_ClothSecondary_WearRemap;ClothSecondary_Wear Remap;76;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-2133.324,-2446.435;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;841;-3635.72,896.9597;Float;False;Property;_Armor_DetailNormalTransform;Armor_Detail Normal Transform;19;0;Create;True;0;0;False;0;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;153;-2070.498,-7471.092;Float;False;473.6343;315.1763;ArmorPrimary_Material Params;5;157;156;155;154;389;;1,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;280;163.1888,-7809.405;Float;False;473.6343;315.1763;SuitPrimary_Wear Remap;1;356;;0,0,1,1;0;0
Node;AmplifyShaderEditor.ConditionalIfNode;14;-2404.102,-2058.402;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.3334;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;13;-2404.344,-2215.389;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.667;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;361;-1144.112,-5968.662;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;177;-2066.934,-6492.968;Float;False;470.6343;315.1763;ArmorPrimary_Worn Material Parameters;5;179;181;180;182;391;;0.5019608,0,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;457;-2.167952,765.5364;Float;False;Constant;_Float31;Float 31;9;0;Create;True;0;0;False;0;0.1568628;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;182;-2061.635,-6251.792;Float;False;Property;_WornArmorPrimary_Metalness;WornArmorPrimary_Metalness;40;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;-1564.246,-6389.522;Float;False;Property;_WornArmorSecondary_DetailNormalBlend;WornArmorSecondary_Detail Normal Blend;56;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;207;-1568.109,-7435.822;Float;False;Property;_ArmorSecondary_DetailDiffuseBlend;ArmorSecondary_Detail Diffuse Blend;44;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-2065.199,-7298.916;Float;False;Property;_ArmorPrimary_DetailRoughnessBlend;ArmorPrimary_Detail Roughness Blend;29;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;309;662.5524,-7812.629;Float;False;473.6343;315.1763;SuitSecondary_Wear Remap;1;359;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;214;-1564.545,-6458.698;Float;False;Property;_WornArmorSecondary_DetailDiffuseBlend;WornArmorSecondary_Detail Diffuse Blend;54;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;213;-1565.246,-6322.522;Float;False;Property;_WornArmorSecondary_DetailRoughnessBlend;WornArmorSecondary_Detail Roughness Blend;55;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;226;-949.2431,-6493.674;Float;False;470.6343;315.1763;ClothPrimary_Worn Material Parameters;5;246;245;243;242;397;;0,0.5019608,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-2068.498,-7435.092;Float;False;Property;_ArmorPrimary_DetailDiffuseBlend;ArmorPrimary_Detail Diffuse Blend;27;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;845;-3645.41,1154.01;Float;False;Property;_Cloth_DetailNormalTransform;Cloth_Detail Normal Transform;21;0;Create;True;0;0;False;0;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;842;-3327.712,931.9196;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;155;-2065.199,-7364.916;Float;False;Property;_ArmorPrimary_DetailNormalBlend;ArmorPrimary_Detail Normal Blend;28;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-2060.635,-6387.792;Float;False;Property;_WornArmorPrimary_DetailNormalBlend;WornArmorPrimary_Detail Normal Blend;38;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-2063.199,-7228.916;Float;False;Property;_ArmorPrimary_Metalness;ArmorPrimary_Metalness;30;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;208;-1567.81,-7367.646;Float;False;Property;_ArmorSecondary_DetailNormalBlend;ArmorSecondary_Detail Normal Blend;45;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;179;-2063.934,-6455.968;Float;False;Property;_WornArmorPrimary_DetailDiffuseBlend;WornArmorPrimary_Detail Diffuse Blend;37;0;Create;True;0;0;False;0;0;-1.83;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;356;183.0762,-7762.036;Float;False;Property;_SuitPrimary_WearRemap;SuitPrimary_Wear Remap;93;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;219;-1565.81,-7229.646;Float;False;Property;_ArmorSecondary_Metalness;ArmorSecondary_Metalness;47;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;367;-1143.212,-5861.08;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;181;-2060.635,-6319.792;Float;False;Property;_WornArmorPrimary_DetailRoughnessBlend;WornArmorPrimary_Detail Roughness Blend;39;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;217;-1564.246,-6253.522;Float;False;Property;_WornArmorSecondary_Metalness;WornArmorSecondary_Metalness;57;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;209;-1567.81,-7300.646;Float;False;Property;_ArmorSecondary_DetailRoughnessBlend;ArmorSecondary_Detail Roughness Blend;46;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2149.946,-2097.611;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;224;-952.8071,-7471.797;Float;False;473.6343;315.1763;ClothPrimary_Material Params;5;248;238;237;236;395;;0,1,0,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;458;192.0043,728.1115;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;460;329.8316,730.9316;Float;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;15;-2400.495,-1903.116;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.998;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;65;-2509.352,-3391.654;Float;False;248;252;Dye Mask Threshold;1;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;237;-947.508,-7365.622;Float;False;Property;_ClothPrimary_DetailNormalBlend;ClothPrimary_Detail Normal Blend;62;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;236;-947.8071,-7434.797;Float;False;Property;_ClothPrimary_DetailDiffuseBlend;ClothPrimary_Detail Diffuse Blend;61;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;392;-1227.087,-7420.141;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;245;-943.944,-6388.498;Float;False;Property;_WornClothPrimary_DetailNormalBlend;WornClothPrimary_Detail Normal Blend;72;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;844;-3012.29,882.884;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;843;-3017.62,1004.989;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;846;-3332.401,1180.448;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;394;-1228.087,-6446.141;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector4Node;359;680.5737,-7764.377;Float;False;Property;_SuitSecondary_WearRemap;SuitSecondary_Wear Remap;110;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;253;-453.4433,-7475.018;Float;False;473.6343;315.1763;ClothSecondary_Material Params;5;277;267;266;265;400;;0,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;366;-1146.617,-5755.391;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;391;-1727.39,-6448.506;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;255;-449.8793,-6496.895;Float;False;470.6343;315.1763;ClothSecondary_Worn Material Parameters;5;275;274;272;271;398;;0,0.5019608,0.5019608,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;389;-1731.048,-7425.875;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;243;-944.2431,-6455.674;Float;False;Property;_WornClothPrimary_DetailDiffuseBlend;WornClothPrimary_Detail Diffuse Blend;71;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;242;-941.944,-6321.498;Float;False;Property;_WornClothPrimary_DetailRoughnessBlend;WornClothPrimary_Detail Roughness Blend;73;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;246;-943.944,-6252.498;Float;False;Property;_WornClothPrimary_Metalness;WornClothPrimary_Metalness;74;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2047.962,-951.002;Float;True;Property;_GstackTexture;Gstack Texture;1;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;459;190.8972,831.1284;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-945.508,-7228.622;Float;False;Property;_ClothPrimary_Metalness;ClothPrimary_Metalness;64;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;238;-947.508,-7299.622;Float;False;Property;_ClothPrimary_DetailRoughnessBlend;ClothPrimary_Detail Roughness Blend;63;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;266;-448.1442,-7369.842;Float;False;Property;_ClothSecondary_DetailNormalBlend;ClothSecondary_Detail Normal Blend;79;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;397;-606.9744,-6439.354;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;274;-444.5803,-6391.718;Float;False;Property;_WornClothSecondary_DetailNormalBlend;WornClothSecondary_Detail Normal Blend;89;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;395;-613.0297,-7406.702;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector4Node;849;-3636.411,1416.605;Float;False;Property;_Suit_DetailNormalTransform;Suit_Detail Normal Transform;23;0;Create;True;0;0;False;0;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;847;-3022.31,1253.517;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;848;-3016.98,1131.412;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;118;-2426.403,1007.517;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;408;677.955,-6108.907;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;284;168.4297,-6509.019;Float;False;470.6343;315.1763;SuitPrimary_Worn Material Parameters;5;304;303;301;300;403;;0,0,0.5019608,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;462;454.8311,737.5364;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;277;-446.1442,-7231.842;Float;False;Property;_ClothSecondary_Metalness;ClothSecondary_Metalness;81;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;271;-443.5803,-6321.718;Float;False;Property;_WornClothSecondary_DetailRoughnessBlend;WornClothSecondary_Detail Roughness Blend;90;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;282;164.8658,-7484.719;Float;False;473.6343;315.1763;SuitPrimary_Material Params;5;306;296;295;294;401;;0,0,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;267;-448.1442,-7301.842;Float;False;Property;_ClothSecondary_DetailRoughnessBlend;ClothSecondary_Detail Roughness Blend;80;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;265;-447.4433,-7439.018;Float;False;Property;_ClothSecondary_DetailDiffuseBlend;ClothSecondary_Detail Diffuse Blend;78;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;272;-445.8793,-6459.895;Float;False;Property;_WornClothSecondary_DetailDiffuseBlend;WornClothSecondary_Detail Diffuse Blend;88;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;369;-778.6963,-6075.248;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;461;340.8316,831.5334;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;47;-2459.351,-3341.655;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;0.157;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;194;-1575.51,-7153.618;Float;False;473.6343;315.1763;ArmorSecondary_Material Advanced Params;4;206;205;203;393;;1,1,0,1;0;0
Node;AmplifyShaderEditor.LerpOp;364;-1147.371,-5643.395;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;159;-2072.899,-7151.888;Float;False;473.6343;315.1763;ArmorPrimary_Material Advanced Params;4;163;162;161;390;;1,0,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;275;-444.5803,-6256.718;Float;False;Property;_WornClothSecondary_Metalness;WornClothSecondary_Metalness;91;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;303;173.7287,-6403.843;Float;False;Property;_WornSuitPrimary_DetailNormalBlend;WornSuitPrimary_Detail Normal Blend;106;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;363;-1150.337,-5527.845;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;161;-2067.899,-7115.888;Float;False;Property;_ArmorPrimary_Iridescence;ArmorPrimary_Iridescence;31;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;294;169.8658,-7447.719;Float;False;Property;_SuitPrimary_DetailDiffuseBlend;SuitPrimary_Detail Diffuse Blend;95;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;535;-1982.226,876.0747;Float;True;Property;_DetailNormal01;Detail Normal 01;7;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;True;1;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;-1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;534;-1984.545,1059.098;Float;True;Property;_DetailNormal02;Detail Normal 02;9;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;True;1;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;-1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;850;-3332.402,1443.043;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;119;-2417.156,1379.043;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;306;172.1648,-7241.544;Float;False;Property;_SuitPrimary_Metalness;SuitPrimary_Metalness;98;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;296;170.1648,-7312.544;Float;False;Property;_SuitPrimary_DetailRoughnessBlend;SuitPrimary_Detail Roughness Blend;97;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;398;-122.9744,-6429.354;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;223;-955.2081,-7152.594;Float;False;473.6343;315.1763;ClothPrimary_Material Advanced Params;4;235;234;232;396;;0,1,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;304;173.7287,-6267.843;Float;False;Property;_WornSuitPrimary_Metalness;WornSuitPrimary_Metalness;108;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;400;-114.3669,-7394.649;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;300;174.7287,-6335.843;Float;False;Property;_WornSuitPrimary_DetailRoughnessBlend;WornSuitPrimary_Detail Roughness Blend;107;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;-2065.601,-6979.712;Float;False;Property;_ArmorPrimary_Transmission;ArmorPrimary_Transmission;33;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;203;-1574.239,-7117.618;Float;False;Property;_ArmorSecondary_Iridescence;ArmorSecondary_Iridescence;48;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;301;172.4297,-6472.019;Float;False;Property;_WornSuitPrimary_DetailDiffuseBlend;WornSuitPrimary_Detail Diffuse Blend;105;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;295;170.1648,-7379.544;Float;False;Property;_SuitPrimary_DetailNormalBlend;SuitPrimary_Detail Normal Blend;96;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;313;667.7933,-6509.817;Float;False;470.6343;315.1763;SuitSecondary_Worn Material Parameters;5;333;332;330;329;406;;0.5019608,0.5019608,0.5019608,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;311;664.2294,-7487.94;Float;False;473.6343;315.1763;SuitSecondary_Material Params;5;335;325;324;323;404;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;206;-1568.212,-6981.442;Float;False;Property;_ArmorSecondary_Transmission;ArmorSecondary_Transmission;50;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;463;600.2596,817.3046;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;368;-777.4832,-5967.851;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-2067.601,-7046.712;Float;False;Property;_ArmorPrimary_Fuzz;ArmorPrimary_Fuzz;32;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;407;676.762,-6001.51;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-1570.212,-7048.442;Float;False;Property;_ArmorSecondary_Fuzz;ArmorSecondary_Fuzz;49;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;403;497.4108,-6441.022;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;553;-1551.099,1344.398;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;325;669.5284,-7315.764;Float;False;Property;_SuitSecondary_DetailRoughnessBlend;SuitSecondary_Detail Roughness Blend;114;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;536;-1983.545,1247.098;Float;True;Property;_DetailNormal03;Detail Normal 03;11;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;True;1;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;851;-3022.311,1516.113;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;852;-3016.981,1394.008;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;232;-950.2081,-7116.594;Float;False;Property;_ClothPrimary_Iridescence;ClothPrimary_Iridescence;65;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;235;-947.9101,-6980.417;Float;False;Property;_ClothPrimary_Transmission;ClothPrimary_Transmission;67;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;374;-776.5828,-5856.31;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;390;-1731.375,-7119.355;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;466;-993.3474,-5308.305;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;785;-1132.119,-3826.848;Float;False;3451.031;2201.251;Comment;12;64;62;25;596;614;605;624;625;626;627;628;629;Color and Detail Maps;1,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;252;-455.8443,-7155.814;Float;False;473.6343;315.1763;ClothSecondary_Material Advanced Params;4;264;263;261;399;;0,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;335;671.5284,-7244.764;Float;False;Property;_SuitSecondary_Metalness;SuitSecondary_Metalness;115;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;413;677.6619,-5893.93;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;324;669.5284,-7382.764;Float;False;Property;_SuitSecondary_DetailNormalBlend;SuitSecondary_Detail Normal Blend;113;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;329;674.0925,-6336.64;Float;False;Property;_WornSuitSecondary_DetailRoughnessBlend;WornSuitSecondary_Detail Roughness Blend;124;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;330;671.7933,-6472.817;Float;False;Property;_WornSuitSecondary_DetailDiffuseBlend;WornSuitSecondary_Detail Diffuse Blend;122;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;333;673.0925,-6268.64;Float;False;Property;_WornSuitSecondary_Metalness;WornSuitSecondary_Metalness;125;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;401;504.4108,-7407.022;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;323;669.2294,-7450.94;Float;False;Property;_SuitSecondary_DetailDiffuseBlend;SuitSecondary_Detail Diffuse Blend;112;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;332;673.0925,-6404.64;Float;False;Property;_WornSuitSecondary_DetailNormalBlend;WornSuitSecondary_Detail Normal Blend;123;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;234;-949.9101,-7046.299;Float;False;Property;_ClothPrimary_Fuzz;ClothPrimary_Fuzz;66;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;464;749.9568,781.1706;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;393;-1233.087,-7100.87;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;537;-1977.545,1435.098;Float;True;Property;_DetailNormal04;Detail Normal 04;13;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;True;1;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;554;-1547.505,1454.763;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;120;-2398.721,1758.464;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;281;160.0416,-7165.516;Float;False;473.6343;315.1763;SuitPrimary_Material Advanced Params;4;293;292;290;402;;0,0,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;261;-450.8443,-7119.814;Float;False;Property;_ClothSecondary_Iridescence;ClothSecondary_Iridescence;82;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;373;-779.988,-5754.578;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;404;1006.225,-7390.722;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;64;-1078.586,-2693.49;Float;False;754.4247;1062.227;ColorB;11;568;569;570;571;572;573;581;582;583;584;585;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;263;-449.5463,-7052.638;Float;False;Property;_ClothSecondary_Fuzz;ClothSecondary_Fuzz;83;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;406;1002.225,-6447.722;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;264;-449.5463,-6982.638;Float;False;Property;_ClothSecondary_Transmission;ClothSecondary_Transmission;84;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;465;1011.833,773.4728;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;396;-616.9744,-7108.354;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;412;674.257,-5788.24;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;62;-1082.119,-3769.839;Float;False;754.4244;1062.227;ColorA;11;562;563;564;565;566;567;574;575;576;577;578;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;376;-248.7282,-6121.985;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;293;167.3396,-6993.339;Float;False;Property;_SuitPrimary_Transmission;SuitPrimary_Transmission;101;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;828;-3667.542,119.8758;Float;False;Property;_Armor_DetailDiffuseTransform;Armor_Detail Diffuse Transform;18;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;371;-780.7422,-5642.578;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;467;1202.573,765.1241;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;375;-243.28,-6004.26;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;568;-1056.161,-2483.448;Float;False;Property;_WornArmorSecondary_Color;WornArmorSecondary_Color;52;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;410;673.5031,-5676.24;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;570;-1058.333,-2646.224;Float;False;Property;_WornArmorPrimary_Color;WornArmorPrimary_Color;35;0;Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;562;-1069.781,-3729.536;Float;False;Property;_ArmorPrimary_Color;ArmorPrimary_Color;24;0;Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;310;661.8284,-7168.736;Float;False;473.6343;315.1763;SuitSecondary_Material Advanced Params;4;322;321;319;405;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;468;1016.891,868.1508;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;555;-1542.117,1562.522;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;538;-1975.779,1621.68;Float;True;Property;_DetailNormal05;Detail Normal 05;15;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;True;1;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;290;165.0416,-7129.516;Float;False;Property;_SuitPrimary_Iridescence;SuitPrimary_Iridescence;99;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;563;-1067.609,-3566.759;Float;False;Property;_ArmorSecondary_Color;ArmorSecondary_Color;41;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;292;165.3396,-7060.339;Float;False;Property;_SuitPrimary_Fuzz;SuitPrimary_Fuzz;100;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;399;-112.094,-7094.875;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;552;-1972.141,1804.988;Float;True;Property;_DetailNormal06;Detail Normal 06;17;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;True;1;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;556;-1540.769,1672.974;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;321;667.1263,-7063.56;Float;False;Property;_SuitSecondary_Fuzz;SuitSecondary_Fuzz;117;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;322;669.1263,-6996.56;Float;False;Property;_SuitSecondary_Transmission;SuitSecondary_Transmission;118;0;Create;True;0;0;False;0;0;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;581;-573.3909,-2663.198;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;833;-3652.033,391.0466;Float;False;Property;_Cloth_DetailDiffuseTransform;Cloth_Detail Diffuse Transform;20;0;Create;True;0;0;False;0;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;574;-589.8889,-3698.805;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;402;499.4108,-7117.022;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;381;-242.3795,-5896.678;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;569;-1056.161,-2323.448;Float;False;Property;_WornClothPrimary_Color;WornClothPrimary_Color;69;0;Create;True;0;0;False;0;0,1,0,0;0,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;702;613.6332,-5395.331;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;319;667.1284,-7131.736;Float;False;Property;_SuitSecondary_Iridescence;SuitSecondary_Iridescence;116;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;701;-613.2848,-5324.029;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;564;-1068.609,-3405.759;Float;False;Property;_ClothPrimary_Color;ClothPrimary_Color;58;0;Create;True;0;0;False;0;0,1,0,0;0,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;469;1370.62,799.8924;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;830;-3321.534,168.8357;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LerpOp;557;-1539.422,1786.121;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;415;-431.8781,-5349.256;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;596;-15.64122,-3436.699;Float;False;611.2107;297.104;Bungie Overlay;8;604;603;602;601;600;599;598;597;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;582;-574.8838,-2557.1;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;834;-3323.025,425.4848;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;831;-3006.113,119.8001;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;575;-591.3818,-3592.707;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;571;-1056.161,-2161.448;Float;False;Property;_WornClothSecondary_Color;WornClothSecondary_Color;86;0;Create;True;0;0;False;0;0,1,1,0;0,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;405;1003.225,-7121.722;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;470;1543.13,822.0585;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;25;-5.844649,-3769.805;Float;False;611.2107;297.104;Bungie Overlay;8;588;589;590;591;592;593;594;595;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;565;-1067.609,-3243.759;Float;False;Property;_ClothSecondary_Color;ClothSecondary_Color;75;0;Create;True;0;0;False;0;0,1,1,0;0,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;454;855.6198,-5358.718;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LerpOp;380;-245.7848,-5790.988;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;832;-3011.442,241.9052;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;576;-590.4819,-3485.127;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;837;-3648.375,635.3638;Float;False;Property;_Suit_DetailDiffuseTransform;Suit_Detail Diffuse Transform;22;0;Create;True;0;0;False;0;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;591;6.664856,-3716.553;Float;False;Constant;_Float5;Float 5;100;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;836;-3007.603,376.4493;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;654;-1347.244,1379.188;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;115;-2455.739,-208.5767;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;561;-2054.232,-1142.365;Float;True;Property;_DiffuseTexture;Diffuse Texture;0;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;545;-2045.937,-763.5131;Float;True;Property;_NormalMap;Normal Map;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;-1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;597;-3.131699,-3383.447;Float;False;Constant;_Float7;Float 7;100;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;523;-69.79223,-1528.892;Float;False;1588.4;348.3379;Comment;3;514;515;49;Metalness;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;566;-1066.962,-3080.908;Float;False;Property;_SuitPrimary_Color;SuitPrimary_Color;92;0;Create;True;0;0;False;0;0,0,1,0;0,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;583;-573.9839,-2449.52;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;572;-1055.514,-1997.596;Float;False;Property;_WornSuitPrimary_Color;WornSuitPrimary_Color;103;0;Create;True;0;0;False;0;0,0,1,0;0,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;378;-246.5388,-5678.987;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;788;-788.5777,936.4153;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;835;-3012.933,498.5544;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;567;-1063.962,-2915.908;Float;False;Property;_SuitSecondary_Color;SuitSecondary_Color;109;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;49;-45.40383,-1491.697;Float;False;943.1265;287;Metalness Nondyeable;9;38;39;42;43;40;41;44;45;46;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;598;147.554,-3398.392;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;93;-2003.435,-303.4697;Float;True;Property;_DetailDiffuse01;DetailDiffuse01;6;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;1;False;gray;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;700;-1001.34,1633.473;Float;False;Constant;_Color1;Color 1;101;0;Create;True;0;0;False;0;0,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;577;-593.8868,-3379.434;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;838;-3325.367,669.3237;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;704;-503.847,933.8449;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;694;-1365.435,1131.838;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;593;7.468079,-3579.09;Float;False;Constant;_Float6;Float 6;100;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;484;-240.7512,-4711.222;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ColorNode;573;-1049.226,-1832.596;Float;False;Property;_WornSuitSecondary_Color;WornSuitSecondary_Color;120;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;588;157.3505,-3731.498;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;599;-2.328476,-3245.984;Float;False;Constant;_Float8;Float 8;100;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;692;-1006.376,1349.927;Float;True;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;584;-577.3888,-2343.827;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;94;-2002.535,-114.9932;Float;True;Property;_DetailDiffuse02;DetailDiffuse02;8;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;-2443.399,193.7327;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;695;-1005.066,1093.968;Float;True;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;840;-3009.945,620.288;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;765;322.3236,1151.459;Float;False;2095.987;689.4496;Iridescence;19;633;637;634;635;639;636;631;672;671;646;670;815;820;821;822;823;824;825;826;Iridescence;0.8206897,0,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;95;-2005.535,71.00674;Float;True;Property;_DetailDiffuse03;DetailDiffuse03;10;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;152;-2072.063,-8115.775;Float;False;473.6343;315.1763;ArmorPrimary_Roughness Remap;1;344;;1,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;192;-1571.406,-6815.358;Float;False;473.6343;315.1763;ArmorSecondary_Worn Roughness Remap;1;348;;0.5,0.5019608,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;766;-101.2055,-1121.004;Float;False;1770.951;742.2281;Smoothness;13;442;418;452;451;477;473;478;474;475;479;480;476;481;Smoothness;0,1,0.04827571,1;0;0
Node;AmplifyShaderEditor.LerpOp;684;-812.2627,-217.1122;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;196;-1574.674,-8117.509;Float;False;473.6343;315.1763;ArmorSecondary_Roughness Remap;1;345;;1,1,0,1;0;0
Node;AmplifyShaderEditor.LerpOp;699;-407.5945,1267.662;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-23.40285,-1329.702;Float;False;Constant;_Float4;Float 4;9;0;Create;True;0;0;False;0;0.12549;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;839;-3015.275,742.3932;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;585;-578.1428,-2231.828;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;171;-2068.795,-6813.628;Float;False;473.6343;315.1763;ArmorPrimary_Worn Roughness Remap;1;343;;0.5,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;600;160.8683,-3265.447;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-29.40285,-1414.698;Float;False;Constant;_Float3;Float 3;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;592;170.6649,-3598.553;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;601;282.8685,-3388.447;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;590;292.6649,-3721.553;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FloorOpNode;499;230.1236,1515.188;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;435;-809.6841,-967.3165;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;578;-594.6408,-3267.435;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;633;399.9365,1495.126;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;343;-2058.869,-6772.645;Float;False;Property;_WornArmorPrimary_RoughnessRemap;WornArmorPrimary_Roughness Remap;36;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;418;-51.20551,-1071.004;Float;False;611.2107;297.104;Bungie Overlay;8;426;425;424;423;422;421;420;419;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;96;-2000.535,258.0068;Float;True;Property;_DetailDiffuse04;DetailDiffuse04;12;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;221;-951.1041,-6814.333;Float;False;473.6343;315.1763;ClothPrimary_Worn Roughness Remap;1;351;;0,0.5019608,0,1;0;0
Node;AmplifyShaderEditor.LerpOp;436;-809.8771,-859.918;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;348;-1545.501,-6774.367;Float;False;Property;_WornArmorSecondary_RoughnessRemap;WornArmorSecondary_Roughness Remap;53;0;Create;True;0;0;False;0;0,1,0,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;685;-812.4557,-109.7139;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;605;662.8025,-3769.056;Float;False;611.2107;297.104;Bungie Overlay;8;613;612;610;609;608;607;606;611;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;344;-2060.091,-8075.936;Float;False;Property;_ArmorPrimary_RoughnessRemap;ArmorPrimary_Roughness Remap;26;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;225;-954.372,-8116.484;Float;False;473.6343;315.1763;ClothPrimary_Roughness Remap;1;349;;0,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;442;-48.54124,-767.6331;Float;False;611.2107;297.104;Bungie Overlay;8;450;449;448;447;446;445;444;443;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;117;-2438.088,566.28;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;614;658.1312,-3444.667;Float;False;611.2107;297.104;Bungie Overlay;8;622;621;620;619;618;617;616;615;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;603;295.6717,-3262.984;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;38;164.7691,-1452.124;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;542;-221.3246,1115.371;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;594;305.4681,-3596.09;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;589;466.3505,-3707.498;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;345;-1564.46,-8078.35;Float;False;Property;_ArmorSecondary_RoughnessRemap;ArmorSecondary_Roughness Remap;43;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;602;456.5543,-3374.392;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;349;-932.4095,-8068.676;Float;False;Property;_ClothPrimary_RoughnessRemap;ClothPrimary_Roughness Remap;60;0;Create;True;0;0;False;0;0,1,0,1;1.88,7.31,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;615;670.6407,-3391.415;Float;False;Constant;_Float11;Float 11;100;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;437;-808.9772,-752.3387;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;351;-928.7034,-6767.041;Float;False;Property;_WornClothPrimary_RoughnessRemap;WornClothPrimary_Roughness Remap;70;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;250;-451.7403,-6817.554;Float;False;473.6343;315.1763;ClothSecondary_Worn Roughness Remap;1;354;;0,0.5019608,0.5019608,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;784;154.4618,-4399.884;Float;False;1112.334;396.9746;Comment;8;500;501;502;498;503;497;504;505;Odd/Even Index;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;422;-27.98958,-979.7288;Float;False;Constant;_Float23;Float 23;3;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;634;524.6212,1493.336;Float;False;2;0;FLOAT;128;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;41;302.5974,-1449.304;Float;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;595;480,-3584;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;97;-1996.035,440.3215;Float;True;Property;_DetailDiffuse05;DetailDiffuse05;14;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;606;675.312,-3715.804;Float;False;Constant;_Float9;Float 9;100;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;686;-811.5558,-2.134555;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;254;-455.0082,-8119.707;Float;False;473.6343;315.1763;ClothSecondary_Roughness Remap;1;352;;0,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;604;470.6719,-3253.984;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;185;-1595.488,-6044.038;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FresnelNode;637;372.3236,1214.445;Float;True;Standard;TangentNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;446;-25.32531,-676.3579;Float;False;Constant;_Float29;Float 29;3;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;163.662,-1349.107;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;383;62.67971,-6083.566;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;607;825.9976,-3730.749;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;382;62.48684,-5976.169;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;40;313.5974,-1348.702;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;427.597,-1442.698;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;608;676.1152,-3578.341;Float;False;Constant;_Float10;Float 10;100;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;635;667.3889,1494.457;Float;False;2;0;FLOAT;0;False;1;FLOAT;128;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;560;-1996.307,623.7943;Float;True;Property;_DetailDiffuse06;DetailDiffuse06;16;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;617;671.4439,-3253.952;Float;False;Constant;_Float12;Float 12;100;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;419;120.8146,-1007.04;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;283;163.3009,-8129.408;Float;False;473.6343;315.1763;SuitPrimary_Roughness Remap;1;357;;0,0,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;438;-812.3821,-646.646;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;687;-814.9607,103.5587;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;354;-433.541,-6764.707;Float;False;Property;_WornClothSecondary_RoughnessRemap;WornClothSecondary_Roughness Remap;87;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;443;123.4789,-703.6691;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;421;-43.14316,-897.3973;Float;False;Constant;_Float21;Float 21;3;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;500;418.4022,-4163.316;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;352;-435.8767,-8063.337;Float;False;Property;_ClothSecondary_RoughnessRemap;ClothSecondary_Roughness Remap;77;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;186;-1595.681,-5936.641;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;445;-40.47885,-594.0264;Float;False;Constant;_Float27;Float 27;3;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;616;821.3263,-3406.36;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;279;165.5688,-6827.255;Float;False;473.6343;315.1763;SuitPrimary_Worn Roughness Remap;1;355;;0,0,0.5019608,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;639;676.9929,1369.766;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;388;63.38675,-5868.588;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;618;834.6407,-3273.415;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;450;250.8254,-700.3231;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;187;-1594.781,-5829.06;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;688;-820.5286,215.2215;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;619;956.6405,-3396.415;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;783;-375.213,1949.212;Float;False;1928.781;1218.579;Comment;14;755;768;769;770;774;771;775;772;776;773;777;778;781;767;Emission;0,0.006896734,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;312;662.6644,-8130.208;Float;False;473.6343;315.1763;SuitSecondary_Roughness Remap;1;358;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;426;248.1611,-1003.694;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;420;120.6153,-871.729;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;501;559.206,-4165.911;Float;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;440;-813.1361,-534.6469;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;444;123.2796,-568.3581;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;610;960.3118,-3727.804;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;355;183.0762,-6771.714;Float;False;Property;_WornSuitPrimary_RoughnessRemap;WornSuitPrimary_Roughness Remap;104;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;45;573.0262,-1362.931;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;609;837.312,-3601.804;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;308;665.9324,-6830.476;Float;False;473.6343;315.1763;SuitSecondary_Worn Roughness Remap;1;360;;0.5,0.5019608,0.5019608,1;0;0
Node;AmplifyShaderEditor.Vector4Node;357;186.7476,-8082.022;Float;False;Property;_SuitPrimary_RoughnessRemap;SuitPrimary_Roughness Remap;94;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;670;910.6237,1201.459;Float;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;636;880.5161,1431.961;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;423;250.3547,-895.1956;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;621;969.4437,-3270.952;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;188;-1598.186,-5723.37;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector4Node;358;678.238,-8082.027;Float;False;Property;_SuitSecondary_RoughnessRemap;SuitSecondary_Roughness Remap;111;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;360;685.2451,-6781.062;Float;False;Property;_WornSuitSecondary_RoughnessRemap;WornSuitSecondary_Roughness Remap;121;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;135;30.19819,-246.5514;Float;False;1249.486;511.5902;Alpha Test;3;122;132;133;Alpha Test;0,0,0.6655172,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;755;283.9692,1999.212;Float;False;943.1265;287;Color ramp;9;764;763;762;761;760;759;758;757;756;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;620;1130.328,-3382.36;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;46;722.7233,-1399.065;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;631;1036.312,1354.01;Float;True;Property;_Iridescence_Lookup;_Iridescence_Lookup;5;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;502;680.4201,-4162.911;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;449;409.5778,-720.5261;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;387;59.98196,-5762.898;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;672;1212.903,1212.932;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;425;406.9135,-1023.897;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;447;255.2318,-591.8247;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;515;953.6422,-1454.732;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;611;1127.325,-3718.754;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;612;970.115,-3594.341;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;122;69.19813,-32.96211;Float;False;943.1265;287;Color ramp;9;131;130;129;128;127;126;125;124;123;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;646;401.6892,1707.909;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;424;428.7844,-925.0182;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;756;301.9702,2071.212;Float;False;Constant;_Float15;Float 15;9;0;Create;True;0;0;False;0;0.157;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;769;-324.0668,2309.169;Float;False;Property;_ArmorSecondary_Emission;ArmorSecondary_Emission;51;1;[HDR];Create;True;0;0;False;0;1,1,0,1;1,1,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;768;-325.213,2146.307;Float;False;Property;_ArmorPrimary_Emission;ArmorPrimary_Emission;34;1;[HDR];Create;True;0;0;False;0;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;757;310.9702,2170.212;Float;False;Constant;_Float16;Float 16;9;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;498;308.6025,-4341.621;Float;False;Constant;_Float14;Float 14;113;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;613;1136.496,-3595.201;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;503;795.2061,-4160.911;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;190;-1598.94,-5611.37;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;622;1144.445,-3261.952;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;514;1334.607,-1429.607;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;448;431.4487,-621.6473;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;385;59.22763,-5650.9;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;671;1416.395,1368.816;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;125;125.1991,134.0384;Float;False;Constant;_Float18;Float 18;9;0;Create;True;0;0;False;0;0.055;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;119.1991,37.03831;Float;False;Constant;_Float19;Float 19;9;0;Create;True;0;0;False;0;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;770;-320.7668,2470.791;Float;False;Property;_ClothPrimary_Emission;ClothPrimary_Emission;68;1;[HDR];Create;True;0;0;False;0;0,1,0,1;0,1,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;774;115.816,2420.91;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;758;489.1425,2055.787;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;822;1643.962,1522.753;Float;False;Constant;_Float22;Float 22;120;0;Create;True;0;0;False;0;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;452;649.9705,-698.6896;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;624;1410.141,-3610.216;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;815;1082.425,1681.995;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;504;926.2066,-4158.911;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;625;1402.925,-3492.314;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;451;650.4829,-965.7549;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;820;1597.327,1397.891;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;471;-1571.169,-5323.303;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;453;72.05421,-5469.439;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ConditionalIfNode;497;512.3594,-4349.884;Float;False;False;5;0;FLOAT;0;False;1;FLOAT;-0.0001;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;775;115.6229,2528.308;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;760;631.97,2049.213;Float;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;626;1621.396,-3586.679;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;129;274.3711,23.61296;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;477;960.6814,-601.1855;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;505;1130.49,-4245.628;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;821;1836.944,1410.492;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;473;939.5792,-843.8301;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;759;493.0349,2151.807;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;771;-320.7668,2636.045;Float;False;Property;_ClothSecondary_Emission;ClothSecondary_Emission;85;1;[HDR];Create;True;0;0;False;0;0,1,1,1;0,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;657;2756.797,-3865.634;Float;False;222.9307;1452.216;OUTPUTS;8;658;661;662;663;660;664;782;792;OUTPUTS;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;772;-319.7668,2800.548;Float;False;Property;_SuitPrimary_Emission;SuitPrimary_Emission;102;1;[HDR];Create;True;0;0;False;0;0,0,1,1;0,0,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;776;116.5228,2635.888;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;762;630.97,2153.212;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;761;750.9696,2055.213;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;474;961.071,-746.8393;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;629;1616.003,-3760.128;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;126;278.2635,119.6334;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;475;1100.871,-811.6318;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;823;2037.02,1333.155;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;627;1833.939,-3626.179;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;131;417.1986,17.03861;Float;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;479;1106.81,-587.399;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;663;2790.034,-3615.961;Float;False;152.7666;166.6935;Metalness;1;524;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;478;961.5948,-511.7759;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;628;2117.946,-3742.64;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;773;-319.7668,2960.791;Float;False;Property;_SuitSecondary_Emission;SuitSecondary_Emission;119;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;536.1982,23.0385;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;664;2786.667,-3795.249;Float;False;162.5916;160.8999;Color;1;630;;1,0,0,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;476;1248.391,-771.923;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;763;902.3979,2127.981;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;777;113.1179,2741.582;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;825;1867.916,1195.121;Float;False;3;0;FLOAT;0.02;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;480;1254.33,-547.6901;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;824;2242.311,1332.365;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;128;416.1986,121.0384;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;524;2798.108,-3576.443;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;658;2796.308,-3088.946;Float;False;127;127;Cavity Map;1;656;;0.5582311,0.4044118,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;682;3137.492,-3584.664;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;797;513.9423,-5025.618;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;778;112.3639,2853.582;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;123;687.6265,95.80798;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;132;125.2764,-196.5515;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;656;2801.847,-3053.521;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;796;447.6641,-4841.126;Float;False;Constant;_Float17;Float 17;122;0;Create;True;0;0;False;0;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;826;2192.905,1538.114;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;660;2784.198,-2951.125;Float;False;161;179;Iridescence;1;814;;0.9034481,0,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;481;1485.745,-729.5118;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;662;2788.426,-3434.943;Float;False;152;160;Smoothness;1;482;;0,1,0,1;0;0
Node;AmplifyShaderEditor.LerpOp;630;2795.327,-3754.018;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;764;1052.095,2093.544;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;781;1378.568,2283.597;Float;False;Constant;_Float2;Float 2;116;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;130;837.3233,61.37072;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;382.5595,-119.7708;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;30;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;798;202.7307,-4921.867;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;794;810.4601,-4837.306;Float;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;782;2792.039,-2735.575;Float;False;148.229;149.873;Emission;1;780;;0,0,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;792;2787.238,-2572.566;Float;False;143.1616;144.5615;Transmission;1;791;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;813;1553.059,-6342.899;Float;False;474.3419;159.5313;Using this to hide the mask clip value from the user;1;812;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;680;3266.708,-3362.891;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;661;2781.598,-3264.604;Float;False;147.8179;161.1381;Alpha Test;1;134;;0,0,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;482;2796.426,-3396.943;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;814;2808.943,-2894.98;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;681;3327.322,-3647.197;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;767;1145.792,2290.95;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;795;3510.659,-3615.735;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;780;2805.899,-2696.116;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;791;2798.801,-2531.915;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;812;1679.621,-6290.165;Float;False;Constant;_Maskclipvalue;Mask clip value;123;1;[HideInInspector];Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;705;3512.826,-3511.655;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;679;3110.42,-3202.672;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.89;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;134;2787.917,-3222.868;Float;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;370;-782.708,-5531.028;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;372;-977.8102,-5602.322;Float;False;Constant;_Float25;Float 25;28;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3777.748,-3539.293;Half;False;True;2;Half;ASEMaterialInspector;0;0;StandardSpecular;D2 PlayerGear Shader 2.1.0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;False;True;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;True;812;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;709;0;708;0
WireConnection;713;0;709;2
WireConnection;713;2;711;0
WireConnection;710;0;709;0
WireConnection;710;2;711;0
WireConnection;716;0;713;0
WireConnection;712;0;709;1
WireConnection;712;2;711;0
WireConnection;714;0;710;0
WireConnection;717;0;710;0
WireConnection;717;1;712;0
WireConnection;721;0;712;0
WireConnection;721;1;716;0
WireConnection;719;0;710;0
WireConnection;719;1;712;0
WireConnection;726;0;710;0
WireConnection;726;1;712;0
WireConnection;718;0;717;0
WireConnection;720;0;719;0
WireConnection;724;0;721;0
WireConnection;724;1;714;0
WireConnection;736;0;720;0
WireConnection;735;0;718;0
WireConnection;725;0;724;0
WireConnection;727;0;726;0
WireConnection;727;1;716;0
WireConnection;737;0;725;0
WireConnection;728;0;727;0
WireConnection;732;0;710;0
WireConnection;732;1;712;0
WireConnection;729;0;714;0
WireConnection;729;1;713;0
WireConnection;741;0;735;0
WireConnection;741;1;736;0
WireConnection;738;0;728;0
WireConnection;743;0;741;0
WireConnection;743;1;737;0
WireConnection;733;0;732;0
WireConnection;733;1;713;0
WireConnection;731;0;729;0
WireConnection;734;0;733;0
WireConnection;744;0;743;0
WireConnection;744;1;738;0
WireConnection;739;0;731;0
WireConnection;745;0;744;0
WireConnection;745;1;739;0
WireConnection;740;0;734;0
WireConnection;746;0;745;0
WireConnection;746;1;740;0
WireConnection;747;0;746;0
WireConnection;750;0;747;0
WireConnection;751;0;750;0
WireConnection;748;0;747;0
WireConnection;749;0;748;0
WireConnection;749;1;751;0
WireConnection;752;0;4;0
WireConnection;752;1;749;0
WireConnection;706;0;752;0
WireConnection;5;0;706;0
WireConnection;5;2;6;0
WireConnection;11;0;706;1
WireConnection;11;2;6;0
WireConnection;9;0;706;0
WireConnection;9;2;6;0
WireConnection;10;0;706;1
WireConnection;10;4;6;0
WireConnection;362;0;342;0
WireConnection;362;1;346;0
WireConnection;362;2;5;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;14;0;706;1
WireConnection;14;2;6;0
WireConnection;13;0;706;1
WireConnection;13;4;6;0
WireConnection;361;0;362;0
WireConnection;361;1;350;0
WireConnection;361;2;9;0
WireConnection;842;0;841;0
WireConnection;367;0;361;0
WireConnection;367;1;353;0
WireConnection;367;2;12;0
WireConnection;16;0;13;0
WireConnection;16;1;14;0
WireConnection;458;0;456;0
WireConnection;458;1;457;0
WireConnection;460;1;458;0
WireConnection;15;0;706;1
WireConnection;15;2;6;0
WireConnection;392;0;207;0
WireConnection;392;1;208;0
WireConnection;392;2;209;0
WireConnection;392;3;219;0
WireConnection;844;0;842;0
WireConnection;844;1;842;1
WireConnection;843;0;842;2
WireConnection;843;1;842;3
WireConnection;846;0;845;0
WireConnection;394;0;214;0
WireConnection;394;1;216;0
WireConnection;394;2;213;0
WireConnection;394;3;217;0
WireConnection;366;0;367;0
WireConnection;366;1;356;0
WireConnection;366;2;16;0
WireConnection;391;0;179;0
WireConnection;391;1;180;0
WireConnection;391;2;181;0
WireConnection;391;3;182;0
WireConnection;389;0;154;0
WireConnection;389;1;155;0
WireConnection;389;2;156;0
WireConnection;389;3;157;0
WireConnection;459;0;457;0
WireConnection;459;1;456;0
WireConnection;397;0;243;0
WireConnection;397;1;245;0
WireConnection;397;2;242;0
WireConnection;397;3;246;0
WireConnection;395;0;236;0
WireConnection;395;1;237;0
WireConnection;395;2;238;0
WireConnection;395;3;248;0
WireConnection;847;0;846;2
WireConnection;847;1;846;3
WireConnection;848;0;846;0
WireConnection;848;1;846;1
WireConnection;118;0;844;0
WireConnection;118;1;843;0
WireConnection;408;0;391;0
WireConnection;408;1;394;0
WireConnection;408;2;5;0
WireConnection;462;0;460;0
WireConnection;462;1;2;4
WireConnection;369;0;389;0
WireConnection;369;1;392;0
WireConnection;369;2;5;0
WireConnection;461;0;457;0
WireConnection;461;1;459;0
WireConnection;47;0;2;4
WireConnection;47;2;6;0
WireConnection;364;0;366;0
WireConnection;364;1;359;0
WireConnection;364;2;15;0
WireConnection;363;1;364;0
WireConnection;363;2;47;0
WireConnection;535;1;118;0
WireConnection;534;1;118;0
WireConnection;850;0;849;0
WireConnection;119;0;848;0
WireConnection;119;1;847;0
WireConnection;398;0;272;0
WireConnection;398;1;274;0
WireConnection;398;2;271;0
WireConnection;398;3;275;0
WireConnection;400;0;265;0
WireConnection;400;1;266;0
WireConnection;400;2;267;0
WireConnection;400;3;277;0
WireConnection;463;0;462;0
WireConnection;463;1;461;0
WireConnection;368;0;369;0
WireConnection;368;1;395;0
WireConnection;368;2;9;0
WireConnection;407;0;408;0
WireConnection;407;1;397;0
WireConnection;407;2;9;0
WireConnection;403;0;301;0
WireConnection;403;1;303;0
WireConnection;403;2;300;0
WireConnection;403;3;304;0
WireConnection;553;0;535;0
WireConnection;553;1;534;0
WireConnection;553;2;5;0
WireConnection;536;1;119;0
WireConnection;851;0;850;2
WireConnection;851;1;850;3
WireConnection;852;0;850;0
WireConnection;852;1;850;1
WireConnection;374;0;368;0
WireConnection;374;1;400;0
WireConnection;374;2;12;0
WireConnection;390;0;161;0
WireConnection;390;1;162;0
WireConnection;390;2;163;0
WireConnection;466;0;363;0
WireConnection;413;0;407;0
WireConnection;413;1;398;0
WireConnection;413;2;12;0
WireConnection;401;0;294;0
WireConnection;401;1;295;0
WireConnection;401;2;296;0
WireConnection;401;3;306;0
WireConnection;464;0;463;0
WireConnection;393;0;203;0
WireConnection;393;1;205;0
WireConnection;393;2;206;0
WireConnection;537;1;119;0
WireConnection;554;0;553;0
WireConnection;554;1;536;0
WireConnection;554;2;9;0
WireConnection;120;0;852;0
WireConnection;120;1;851;0
WireConnection;373;0;374;0
WireConnection;373;1;401;0
WireConnection;373;2;16;0
WireConnection;404;0;323;0
WireConnection;404;1;324;0
WireConnection;404;2;325;0
WireConnection;404;3;335;0
WireConnection;406;0;330;0
WireConnection;406;1;332;0
WireConnection;406;2;329;0
WireConnection;406;3;333;0
WireConnection;465;0;464;0
WireConnection;465;1;466;1
WireConnection;396;0;232;0
WireConnection;396;1;234;0
WireConnection;396;2;235;0
WireConnection;412;0;413;0
WireConnection;412;1;403;0
WireConnection;412;2;16;0
WireConnection;376;0;390;0
WireConnection;376;1;393;0
WireConnection;376;2;5;0
WireConnection;371;0;373;0
WireConnection;371;1;404;0
WireConnection;371;2;15;0
WireConnection;467;0;465;0
WireConnection;467;1;466;0
WireConnection;375;0;376;0
WireConnection;375;1;396;0
WireConnection;375;2;9;0
WireConnection;410;0;412;0
WireConnection;410;1;406;0
WireConnection;410;2;15;0
WireConnection;468;0;466;2
WireConnection;468;1;466;3
WireConnection;555;0;554;0
WireConnection;555;1;537;0
WireConnection;555;2;12;0
WireConnection;538;1;120;0
WireConnection;399;0;261;0
WireConnection;399;1;263;0
WireConnection;399;2;264;0
WireConnection;552;1;120;0
WireConnection;556;0;555;0
WireConnection;556;1;538;0
WireConnection;556;2;16;0
WireConnection;581;0;570;0
WireConnection;581;1;568;0
WireConnection;581;2;5;0
WireConnection;574;0;562;0
WireConnection;574;1;563;0
WireConnection;574;2;5;0
WireConnection;402;0;290;0
WireConnection;402;1;292;0
WireConnection;402;2;293;0
WireConnection;381;0;375;0
WireConnection;381;1;399;0
WireConnection;381;2;12;0
WireConnection;702;0;410;0
WireConnection;701;0;371;0
WireConnection;469;0;467;0
WireConnection;469;1;466;2
WireConnection;469;2;468;0
WireConnection;830;0;828;0
WireConnection;557;0;556;0
WireConnection;557;1;552;0
WireConnection;557;2;15;0
WireConnection;415;0;701;0
WireConnection;582;0;581;0
WireConnection;582;1;569;0
WireConnection;582;2;9;0
WireConnection;834;0;833;0
WireConnection;831;0;830;0
WireConnection;831;1;830;1
WireConnection;575;0;574;0
WireConnection;575;1;564;0
WireConnection;575;2;9;0
WireConnection;405;0;319;0
WireConnection;405;1;321;0
WireConnection;405;2;322;0
WireConnection;470;0;469;0
WireConnection;454;0;702;0
WireConnection;380;0;381;0
WireConnection;380;1;402;0
WireConnection;380;2;16;0
WireConnection;832;0;830;2
WireConnection;832;1;830;3
WireConnection;576;0;575;0
WireConnection;576;1;565;0
WireConnection;576;2;12;0
WireConnection;836;0;834;0
WireConnection;836;1;834;1
WireConnection;654;0;557;0
WireConnection;115;0;831;0
WireConnection;115;1;832;0
WireConnection;583;0;582;0
WireConnection;583;1;571;0
WireConnection;583;2;12;0
WireConnection;378;0;380;0
WireConnection;378;1;405;0
WireConnection;378;2;15;0
WireConnection;788;0;415;1
WireConnection;788;1;454;1
WireConnection;788;2;470;0
WireConnection;835;0;834;2
WireConnection;835;1;834;3
WireConnection;598;0;561;0
WireConnection;598;1;597;0
WireConnection;93;1;115;0
WireConnection;577;0;576;0
WireConnection;577;1;566;0
WireConnection;577;2;16;0
WireConnection;838;0;837;0
WireConnection;704;0;788;0
WireConnection;704;1;47;0
WireConnection;694;0;545;0
WireConnection;484;0;378;0
WireConnection;588;0;561;0
WireConnection;588;1;591;0
WireConnection;692;0;654;0
WireConnection;692;1;654;1
WireConnection;584;0;583;0
WireConnection;584;1;572;0
WireConnection;584;2;16;0
WireConnection;94;1;115;0
WireConnection;116;0;836;0
WireConnection;116;1;835;0
WireConnection;695;0;694;0
WireConnection;695;1;694;1
WireConnection;840;0;838;0
WireConnection;840;1;838;1
WireConnection;95;1;116;0
WireConnection;684;0;93;0
WireConnection;684;1;94;0
WireConnection;684;2;5;0
WireConnection;699;0;700;0
WireConnection;699;1;692;0
WireConnection;699;2;704;0
WireConnection;839;0;838;2
WireConnection;839;1;838;3
WireConnection;585;0;584;0
WireConnection;585;1;573;0
WireConnection;585;2;15;0
WireConnection;600;0;561;0
WireConnection;600;1;599;0
WireConnection;592;0;561;0
WireConnection;592;1;593;0
WireConnection;601;0;598;0
WireConnection;590;0;588;0
WireConnection;499;0;484;0
WireConnection;435;0;93;4
WireConnection;435;1;94;4
WireConnection;435;2;5;0
WireConnection;578;0;577;0
WireConnection;578;1;567;0
WireConnection;578;2;15;0
WireConnection;633;0;499;0
WireConnection;96;1;116;0
WireConnection;436;0;435;0
WireConnection;436;1;95;4
WireConnection;436;2;9;0
WireConnection;685;0;684;0
WireConnection;685;1;95;0
WireConnection;685;2;9;0
WireConnection;117;0;840;0
WireConnection;117;1;839;0
WireConnection;603;0;600;0
WireConnection;38;0;43;0
WireConnection;38;1;42;0
WireConnection;542;0;695;0
WireConnection;542;1;699;0
WireConnection;594;0;592;0
WireConnection;589;0;578;0
WireConnection;589;1;590;0
WireConnection;602;0;585;0
WireConnection;602;1;601;0
WireConnection;437;0;436;0
WireConnection;437;1;96;4
WireConnection;437;2;12;0
WireConnection;634;1;633;0
WireConnection;41;1;38;0
WireConnection;595;0;589;0
WireConnection;595;1;594;0
WireConnection;97;1;117;0
WireConnection;686;0;685;0
WireConnection;686;1;96;0
WireConnection;686;2;12;0
WireConnection;604;0;602;0
WireConnection;604;1;603;0
WireConnection;185;0;344;0
WireConnection;185;1;345;0
WireConnection;185;2;5;0
WireConnection;637;0;542;0
WireConnection;39;0;42;0
WireConnection;39;1;43;0
WireConnection;383;0;343;0
WireConnection;383;1;348;0
WireConnection;383;2;5;0
WireConnection;607;0;595;0
WireConnection;607;1;606;0
WireConnection;382;0;383;0
WireConnection;382;1;351;0
WireConnection;382;2;9;0
WireConnection;40;0;42;0
WireConnection;40;1;39;0
WireConnection;44;0;41;0
WireConnection;44;1;2;4
WireConnection;635;0;634;0
WireConnection;560;1;117;0
WireConnection;419;0;2;2
WireConnection;419;1;422;0
WireConnection;438;0;437;0
WireConnection;438;1;97;4
WireConnection;438;2;16;0
WireConnection;687;0;686;0
WireConnection;687;1;97;0
WireConnection;687;2;16;0
WireConnection;443;0;2;2
WireConnection;443;1;446;0
WireConnection;500;0;499;0
WireConnection;186;0;185;0
WireConnection;186;1;349;0
WireConnection;186;2;9;0
WireConnection;616;0;604;0
WireConnection;616;1;615;0
WireConnection;639;0;637;0
WireConnection;388;0;382;0
WireConnection;388;1;354;0
WireConnection;388;2;12;0
WireConnection;618;0;604;0
WireConnection;618;1;617;0
WireConnection;450;0;443;0
WireConnection;187;0;186;0
WireConnection;187;1;352;0
WireConnection;187;2;12;0
WireConnection;688;0;687;0
WireConnection;688;1;560;0
WireConnection;688;2;15;0
WireConnection;619;0;616;0
WireConnection;426;0;419;0
WireConnection;420;0;2;2
WireConnection;420;1;421;0
WireConnection;501;0;500;0
WireConnection;440;0;438;0
WireConnection;440;1;560;4
WireConnection;440;2;15;0
WireConnection;444;0;2;2
WireConnection;444;1;445;0
WireConnection;610;0;607;0
WireConnection;45;0;44;0
WireConnection;45;1;40;0
WireConnection;609;0;595;0
WireConnection;609;1;608;0
WireConnection;670;0;578;0
WireConnection;636;0;639;0
WireConnection;636;1;635;0
WireConnection;423;0;420;0
WireConnection;621;0;618;0
WireConnection;188;0;187;0
WireConnection;188;1;357;0
WireConnection;188;2;16;0
WireConnection;620;0;688;0
WireConnection;620;1;619;0
WireConnection;46;0;45;0
WireConnection;631;1;636;0
WireConnection;502;0;501;0
WireConnection;449;0;450;0
WireConnection;449;1;440;0
WireConnection;387;0;388;0
WireConnection;387;1;355;0
WireConnection;387;2;16;0
WireConnection;672;0;670;0
WireConnection;425;0;426;0
WireConnection;425;1;440;0
WireConnection;447;0;444;0
WireConnection;515;0;415;3
WireConnection;515;1;454;3
WireConnection;515;2;470;0
WireConnection;611;0;688;0
WireConnection;611;1;610;0
WireConnection;612;0;609;0
WireConnection;646;0;484;0
WireConnection;424;0;425;0
WireConnection;424;1;423;0
WireConnection;613;0;611;0
WireConnection;613;1;612;0
WireConnection;503;0;502;0
WireConnection;190;0;188;0
WireConnection;190;1;358;0
WireConnection;190;2;15;0
WireConnection;622;0;620;0
WireConnection;622;1;621;0
WireConnection;514;0;46;0
WireConnection;514;1;515;0
WireConnection;514;2;47;0
WireConnection;448;0;449;0
WireConnection;448;1;447;0
WireConnection;385;0;387;0
WireConnection;385;1;360;0
WireConnection;385;2;15;0
WireConnection;671;0;631;0
WireConnection;671;1;672;0
WireConnection;774;0;768;0
WireConnection;774;1;769;0
WireConnection;774;2;5;0
WireConnection;758;0;757;0
WireConnection;758;1;756;0
WireConnection;452;0;2;2
WireConnection;452;1;448;0
WireConnection;452;2;454;2
WireConnection;624;0;595;0
WireConnection;624;1;613;0
WireConnection;624;2;415;0
WireConnection;815;0;646;0
WireConnection;504;0;503;0
WireConnection;625;0;604;0
WireConnection;625;1;622;0
WireConnection;625;2;454;0
WireConnection;451;0;2;2
WireConnection;451;1;424;0
WireConnection;451;2;415;2
WireConnection;820;0;671;0
WireConnection;820;2;514;0
WireConnection;471;0;190;0
WireConnection;453;0;385;0
WireConnection;497;0;484;0
WireConnection;497;2;498;0
WireConnection;775;0;774;0
WireConnection;775;1;770;0
WireConnection;775;2;9;0
WireConnection;760;1;758;0
WireConnection;626;0;624;0
WireConnection;626;1;625;0
WireConnection;626;2;470;0
WireConnection;129;0;125;0
WireConnection;129;1;127;0
WireConnection;477;0;452;0
WireConnection;477;1;453;1
WireConnection;505;0;504;0
WireConnection;505;1;497;0
WireConnection;821;0;820;0
WireConnection;821;1;822;0
WireConnection;821;2;815;0
WireConnection;473;0;451;0
WireConnection;473;1;471;1
WireConnection;759;0;756;0
WireConnection;759;1;757;0
WireConnection;776;0;775;0
WireConnection;776;1;771;0
WireConnection;776;2;12;0
WireConnection;762;0;759;0
WireConnection;762;1;756;0
WireConnection;761;0;760;0
WireConnection;761;1;2;3
WireConnection;474;0;471;2
WireConnection;474;1;471;3
WireConnection;629;1;631;0
WireConnection;629;2;672;0
WireConnection;126;0;127;0
WireConnection;126;1;125;0
WireConnection;475;0;473;0
WireConnection;475;1;471;0
WireConnection;823;0;821;0
WireConnection;823;2;505;0
WireConnection;627;0;561;0
WireConnection;627;1;626;0
WireConnection;627;2;47;0
WireConnection;131;1;129;0
WireConnection;479;0;477;0
WireConnection;479;1;453;0
WireConnection;478;0;453;2
WireConnection;478;1;453;3
WireConnection;628;0;627;0
WireConnection;628;1;629;0
WireConnection;628;2;505;0
WireConnection;124;0;131;0
WireConnection;124;1;2;3
WireConnection;476;0;475;0
WireConnection;476;1;471;2
WireConnection;476;2;474;0
WireConnection;763;0;761;0
WireConnection;763;1;762;0
WireConnection;777;0;776;0
WireConnection;777;1;772;0
WireConnection;777;2;16;0
WireConnection;825;2;46;0
WireConnection;480;0;479;0
WireConnection;480;1;453;2
WireConnection;480;2;478;0
WireConnection;824;0;823;0
WireConnection;824;2;514;0
WireConnection;128;0;126;0
WireConnection;128;1;127;0
WireConnection;524;0;514;0
WireConnection;524;2;505;0
WireConnection;682;0;524;0
WireConnection;797;0;484;1
WireConnection;778;0;777;0
WireConnection;778;1;773;0
WireConnection;778;2;15;0
WireConnection;123;0;124;0
WireConnection;123;1;128;0
WireConnection;656;0;694;2
WireConnection;656;1;654;2
WireConnection;826;0;825;0
WireConnection;826;1;824;0
WireConnection;826;2;47;0
WireConnection;481;0;476;0
WireConnection;481;1;480;0
WireConnection;481;2;470;0
WireConnection;630;0;627;0
WireConnection;630;1;628;0
WireConnection;630;2;47;0
WireConnection;764;0;763;0
WireConnection;130;0;123;0
WireConnection;133;0;132;3
WireConnection;798;0;484;2
WireConnection;794;0;637;0
WireConnection;794;1;47;0
WireConnection;794;2;797;0
WireConnection;794;3;796;0
WireConnection;680;0;630;0
WireConnection;680;1;524;0
WireConnection;482;0;2;2
WireConnection;482;1;481;0
WireConnection;482;2;47;0
WireConnection;814;0;826;0
WireConnection;814;1;656;0
WireConnection;681;0;630;0
WireConnection;681;1;682;0
WireConnection;767;0;764;0
WireConnection;767;1;778;0
WireConnection;795;0;681;0
WireConnection;795;1;794;0
WireConnection;780;0;767;0
WireConnection;780;1;781;0
WireConnection;791;0;798;0
WireConnection;791;1;47;0
WireConnection;705;0;680;0
WireConnection;705;1;814;0
WireConnection;679;0;482;0
WireConnection;134;1;130;0
WireConnection;134;2;133;0
WireConnection;370;0;372;0
WireConnection;370;1;371;0
WireConnection;370;2;47;0
WireConnection;0;0;795;0
WireConnection;0;1;542;0
WireConnection;0;2;780;0
WireConnection;0;3;705;0
WireConnection;0;4;679;0
WireConnection;0;5;2;1
WireConnection;0;6;791;0
WireConnection;0;9;134;0
ASEEND*/
//CHKSM=4A9106E2F352FE5740FD464F14E1DEB2D9A4099C
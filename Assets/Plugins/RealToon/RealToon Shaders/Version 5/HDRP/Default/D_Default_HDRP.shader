//RealToon V5.0.8 (HDRP with DXR/Raytracing)
//MJQStudioWorks
//©2024

//Note:
//
//		Hi, Thank you again for using RealToon.
//		I spent alot of time making this RealToon HDRP Version, 
//		I started working on this 2019.
//
//      8 years and finally this 2024, RealToon Shader (HDRP) is out of beta and it is now fully functional.
//      Out of beta starting (Unity 6 - HDRP 17).
//
//		( MJQ Studio Works [PH] )
//

Shader "HDRP/RealToon/Version 5/Default"
{
    Properties
    {

		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", int) = 2

		[Toggle(N_F_TRANS_ON)] _TRANSMODE ("Transparent Mode", Float ) = 0.0

        _MainTex ("Texture", 2D) = "white" {}
        [ToggleUI] _TexturePatternStyle ("Texture Pattern Style", Float ) = 0.0
        [HDR] _MainColor ("Main Color", Color) = (1.0,1.0,1.0,1.0)
        _MaiColPo("Main Color Power", Float) = 0.15

		[ToggleUI] _MVCOL ("Mix Vertex Color", Float ) = 0.0

		[ToggleUI] _MCIALO ("Main Color In Ambient Light Only", Float ) = 0.0

		[HDR] _HighlightColor ("Highlight Color", Color) = (1.0,1.0,1.0,1.0)
        _HighlightColorPower ("Highlight Color Power", Float ) = 1.0

        [ToggleUI] _EnableTextureTransparent ("Enable Texture Transparent", Float ) = 0.0

		_MCapIntensity ("Intensity", Range(0, 1)) = 1.0
		_MCap ("MatCap", 2D) = "white" {}
		[ToggleUI] _SPECMODE ("Specular Mode", Float ) = 0.0
		_SPECIN ("Specular Power", Float ) = 1.0
		_MCapMask ("Mask MatCap", 2D) = "white" {}

        _Cutout ("Cutout", Range(0, 1)) = 0.0
		[ToggleUI] _AlphaBasedCutout ("Alpha Based Cutout", Float ) = 1.0
        [Toggle(N_F_SCO_ON)] _N_F_SCO ("Soft Cutout", Float ) = 0.0
        [ToggleUI] _UseSecondaryCutoutOnly ("Use Secondary Cutout Only", Float ) = 0.0
        _SecondaryCutout ("Secondary Cutout", 2D) = "white" {}

        [Toggle(N_F_COEDGL_ON)] _N_F_COEDGL("Enable Glow Edge", Float) = 0.0
        [HDR] _Glow_Color("Glow Color", Color) = (1.0,1.0,1.0,1.0)
        _Glow_Edge_Width("Glow Edge Width", Float) = 1.0

        [Toggle(N_F_SIMTRANS_ON)] _SimTrans("Simple Transparency Mode", Float) = 0.0

		_Opacity ("Opacity", Range(0, 1)) = 1.0
		_TransparentThreshold ("Transparent Threshold", Float ) = 0.0

        [Enum(UnityEngine.Rendering.BlendMode)] _BleModSour("Blend - Source", int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _BleModDest("Blend - Destination", int) = 0

        _MaskTransparency ("Mask Transparency", 2D) = "black" {}

        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalScale ("Normal Map Intensity", Float ) = 1.0

        _Saturation ("Saturation", Range(0, 2)) = 1.0

        _OutlineWidth ("Width", Float ) = 0.5
        _OutlineWidthControl ("Width Control", 2D) = "white" {}

		[Toggle(N_F_O_NM_ON)] _N_F_O_NM ("Enhance outline using normal map", Float ) = 0.0

		_ONormMap ("Normal Map", 2D) = "bump" {}
        _ONormMapInt ("Normal Map Intensity", Float ) = 1.0

		[Enum(Normal,0,Origin,1)] _OutlineExtrudeMethod("Outline Extrude Method", int) = 0

		_OutlineOffset ("Outline Offset", Vector) = (0,0,0)
		_OutlineZPostionInCamera ("Outline Z Position In Camera", Float) = 0.0

		[Enum(Off,1,On,0)] _DoubleSidedOutline("Double Sided Outline", int) = 1

        [HDR] _OutlineColor ("Color", Color) = (0.0,0.0,0.0,1.0)

		[ToggleUI] _MixMainTexToOutline ("Mix Main Texture To Outline", Float ) = 0.0

        _NoisyOutlineIntensity ("Noisy Outline Intensity", Range(0, 1)) = 0.0
        [Toggle(N_F_DNO_ON)] _DynamicNoisyOutline ("Dynamic Noisy Outline", Float ) = 0.0
        [ToggleUI] _LightAffectOutlineColor ("Light Affect Outline Color", Float ) = 0.0

        [ToggleUI] _OutlineWidthAffectedByViewDistance ("Outline Width Affected By View Distance", Float ) = 0.0
		_FarDistanceMaxWidth ("Far Distance Max Width", Float ) = 10.0

		[ToggleUI] _TOAO ("Transparent Opacity Affect Outline", Float ) = 1.0
        [ToggleUI] _VertexColorBlueAffectOutlineWitdh ("Vertex Color Blue Affect Outline Witdh", Float ) = 0.0

        _DepthThreshold("Depth Threshold", Float) = 900.0
        _NormalThreshold("Normal Threshold", Float) = 1.3
        _NormalMin("Normal Min", Float) = 1.0
        _NormalMax("Normal Max", Float) = 1.0

        [Toggle(N_F_O_MOTTSO_ON)] _N_F_MSSOLTFO("Mix Outline To The Shader Output", Float) = 0.0

        _SelfLitIntensity ("Intensity", Range(0, 1)) = 0.0
        [HDR] _SelfLitColor ("Color", Color) = (1.0,1.0,1.0,1.0)
        _SelfLitPower ("Power", Float ) = 50.0
		_TEXMCOLINT ("Texture and Main Color Intensity", Float ) = 0.0
        [ToggleUI] _SelfLitHighContrast ("High Contrast", Float ) = 1.0
        _MaskSelfLit ("Mask Self Lit", 2D) = "white" {}

        _GlossIntensity ("Gloss Intensity", Range(0, 1)) = 1.0
        _Glossiness ("Glossiness", Range(0, 1)) = 0.6
        _GlossSoftness ("Softness", Range(0, 1)) = 0.0
        [HDR] _GlossColor ("Color", Color) = (1.0,1.0,1.0,1.0)
        _GlossColorPower ("Color Power", Float ) = 1.0
        _MaskGloss ("Mask Gloss", 2D) = "white" {}

        _GlossTexture ("Gloss Texture", 2D) = "black" {}
        _GlossTextureSoftness ("Softness", Float ) = 0.0
		[ToggleUI] _PSGLOTEX ("Pattern Style", Float ) = 0.0
        _GlossTextureRotate ("Rotate", Float ) = 0.0
        [ToggleUI] _GlossTextureFollowObjectRotation ("Follow Object Rotation", Float ) = 0.0
        _GlossTextureFollowLight ("Follow Light", Range(0, 1)) = 0.0

        [HDR] _OverallShadowColor ("Overall Shadow Color", Color) = (0.0,0.0,0.0,1.0)
        _OverallShadowColorPower ("Overall Shadow Color Power", Float ) = 1.0

        [ToggleUI] _SelfShadowShadowTAtViewDirection ("Self Shadow & ShadowT At View Direction", Float ) = 0.0

		_ReduSha ("Reduce Shadow", Float ) = 0.0
		_ShadowHardness ("Shadow Hardness", Range(0, 1)) = 0.0

        _SelfShadowRealtimeShadowIntensity ("Self Shadow & Realtime Shadow Intensity", Range(0, 1)) = 1.0
        _SelfShadowThreshold ("Threshold", Range(0, 1)) = 0.930
        [ToggleUI] _VertexColorGreenControlSelfShadowThreshold ("Vertex Color Green Control Self Shadow Threshold", Float ) = 0.0
        _SelfShadowHardness ("Hardness", Range(0, 1)) = 1.0
        [HDR] _SelfShadowRealTimeShadowColor ("Self Shadow & Real Time Shadow Color", Color) = (1.0,1.0,1.0,1.0)
        _SelfShadowRealTimeShadowColorPower ("Self Shadow & Real Time Shadow Color Power", Float ) = 1.0
        [ToggleUI] _LigIgnoYNorDir("Light Ignore Y Normal Direction", Float) = 0
		[ToggleUI] _SelfShadowAffectedByLightShadowStrength ("Self Shadow Affected By Light Shadow Strength", Float ) = 0.0

        _SmoothObjectNormal ("Smooth Object Normal", Range(0, 1)) = 0.0
        [ToggleUI] _VertexColorRedControlSmoothObjectNormal ("Vertex Color Red Control Smooth Object Normal", Float ) = 0.0
        _XYZPosition ("XYZ Position", Vector) = (0.0,0.0,0.0,0.0)
        [ToggleUI] _ShowNormal ("Show Normal", Float ) = 0.0

        _ShadowColorTexture ("Shadow Color Texture", 2D) = "white" {}
        _ShadowColorTexturePower ("Power", Float ) = 0.0

        _ShadowTIntensity ("ShadowT Intensity", Range(0, 1)) = 1.0
        _ShadowT ("ShadowT", 2D) = "white" {}
        _ShadowTLightThreshold ("Light Threshold", Float ) = 50.0
        _ShadowTShadowThreshold ("Shadow Threshold", Float ) = 0.0
		_ShadowTHardness ("Hardness", Range(0, 1)) = 1.0
        [HDR] _ShadowTColor ("Color", Color) = (1.0,1.0,1.0,1.0)
        _ShadowTColorPower ("Color Power", Float ) = 1.0

		[ToggleUI] _STIL ("Ignore Light", Float ) = 0.0

		[Toggle(N_F_STIS_ON)] _N_F_STIS ("Show In Shadow", Float ) = 0.0

		[Toggle(N_F_STIAL_ON )] _N_F_STIAL ("Show In Ambient Light", Float ) = 0.0
        _ShowInAmbientLightShadowIntensity ("Show In Ambient Light & Shadow Intensity", Range(0, 1)) = 1.0
        _ShowInAmbientLightShadowThreshold ("Show In Ambient Light & Shadow Threshold", Float ) = 0.4

        [ToggleUI] _LightFalloffAffectShadowT ("Light Falloff Affect ShadowT", Float ) = 0.0

        _PTexture ("PTexture", 2D) = "white" {}
		_PTCol ("Color", Color) = (0.0,0.0,0.0,1.0)
        _PTexturePower ("Power", Float ) = 1.0

		[Toggle(N_F_RELGI_ON)] _RELG ("Receive Environmental Lighting and GI", Float ) = 1.0
        _EnvironmentalLightingIntensity ("Environmental Lighting Intensity", Float ) = 1.0

		_RTGIShaFallo ("Raytraced GI Shade Falloff", Range(0, 1) ) = 0.0

        [ToggleUI] _GIFlatShade ("GI Flat Shade", Float ) = 0.0
        _GIShadeThreshold ("GI Shade Threshold", Range(0, 1)) = 0.0

        [ToggleUI] _LightAffectShadow ("Light Affect Shadow", Float ) = 0.0
        _LightIntensity ("Light Intensity", Float ) = 1.0

		[Toggle(N_F_USETLB_ON)] _UseTLB ("Use Traditional Light Blend", Float ) = 0.0
		[Toggle(N_F_PAL_ON)] _N_F_PAL ("Enable Punctual Lights", Float ) = 1.0
		[Toggle(N_F_AL_ON)] _N_F_AL ("Enable Area Light", Float ) = 0.0

		_DirectionalLightIntensity ("Directional Light Intensity", Float ) = 0.0
		_PointSpotlightIntensity ("Point and Spot Light Intensity", Float ) = 0.0
		
		_ALIntensity ("Area Light Intensity", Float ) = 0.0
		[Toggle(N_F_ALSL_ON)] _N_F_ALSL ("Area Light Smooth Look", Float ) = 0.0
		_ALTuFo ("Tube Light Falloff (Temp Option)", Float ) = 20.0

		_LightFalloffSoftness ("Light Falloff Softness", Range(0, 1)) = 1.0

        _CustomLightDirectionIntensity ("Intensity", Range(0, 1)) = 0.0
        [ToggleUI] _CustomLightDirectionFollowObjectRotation ("Follow Object Rotation", Float ) = 0.0
        _CustomLightDirection ("Custom Light Direction", Vector) = (0.0,0.0,10.0,0.0)

        _ReflectionIntensity ("Intensity", Range(0, 1)) = 0.0
        _Smoothness("Smoothness", Float) = 1.0
		_RefMetallic ("Metallic", Range(0, 1) ) = 0.0

        _MaskReflection ("Mask Reflection", 2D) = "white" {}

        _FReflection ("FReflection", 2D) = "black" {}

		_RimLigInt ("Rim Light Intensity", Range(0, 1)) = 1.0
        _RimLightUnfill ("Unfill", Float ) = 1.5
        _RimLighThres ("Threshold", Float ) = 900
        [HDR] _RimLightColor ("Color", Color) = (1.0,1.0,1.0,1.0)
        _RimLightColorPower ("Color Power", Float ) = 1.0
        _RimLightSoftness ("Softness", Range(0, 1)) = 1.0
        [Toggle(N_F_SSRRL_ON)] _SSRimLig ("Screen Space Rim Light", Float ) = 0.0
        [ToggleUI] _RimLightInLight ("Rim Light In Light", Float ) = 1.0
        [ToggleUI] _LightAffectRimLightColor ("Light Affect Rim Light Color", Float ) = 0.0

        _TriPlaTile("Tile", Float) = 1.0
        _TriPlaBlend("Blend", Float) = 4.0

        _MinFadDistance("Min Distance", Float) = 0.0
		_MaxFadDistance("Max Distance", Float) = 2.0

		//Tessellation is still in development
		//_TessellationSmoothness ("Smoothness", Range(0, 1)) = 0.5
		//_TessellationTransition ("Tessellation Transition", Range(0, 1)) = 0.8
        //_TessellationNear ("Tessellation Near", Float ) = 1
        //_TessellationFar ("Tessellation Far", Float ) = 1
		//====================================

		//(See Through) disabled temporarily due to unity HDRP handles the stencil differently.
		//Will be re-enable soon once a new solution is implemented.
		//_RefVal ("ID", int ) = 0
        //[Enum(Blank,8,A,0,B,2)] _Oper("Set 1", int) = 0
        //[Enum(Blank,8,None,4,A,6,B,7)] _Compa("Set 2", int) = 4

		[Toggle(N_F_MC_ON)] _N_F_MC ("MatCap", Float ) = 0.0
		[Toggle(N_F_NM_ON)] _N_F_NM ("Normal Map", Float ) = 0.0
		[Toggle(N_F_CO_ON)] _N_F_CO ("Cutout", Float ) = 0.0
		[Toggle(N_F_O_ON)] _N_F_O ("Outline", Float ) = 1.0
		[Toggle(N_F_CA_ON)] _N_F_CA ("Color Adjustment", Float ) = 0.0
		[Toggle(N_F_SL_ON)] _N_F_SL ("Self Lit", Float ) = 0.0
		[Toggle(N_F_GLO_ON)] _N_F_GLO ("Gloss", Float ) = 0.0
		[Toggle(N_F_GLOT_ON)] _N_F_GLOT ("Gloss Texture", Float ) = 0.0
		[Toggle(N_F_SS_ON)] _N_F_SS ("Self Shadow", Float ) = 1.0
		[Toggle(N_F_SON_ON)] _N_F_SON ("Smooth Object Normal", Float ) = 0.0
		[Toggle(N_F_SCT_ON)] _N_F_SCT ("Shadow Color Texture", Float ) = 0.0
		[Toggle(N_F_ST_ON)] _N_F_ST ("ShadowT", Float ) = 0.0
		[Toggle(N_F_PT_ON)] _N_F_PT ("PTexture", Float ) = 0.0
		[Toggle(N_F_CLD_ON)] _N_F_CLD ("Custom Light Direction", Float ) = 0.0
		[Toggle(N_F_R_ON)] _N_F_R ("Relfection", Float ) = 0.0
		[Toggle(N_F_FR_ON)] _N_F_FR ("FRelfection", Float ) = 0.0
		[Toggle(N_F_RL_ON)] _N_F_RL ("Rim Light", Float ) = 0.0
        [Toggle(N_F_NFD_ON)] _N_F_NFD ("Near Fade Dithering", Float) = 0.0
        [Toggle(N_F_TP_ON)] _N_F_TP ("Triplanar", Float ) = 0.0

		//Temporarily disabled because unity HDRP handles the ztest differently.
		//This might be completely remove soon if it is not really in use.
		//[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", int) = 4

        [ToggleUI] _RecurRen("Recursive Rendering", Float) = 0.0

        [Toggle(N_F_ESSAO_ON)] _N_F_ESSAO("Enable Screen Space Ambient Occlusion", Float) = 0.0
        [HDR] _SSAOColor("Ambient Occlusion Color", Color) = (0.0, 0.0, 0.0, 0.0)

		[Toggle(N_F_ESSS_ON)] _N_F_ESSS ("Enable Screen Space Shadow", Float ) = 1.0
		[Toggle(N_F_ESSR_ON)] _N_F_ESSR ("Enable Screen Space Reflection", Float ) = 1.0
        [Toggle(N_F_ESSGI_ON)] _N_F_ESSGI ("Enable Screen Space Global Illumination", Float) = 1.0

		[Toggle(N_F_HDLS_ON)] _N_F_HDLS ("Hide Directional Light Shadow", Float ) = 0.0
		[Toggle(N_F_HPSAS_ON)] _N_F_HPSAS ("Hide Point, Spot & Area Light Shadows", Float ) = 0.0

		[Toggle(N_F_CS_ON)] _N_F_CS ("Hide Contact Shadow", Float ) = 1.0 
		[Toggle(N_F_DCS_ON)] _N_F_DCS ("Disable Cast Shadow", Float ) = 0.0
		[Toggle(N_F_NLASOBF_ON)] _N_F_NLASOBF ("No Light and Shadow On BackFace", Float ) = 0.0
		[Enum(On,1,Off,0)] _ZWrite("ZWrite", int) = 1

		//Other Properties
		[HideInInspector] _ZTeForLiOpa("ZTeForLiOpa", int) = 3
		[HideInInspector] _SSRefDeOn("SSRefDeOn", int) = 8 //0
		[HideInInspector] _SSRefGBu("SSRefGBu", int) = 10 //2
		[HideInInspector] _SSRefMoVe("SSRefMoVe", int) = 40 //32
		[HideInInspector] _GBuWriMas("GBuWriMas", int) = 14
		[HideInInspector] _MVWriMas("_MVWriMas", int) = 40

    }

    HLSLINCLUDE

    #pragma target 4.5

    //Shader Features

    #pragma shader_feature_local N_F_USETLB_ON
    #pragma shader_feature_local N_F_TRANS_ON
    #pragma shader_feature_local N_F_SIMTRANS_ON

    #pragma shader_feature_local N_F_O_NM_ON
    #pragma shader_feature_local N_F_O_ON
    #pragma shader_feature_local N_F_O_MOTTSO_ON
    #pragma shader_feature_local N_F_MC_ON
    #pragma shader_feature_local N_F_NM_ON
    #pragma shader_feature_local N_F_CO_ON
    #pragma shader_feature_local N_F_SL_ON
    #pragma shader_feature_local N_F_CA_ON
    #pragma shader_feature_local N_F_GLO_ON
    #pragma shader_feature_local N_F_GLOT_ON
    #pragma shader_feature_local N_F_SS_ON
    #pragma shader_feature_local N_F_SCT_ON
    #pragma shader_feature_local N_F_ST_ON
    #pragma shader_feature_local N_F_STIS_ON
    #pragma shader_feature_local N_F_STIAL_ON 
    #pragma shader_feature_local N_F_SON_ON
    #pragma shader_feature_local N_F_PT_ON
    #pragma shader_feature_local N_F_RELGI_ON
    #pragma shader_feature_local N_F_CLD_ON
    #pragma shader_feature_local N_F_R_ON
    #pragma shader_feature_local N_F_FR_ON
    #pragma shader_feature_local N_F_RL_ON
    #pragma shader_feature_local N_F_HDLS_ON
    #pragma shader_feature_local N_F_HPSAS_ON
    #pragma shader_feature_local N_F_PAL_ON
    #pragma shader_feature_local N_F_AL_ON
    #pragma shader_feature_local N_F_ALSL_ON
    #pragma shader_feature_local N_F_NLASOBF_ON
    #pragma shader_feature_local N_F_CS_ON
    #pragma shader_feature_local N_F_DCS_ON
    #pragma shader_feature_local N_F_ESSS_ON
    #pragma shader_feature_local N_F_ESSR_ON
    #pragma shader_feature_local N_F_ESSGI_ON
    #pragma shader_feature_local N_F_ESSAO_ON
    #pragma shader_feature_local N_F_SSRRL_ON
    #pragma shader_feature_local N_F_DNO_ON
    #pragma shader_feature_local N_F_COEDGL_ON
    #pragma shader_feature_local N_F_NFD_ON
    #pragma shader_feature_local N_F_TP_ON
    #pragma shader_feature_local N_F_SCO_ON

    //Others
    #pragma shader_feature_local_fragment _DISABLE_SSR
    #pragma shader_feature_local_raytracing _DISABLE_SSR
    #pragma shader_feature_local _DISABLE_SSR_TRANSPARENT
    #define SUPPORT_GLOBAL_MIP_BIAS

    //Tessellation is still in development
    //#pragma shader_feature_local N_F_TESSE_ON

    //Support recursive rendering for raytracing
    #define HAVE_RECURSIVE_RENDERING

    //Global Includes

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

    //Properties

    #include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/RT_HDRP_PROP.hlsl"

	ENDHLSL



    SubShader
    {

        Tags{"Queue" = "Geometry+225" "RenderPipeline"="HDRenderPipeline" "RenderType" = "HDLitShader"}

        Pass
        {
Name "SceneSelectionPass"
Tags { "LightMode" = "SceneSelectionPass" }

        Cull Off

        HLSLPROGRAM

        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

        #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer
        #pragma multi_compile _ DOTS_INSTANCING_ON

        #pragma multi_compile _ LOD_FADE_CROSSFADE

        #define SHADERPASS SHADERPASS_DEPTH_ONLY
        #define SCENESELECTIONPASS
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitDepthPass.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"
        #include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_DeOnPas.hlsl"

        #pragma vertex Vert
        #pragma fragment Frag

        #pragma editor_sync_compilation

        ENDHLSL
    }

		Pass {

Name"Outline"
Tags{"LightMode"="SRPDefaultUnlit"}
//OL_NRE

			Blend [_BleModSour] [_BleModDest]

Cull [_DoubleSidedOutline]//OL_RCUL
			ZTest LEqual
			ZWrite On

            HLSLPROGRAM

            #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma multi_compile _ LOD_FADE_CROSSFADE

            #define OPAQUE_FOG_PASS

	        #pragma multi_compile_fragment PUNCTUAL_SHADOW_LOW PUNCTUAL_SHADOW_MEDIUM PUNCTUAL_SHADOW_HIGH
	        #pragma multi_compile_fragment DIRECTIONAL_SHADOW_LOW DIRECTIONAL_SHADOW_MEDIUM DIRECTIONAL_SHADOW_HIGH
            #pragma multi_compile_fragment AREA_SHADOW_MEDIUM AREA_SHADOW_HIGH

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

			#define HAS_LIGHTLOOP
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/AtmosphericScattering/AtmosphericScattering.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinGIUtilities.hlsl"

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_OutPas.hlsl"

			ENDHLSL
			
		}
       
		Pass
        {

Name"GBuffer"
Tags{"LightMode"="GBuffer"}

            Cull [_Culling]
			ZTest LEqual    

            Stencil
            {
                WriteMask [_GBuWriMas]
                Ref [_SSRefGBu]
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM

            #pragma only_renderers d3d11 playstation xboxone vulkan xboxseries metal switch

            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma multi_compile _ LOD_FADE_CROSSFADE

            //#pragma multi_compile _ DEBUG_DISPLAY //Temporary Removed (It produces error about "undefined unity_MipmapStreaming_DebugTex_ST")
            #pragma multi_compile_fragment _ SHADOWS_SHADOWMASK

            #pragma multi_compile_fragment PROBE_VOLUMES_OFF PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
            #pragma multi_compile_fragment DECALS_OFF DECALS_3RT DECALS_4RT
            #pragma multi_compile_fragment _ RENDERING_LAYERS

			#ifndef DEBUG_DISPLAY
				#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
			#endif

            #define SHADERPASS SHADERPASS_GBUFFER

            #ifdef DEBUG_DISPLAY
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitSharePass.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_GBuPas.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            ENDHLSL
        }

		Pass
        {

Name"ShadowCaster"
Tags{"LightMode"="ShadowCaster"}

            Cull [_Culling]

			ZClip On
            ZWrite On
            ZTest LEqual

            ColorMask 0

            HLSLPROGRAM

            #pragma only_renderers d3d11 playstation xboxone vulkan xboxseries metal switch

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma multi_compile _ LOD_FADE_CROSSFADE

			#define SHADERPASS SHADERPASS_SHADOWS

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_ShaCasPas.hlsl"
			
            ENDHLSL
        }

		Pass
        {

Name"DepthOnly"
Tags{"LightMode"="DepthOnly"}

            Cull [_Culling]

			Stencil
            {
                WriteMask 8
                Ref [_SSRefDeOn]
                Comp Always
                Pass Replace
            }

            ZWrite On

            HLSLPROGRAM

            #pragma only_renderers d3d11 playstation xboxone vulkan xboxseries metal switch

            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma multi_compile _ LOD_FADE_CROSSFADE

            #pragma multi_compile _ WRITE_NORMAL_BUFFER
            #pragma multi_compile_fragment _ WRITE_MSAA_DEPTH

            #define SHADERPASS SHADERPASS_DEPTH_ONLY

            #ifdef N_F_CO_ON
                #define WRITE_NORMAL_BUFFER
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"

            #ifdef WRITE_NORMAL_BUFFER
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitSharePass.hlsl"
            #else
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitDepthPass.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_DeOnPas.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            ENDHLSL
        }

		Pass
        {

Name"MotionVectors"
Tags{"LightMode"="MotionVectors"}

            Stencil
            {
                WriteMask [_MVWriMas]
                Ref [_SSRefMoVe]
                Comp Always
                Pass Replace
            }

            Cull[_Culling]

            ZWrite On

            HLSLPROGRAM

            #pragma only_renderers d3d11 playstation xboxone vulkan xboxseries metal switch

            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma multi_compile _ LOD_FADE_CROSSFADE

            #pragma multi_compile _ WRITE_NORMAL_BUFFER
            #pragma multi_compile_fragment _ WRITE_MSAA_DEPTH

            #define SHADERPASS SHADERPASS_MOTION_VECTORS

            #ifdef N_F_CO_ON
                #define WRITE_NORMAL_BUFFER
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"

            #ifdef WRITE_NORMAL_BUFFER 
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitSharePass.hlsl"
            #else
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitMotionVectorPass.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_MoVecPas.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            ENDHLSL
        }

        Pass
        {

Name"ForwardLit"
Tags{"LightMode"="ForwardOnly"}

            Blend[_BleModSour][_BleModDest]

            Cull [_Culling]
			ZTest [_ZTeForLiOpa]   
			ZWrite [_ZWrite]

			Stencil {
				WriteMask 6
				Ref 0
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM

            #pragma only_renderers d3d11 playstation xboxone vulkan xboxseries metal switch

			#pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma multi_compile _ LOD_FADE_CROSSFADE

			#define SHADERPASS SHADERPASS_FORWARD
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_COLOR

			#pragma multi_compile _ DEBUG_DISPLAY
            #pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
            #pragma multi_compile_fragment _ PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
            #pragma multi_compile_fragment SCREEN_SPACE_SHADOWS_OFF SCREEN_SPACE_SHADOWS_ON

	        #pragma multi_compile_fragment PUNCTUAL_SHADOW_LOW PUNCTUAL_SHADOW_MEDIUM PUNCTUAL_SHADOW_HIGH
	        #pragma multi_compile_fragment DIRECTIONAL_SHADOW_LOW DIRECTIONAL_SHADOW_MEDIUM DIRECTIONAL_SHADOW_HIGH
            #pragma multi_compile_fragment AREA_SHADOW_MEDIUM AREA_SHADOW_HIGH

			#pragma multi_compile_fragment DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST

            #ifndef SHADER_STAGE_FRAGMENT
                #define SHADOW_LOW
                #define USE_FPTL_LIGHTLIST
            #endif

			#if SHADERPASS != SHADERPASS_FORWARD
			    #error SHADERPASS_is_not_correctly_define
			#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"

			#ifdef DEBUG_DISPLAY
			    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif

			#define HAS_LIGHTLOOP
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/VolumeRendering.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinGIUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/AreaLighting.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/AtmosphericScattering/AtmosphericScattering.hlsl"

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_ForLitPas.hlsl"
			
            ENDHLSL
			 
        }

        Pass
        {

Name"RayTracingPrepass"
Tags{"LightMode"="RayTracingPrepass"}

            Cull[_CullMode]

            ZWrite On
            ZTest LEqual

            HLSLPROGRAM

            #pragma only_renderers d3d11 playstation xboxone vulkan xboxseries metal switch

            #pragma multi_compile _ LOD_FADE_CROSSFADE

            #define SHADERPASS SHADERPASS_CONSTANT

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/ShaderPass/LitConstantPass.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassConstant.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            ENDHLSL

        }

    }



    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }

        Pass
        {

Name"IndirectDXR"
Tags{"LightMode" = "IndirectDXR"}

        HLSLPROGRAM

        #pragma only_renderers d3d11 xboxseries ps5
        #pragma raytracing surface_shader

        #pragma multi_compile _ DEBUG_DISPLAY
        #pragma multi_compile _ PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
        #pragma multi_compile _ MULTI_BOUNCE_INDIRECT

        #define SHADERPASS SHADERPASS_RAYTRACING_INDIRECT

        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_COLOR
        #define SHADOW_LOW

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
        #define HAS_LIGHTLOOP
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitRaytracing.hlsl"
        #define USE_LIGHT_CLUSTER
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingLightCluster.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingFragInputs.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingSampling.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/Common/AtmosphericScatteringRayTracing.hlsl"

        #include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_RT_IndirPas.hlsl"

        ENDHLSL
    }

    Pass
    {

Name"ForwardDXR"
Tags{"LightMode" = "ForwardDXR"}

        HLSLPROGRAM

        #pragma only_renderers d3d11 xboxseries ps5
        #pragma raytracing surface_shader

        #pragma multi_compile _ DEBUG_DISPLAY
        #pragma multi_compile _ PROBE_VOLUMES_L1 PROBE_VOLUMES_L2

        #define SHADERPASS SHADERPASS_RAYTRACING_FORWARD

        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_COLOR
        #define SHADOW_LOW

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
        #define HAS_LIGHTLOOP
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitRaytracing.hlsl"
        #define USE_LIGHT_CLUSTER
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingLightCluster.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingFragInputs.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/Common/AtmosphericScatteringRayTracing.hlsl"

        #include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_RT_ForwPas.hlsl"

        ENDHLSL
    }


    Pass
    {

Name"GBufferDXR"
Tags{"LightMode" = "GBufferDXR"}

        HLSLPROGRAM

        #pragma only_renderers d3d11 xboxseries ps5
        #pragma raytracing surface_shader

        #pragma multi_compile _ DEBUG_DISPLAY
        #pragma multi_compile _ PROBE_VOLUMES_L1 PROBE_VOLUMES_L2

        #define SHADERPASS SHADERPASS_RAYTRACING_GBUFFER

        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_COLOR
        #define SHADOW_LOW

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/Deferred/RaytracingIntersectonGBuffer.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
        #define HAS_LIGHTLOOP
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/StandardLit/StandardLit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitRaytracing.hlsl"
        #define USE_LIGHT_CLUSTER
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingLightCluster.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"

        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingFragInputs.hlsl"

        #include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_RT_GBuPas.hlsl"

        ENDHLSL
    }

    Pass
    {

Name"VisibilityDXR"
Tags{"LightMode" = "VisibilityDXR"}

            HLSLPROGRAM

            #pragma only_renderers d3d11 xboxseries ps5
            #pragma raytracing surface_shader

            #define SHADERPASS SHADERPASS_RAYTRACING_VISIBILITY
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TANGENT

            #pragma multi_compile _ TRANSPARENT_COLOR_SHADOW

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl"

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary\CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingFragInputs.hlsl"

            #include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/Pass/RT_HDRP_RT_VisiPas.hlsl"

            ENDHLSL

          }
    }

FallBack "Hidden/InternalErrorShader"
CustomEditor "RealToon.GUIInspector.RealToonShaderGUI_HDRP_SRP"

}

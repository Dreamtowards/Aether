//RealToon HDRP - OutPas
//MJQStudioWorks

#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/RT_HDRP_Core.hlsl"

struct Attributes
{

    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
	float4 vertexColor	: COLOR;
	float2 uv           : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID

};

struct Varyings
{

    float2 uv                       : TEXCOORD0;
	float4 projPos					: TEXCOORD1;
	float3 positionWS				: TEXCOORD2;
	float3 normalWS					: TEXCOORD3;
	float4 vertexColor				: COLOR;
    float4 positionCS               : SV_POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO

};

Varyings LitPassVertex(Attributes input)
{

	Varyings output;
	ZERO_INITIALIZE(Varyings, output);

	UNITY_SETUP_INSTANCE_ID (input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.uv = input.uv;
    output.vertexColor = input.vertexColor;

	float4 objPos = mul ( GetObjectToWorldMatrix(), float4(0.0,0.0,0.0,1.0) );
	float RTD_OB_VP_CAL = distance(objPos.rgb,GetCurrentViewPosition());

	float RTD_OL_VCRAOW_OO = lerp(_OutlineWidth, (_OutlineWidth * (1.0 - output.vertexColor.b)), _VertexColorBlueAffectOutlineWitdh);

	float RTD_OL_OLWABVD_OO = lerp( RTD_OL_VCRAOW_OO, ( clamp(RTD_OL_VCRAOW_OO*RTD_OB_VP_CAL, RTD_OL_VCRAOW_OO, _FarDistanceMaxWidth) ), _OutlineWidthAffectedByViewDistance );
	float4 _OutlineWidthControl_var = SAMPLE_TEXTURE2D_LOD(_OutlineWidthControl, sampler_OutlineWidthControl, TRANSFORM_TEX(output.uv, _OutlineWidthControl), 0.0);

	#if N_F_DNO_ON

		float4 node_3726 = _Time;
		float node_8530_ang = node_3726.g;
		float node_8530_spd = 0.002;
		float node_8530_cos = cos(node_8530_spd*node_8530_ang);
		float node_8530_sin = sin(node_8530_spd*node_8530_ang);
		float2 node_8530_piv = float2(0.5,0.5);
		float2 node_8530 = (mul(output.uv-node_8530_piv,float2x2( node_8530_cos, -node_8530_sin, node_8530_sin, node_8530_cos))+node_8530_piv);

		float2 RTD_OL_DNOL_OO = node_8530;

	#else

		float2 RTD_OL_DNOL_OO = output.uv;

	#endif

	float2 node_8743 = RTD_OL_DNOL_OO;

    float2 node_1283_skew = node_8743 + 0.2127+node_8743.x*0.3713*node_8743.y;
    float2 node_1283_rnd = 4.789*sin(489.123*(node_1283_skew));
    float node_1283 = frac(node_1283_rnd.x*node_1283_rnd.y*(1+node_1283_skew.x));

	output.normalWS = TransformObjectToWorldNormal(input.normalOS);

	#if N_F_O_NM_ON

		float3 _ONormMap_var = UnpackNormal(SAMPLE_TEXTURE2D_LOD(_ONormMap, sampler_ONormMap, TRANSFORM_TEX(output.uv, _ONormMap), 0.0));
		float3 _normloc = normalize(input.normalOS.xyz + ( lerp( float3(0.0,0.0,1.0), _ONormMap_var.rgb, _ONormMapInt ) * float3(1.0, 1.0, 0.0) ) );
				
	#else
					
		float3 _normloc = input.normalOS.xyz;

	#endif

	_OEM = lerp(_normloc, SafeNormalize(input.positionOS.xyz), _OutlineExtrudeMethod);

	float RTD_OL = ( RTD_OL_OLWABVD_OO*0.01 )*_OutlineWidthControl_var.r*lerp(1.0,node_1283,_NoisyOutlineIntensity);
    output.positionCS = TransformWorldToHClip(  TransformObjectToWorld( (input.positionOS.xyz + _OutlineOffset.xyz * 0.01) + _OEM * RTD_OL ) );

	#if defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)
		output.positionCS.z += _OutlineZPostionInCamera * 0.0005;
	#else
		output.positionCS.z -= _OutlineZPostionInCamera * 0.0005;
	#endif

    output.projPos = ComScrPos (output.positionCS);
	output.positionWS = TransformObjectToWorld(input.positionOS.xyz);

    return output;

}


void LitPassFragment(Varyings input
				, out float4 outColor : SV_Target0
			#ifdef UNITY_VIRTUAL_TEXTURING
				, out float4 outVTFeedback : SV_Target1
			#endif
			#ifdef _DEPTHOFFSET_ON
				, out float outputDepth : DEPTH_OFFSET_SEMANTIC
			#endif
)
{

	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	UNITY_SETUP_INSTANCE_ID (input);
    BuiltinData builtinData;

	PositionInputs posInput = GetPositionInput(input.positionCS.xy, _ScreenSize.zw, input.positionCS.z, input.positionCS.w, input.positionWS);

	float3 VDir = GetWorldSpaceNormalizeViewDir(input.positionWS);

	float3 color = (float3)1.0;
	float3 lightColor = (float3)0.0;

	#if UNITY_VERSION >= 202310
		builtinData.renderingLayers = GetMeshRenderingLayerMask();
	#else
		builtinData.renderingLayers = _EnableLightLayers ? asuint(unity_RenderingLayer.x) : DEFAULT_LIGHT_LAYERS;
	#endif

	for (uint i = 0; i < _DirectionalLightCount; ++i)
	{
		DirectionalLightData light = _DirectionalLightDatas[i];

		if(IsMatchingLightLayer(light.lightLayers, builtinData.renderingLayers))
		{
			lightColor += light.color * GetCurrentExposureMultiplier();
		}
	}

	float4 objPos = float4( TransformObjectToWorld( float3(0.0,0.0,0.0) ), 1.0 );
    float2 sceneUVs = (input.projPos.xy / input.projPos.w);

	float RTD_OB_VP_CAL = distance(objPos.rgb, GetCurrentViewPosition());
	float2 RTD_VD_Cal = (float2((sceneUVs.x * 2.0 - 1.0)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2.0 - 1.0).rg*RTD_OB_VP_CAL);

	float2 RTD_TC_TP_OO = lerp( input.uv, RTD_VD_Cal, _TexturePatternStyle );

	#ifdef N_F_TP_ON
		float4 _MainTex_var = RT_Tripl_Default(_MainTex, sampler_MainTex, input.positionWS.xyz, input.normalWS);
	#else
		float4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, TRANSFORM_TEX(RTD_TC_TP_OO, _MainTex));
	#endif

	#if !defined(SHADER_STAGE_RAY_TRACING) //&& !defined(_TESSELLATION_DISPLACEMENT)
		#ifdef LOD_FADE_CROSSFADE 
			LODDitheringTransition(ComputeFadeMaskSeed(VDir, posInput.positionSS), unity_LODFade.x);
		#endif
	#endif

	//RT_TRANS_CO
	float RTD_TRAN_OPA_Sli;
	bool bo_co_val;
	float RTD_CO;
	float3 GLO_OUT = (float3)0.0;
    RT_TRANS_CO(input.uv, _MainTex_var, RTD_TRAN_OPA_Sli, RTD_CO, bo_co_val, false, input.positionWS.xyz, input.normalWS, input.positionCS.xy, GLO_OUT);

#if N_F_PAL_ON

if (LIGHTFEATUREFLAGS_PUNCTUAL)
{

	float4 distances = (float4)0.0;
	uint p=0;
	uint p_en=0;
				
	#ifdef LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
		GetCountAndStart(posInput, LIGHTCATEGORY_PUNCTUAL, p, p_en);
	#else
		p = 0;
		p_en = _PunctualLightCount;
	#endif

	for (p; p < p_en; ++p)
	{
		LightData Plight = FetchLight(p);

		if(IsMatchingLightLayer(Plight.lightLayers, builtinData.renderingLayers))
		{
			float3 lightToSample = input.positionWS - Plight.positionRWS;

			distances.w = dot(lightToSample, Plight.forward);

			if (Plight.lightType == GPULIGHTTYPE_PROJECTOR_BOX)
			{
				distances.xyz = (float3)1.0;
			}
			else
			{
				float3 unL     = -lightToSample;
				float  distSq  = dot(unL, unL);
				float  distRcp = rsqrt(distSq);
				float  dist    = distSq * distRcp;

				float3 PuncLigDir = unL * distRcp;
				distances.xyz = float3(dist, distSq, distRcp);

				ModifyDistancesForFillLighting(distances, Plight.size.x);
			}

			lightColor += (Plight.color * GetCurrentExposureMultiplier()) * PunctualLightAttenuation(distances, Plight.rangeAttenuationScale , Plight.rangeAttenuationBias, Plight.angleScale, Plight.angleOffset);

		}
	}

}
#endif


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_OutlineColor = float4(LinearToGamma22(_OutlineColor.rgb), _OutlineColor.a);
	#endif

	float3 RTD_OL_LAOC_OO = lerp( lerp(_OutlineColor.rgb,_OutlineColor.rgb * _MainTex_var.rgb, _MixMainTexToOutline) , lerp(float3(0.0, 0.0, 0.0), lerp(_OutlineColor.rgb,_OutlineColor.rgb * _MainTex_var.rgb, _MixMainTexToOutline) ,lightColor.rgb), _LightAffectOutlineColor );
	//


	#if N_F_TRANS_ON

		float Trans_Val = 1.0;
					
		#ifndef N_F_CO_ON

			if(_TOAO == 1)
			{
				Trans_Val = RTD_TRAN_OPA_Sli;
			}
			else
			{
				clip(RTD_TRAN_OPA_Sli - 0.5);
				Trans_Val = 1.0;
			}
	
		#endif
					
	#else

		float Trans_Val = 1.0;

	#endif

	//RT_NFD
	#ifdef N_F_NFD_ON
		RT_NFD(input.positionCS.xy);
	#endif
		
	//
	#ifdef _DEPTHOFFSET_ON 
		outputDepth = posInput.deviceDepth;
	#endif
	//

	float4 finalRGBA = float4(RTD_OL_LAOC_OO, Trans_Val);
    outColor = EL_AT_SC(posInput, VDir, finalRGBA);

	//
	#ifdef UNITY_VIRTUAL_TEXTURING
		float vtAlphaValue = builtinData.opacity;
		outVTFeedback = PackVTFeedbackWithAlpha(builtinData.vtPackedFeedback, input.positionSS.xy, finalRGBA);
	#endif
	//


}
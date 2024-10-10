//RealToon HDRP - ForLitPas
//MJQStudioWorks

#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/RT_HDRP_Core.hlsl"
#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/RT_HDRP_Lit_Func_For_Lit.hlsl"
			
//Tessellation is still in development
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"

#if SHADERPASS != SHADERPASS_FORWARD
	#error SHADERPASS_is_not_correctly_define
#endif

struct Attributes
{

    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
	float4 vertexColor	: COLOR;
	float2 uv           : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID

};

struct Varyings
{

    float2 uv                       : TEXCOORD0;
    float3 normalWS					: TEXCOORD1;
    float4 tangentWS                : TEXCOORD2;
    float3 bitangentWS              : TEXCOORD3;
	float3 positionWS				: TEXCOORD4;
	float4 projPos					: TEXCOORD5;
	float3 smoNorm					: TEXCOORD6;
    float4 positionCS				: SV_POSITION;
	float4 vertexColor				: COLOR;
#ifdef UNITY_VIRTUAL_TEXTURING
	float4 outVTFeedback			: VT_BUFFER_TARGET;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO

};

Varyings LitPassVertex(Attributes input)
{

    Varyings output;
	ZERO_INITIALIZE(Varyings, output);

#if defined(HAVE_RECURSIVE_RENDERING)
	if (_EnableRecursiveRayTracing && _RecurRen > 0.0)
	{
	}
	else
#endif
	{
		UNITY_SETUP_INSTANCE_ID (input);
		UNITY_TRANSFER_INSTANCE_ID(input, output);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
				
		output.normalWS = TransformObjectToWorldNormal(input.normalOS);
		output.tangentWS = float4(TransformObjectToWorldDir(input.tangentOS.xyz), input.tangentOS.w);
		output.bitangentWS = cross(output.normalWS, output.tangentWS.xyz) * (input.tangentOS.w * GetOddNegativeScale());
		output.positionWS = TransformObjectToWorld(input.positionOS.xyz);

		output.uv = input.uv;
		output.vertexColor = input.vertexColor;

		#if N_F_SON_ON
			output.smoNorm = calcNorm(float3(input.positionOS.x + ((_XYZPosition.x) * 0.1), input.positionOS.y + ((_XYZPosition.y) * 0.1), input.positionOS.z + ((_XYZPosition.z) * 0.1)));
		#endif

		output.positionCS = TransformWorldToHClip(output.positionWS);
		output.projPos = ComScrPos(output.positionCS);

	}

    return output;
}

void LitPassFragment(Varyings input 
				, out float4 outColor : SV_Target0
			#ifdef UNITY_VIRTUAL_TEXTURING
				, out float4 outVTFeedback : SV_Target1
			#endif
				, float facing : VFACE
			#ifdef _DEPTHOFFSET_ON
				, out float outputDepth : DEPTH_OFFSET_SEMANTIC
			#endif
)
{

	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	UNITY_SETUP_INSTANCE_ID (input);

	input.positionCS.xy = _OffScreenRendering > 0 ? (input.positionCS.xy * _OffScreenDownsampleFactor) : input.positionCS.xy;
	uint2 tileIndex = uint2(input.positionCS.xy) / GetTileSize();
	PositionInputs posInput = GetPositionInput(input.positionCS.xy, _ScreenSize.zw, input.positionCS.z, input.positionCS.w, input.positionWS.xyz, tileIndex);

	float isFrontFace = ( facing >= 0 ? 1 : 0 );
	float4 objPos = mul ( GetObjectToWorldMatrix(), float4(0.0,0.0,0.0,1.0) );
	float2 sceneUVs = (input.projPos.xy / input.projPos.w);

	input.normalWS = SafeNormalize(input.normalWS);
    float3x3 tangentTransform = float3x3( input.tangentWS.xyz, input.bitangentWS, input.normalWS);
    float3 viewDirection = GetWorldSpaceNormalizeViewDir(input.positionWS);
		
	//RT_NM
    float3 normalLocal = RT_NM(input.uv, input.positionWS.xyz, tangentTransform, input.normalWS);
		
    float3 normalDirection = SafeNormalize(mul( normalLocal, tangentTransform ));
    float3 viewReflectDirection = reflect( -viewDirection, normalDirection );

	float2 RTD_OB_VP_CAL = distance(objPos.xyz, GetCurrentViewPosition());
	float2 RTD_VD_Cal = (float2((sceneUVs.x * 2.0 - 1.0)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2.0 - 1.0).rg*RTD_OB_VP_CAL);
	float2 RTD_TC_TP_OO = lerp( input.uv, RTD_VD_Cal, _TexturePatternStyle );

	#if !defined(SHADER_STAGE_RAY_TRACING) //&& !defined(_TESSELLATION_DISPLACEMENT)
		#ifdef LOD_FADE_CROSSFADE 
			LODDitheringTransition(ComputeFadeMaskSeed(viewDirection, posInput.positionSS), unity_LODFade.x);
		#endif
	#endif


	//=========//
		

	#ifdef N_F_TP_ON
		float4 _MainTex_var = RT_Tripl_Default(_MainTex, sampler_MainTex, input.positionWS.xyz, input.normalWS); 
	#else
		float4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex , TRANSFORM_TEX(RTD_TC_TP_OO, _MainTex) );
	#endif

	float3 _RTD_MVCOL = lerp((float3)1.0, input.vertexColor.rgb, _MVCOL);


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_OverallShadowColor = float4(LinearToGamma22(_OverallShadowColor.rgb), _OverallShadowColor.a);
	#endif

	float3 RTD_OSC = (_OverallShadowColor.rgb*_OverallShadowColorPower);
	//


	//RT_MCAP
	float3 MCapOutP = RT_MCAP( input.uv , normalDirection );


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_MainColor = float4(LinearToGamma22(_MainColor.rgb),_MainColor.a) * _MaiColPo;
	#endif
	//

	//RT_MCAP_SUB1
	float3 RTD_TEX_COL;
	float3 RTD_MCIALO_IL = RT_MCAP_SUB1( MCapOutP , _MainTex_var, _RTD_MVCOL , RTD_TEX_COL);
	//

	//RT_TRANS_CO
	float RTD_TRAN_OPA_Sli;
	bool bo_co_val;
	float RTD_CO;
	float3 GLO_OUT = (float3)0.0;
    RT_TRANS_CO(input.uv, _MainTex_var, RTD_TRAN_OPA_Sli, RTD_CO, bo_co_val, false, input.positionWS.xyz, normalDirection, input.positionCS.xy, GLO_OUT);

	//RT_SON
	float3 RTD_SON_CHE_1;
	float3 RTD_SON = RT_SON( input.vertexColor ,  input.smoNorm , normalDirection , RTD_SON_CHE_1 );

	//RT_RELGI
	float3 RTD_GI_FS_OO = RT_RELGI( RTD_SON );

	//RT_SCT
	float3 RTD_SCT = RT_SCT( input.uv, input.positionWS.xyz, normalDirection, RTD_MCIALO_IL );

	//RT_PT
	float3 RTD_PT_COL;
	float RTD_PT = RT_PT( RTD_VD_Cal , RTD_PT_COL );


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_SelfShadowRealTimeShadowColor = float4(LinearToGamma22(_SelfShadowRealTimeShadowColor.rgb), _SelfShadowRealTimeShadowColor.a);
	#endif

	float3 ss_col = lerp( RTD_PT_COL, (_SelfShadowRealTimeShadowColor.rgb * _SelfShadowRealTimeShadowColorPower) * RTD_OSC * RTD_SCT, RTD_PT);
	//

	//=========//
	//=========//

	float3 color = float3(0.0,0.0,0.0);
	float3 main_light_output = float3(0.0,0.0,0.0);
	float3 A_L_O = float3(0.0,0.0,0.0);
	float3 Ar_L_O = float3(0.0,0.0,0.0);

	//=========//
	//=========//

	LightLoopContext context;
	context.shadowContext  = InitShadowContext();
	context.shadowValue = 1;			
	context.sampleReflection = 0;
	//context.splineVisibility = -1;

	#ifdef APPLY_FOG_ON_SKY_REFLECTIONS
		context.positionWS = posInput.positionWS;
	#endif
	context.contactShadowFade = 0.0;
	context.contactShadow = 0;

	uint i=0;
	uint i_en=0;

	InitContactShadow(posInput, context);

	BuiltinData builtinData;
	ZERO_INITIALIZE(BuiltinData,builtinData);
		
	#ifdef SHADOWS_SHADOWMASK
		float4 shaMask = SampleShadowMask(posInput.positionWS, input.uv.xy);
		builtinData.shadowMask0 = shaMask.x;
		builtinData.shadowMask1 = shaMask.y;
		builtinData.shadowMask2 = shaMask.z;
		builtinData.shadowMask3 = shaMask.w;
	#endif

	#if UNITY_VERSION >= 202310
		builtinData.renderingLayers = GetMeshRenderingLayerMask();
	#else
		builtinData.renderingLayers = _EnableLightLayers ? asuint(unity_RenderingLayer.x) : DEFAULT_LIGHT_LAYERS;
	#endif

	//=========//
	//=========//

	//RT_REF_ENV
	float3 EnvRef = RT_REF_ENV( builtinData , normalDirection , viewReflectDirection , posInput , context , (float3)0.0 , false );

	//=========//
	//=========//


	//=========//
	//=========//

	//RT_SSAO//

	float3 SSAmOc = RT_SSAO(posInput.positionSS);

	//=========//
	//=========//

	float4 DirLigCol = float4(0.0,0.0,0.0,0.0);
	float3 DirLigDir = float3(0.0,0.0,0.0);
	float DirSha = 1.0;
	float DirLigDim = 0.0;
	float DirDifDim = 1.0;
	float DirSpecDim = 1.0;
	float RTD_RT_GI_Sha_FO = 1.0;

	if (LIGHTFEATUREFLAGS_DIRECTIONAL)
	{

		i=0;

		for (i = 0; i < _DirectionalLightCount; ++i)
		{

			DirectionalLightData light = _DirectionalLightDatas[i];

										
			if (IsMatchingLightLayer(light.lightLayers, builtinData.renderingLayers))
			{

				RT_DL( context, light , DirLigCol , DirLigDir , DirSha , DirLigDim , DirDifDim , DirSpecDim , posInput , builtinData );

			}

		}

	}

	//=========//
	//=========//


	#if N_F_NLASOBF_ON
		float3 lightColor = lerp(float3(0.0,0.0,0.0),DirLigCol.rgb * DirDifDim,isFrontFace);
	#else
		float3 lightColor = DirLigCol.rgb * DirDifDim;
	#endif
				
	#if N_F_HDLS_ON

		float attenuation = 1.0; 
	#else
		float dlshmin = lerp(0.0,0.6,_ShadowHardness);
		float dlshmax = lerp(1.0,0.6,_ShadowHardness);

		#if N_F_NLASOBF_ON
			float FB_Check = lerp(1.0,DirSha,isFrontFace);
		#else
			float FB_Check = DirSha;
		#endif

		float attenuation = smoothstep(dlshmin,dlshmax,FB_Check);
	#endif

	float3 lightDirection = DirLigDir;
	float3 halfDirection = normalize(viewDirection + lightDirection);

	//RT_CLD
	float3 RTD_CLD = RT_CLD( lightDirection );

	float3 RTD_ST_SS_AVD_OO = lerp( RTD_CLD, viewDirection, _SelfShadowShadowTAtViewDirection );
	float RTD_NDOTL = 0.5*dot(RTD_ST_SS_AVD_OO, float3(RTD_SON.x, RTD_SON.y * (1 - _LigIgnoYNorDir), RTD_SON.z))+0.5;

	float RTD_LVLC = RTD_LVLC_F(lightColor.rgb * 0.2);
	float3 lig_col_int = (_LightIntensity * lightColor.rgb);

	float3 RTD_LAS = lerp((ss_col * 7) * RTD_LVLC,( ss_col * lig_col_int ),_LightAffectShadow);


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_HighlightColor = float4(LinearToGamma22(_HighlightColor.rgb), _HighlightColor.a);
	#endif

	float3 RTD_HL = (_HighlightColor.rgb*_HighlightColorPower+_DirectionalLightIntensity);
	//


	float3 RTD_MCIALO = lerp(RTD_TEX_COL , lerp( lerp( (RTD_TEX_COL * _MainColor.rgb), (RTD_TEX_COL + _MainColor.rgb), _SPECMODE) , _MainTex_var.rgb * MCapOutP * _RTD_MVCOL * 0.7 , clamp((RTD_LVLC*1.0),0.0,1.0) ) , _MCIALO );


	//RT_GLO
	float RTD_GLO; 
	float3 RTD_GLO_COL;
	RT_GLO( input.uv, RTD_VD_Cal, halfDirection, normalDirection, viewDirection, input.positionWS.xyz, DirSpecDim, RTD_GLO, RTD_GLO_COL);
	float3 RTD_GLO_OTHERS = RTD_GLO;

	//RT_RL
	float3 RTD_RL_LARL_OO;
	float RTD_RL_MAIN;
    float RTD_RL_CHE_1 = RT_RL(viewDirection, normalDirection, lightColor, sceneUVs.xy, input.positionCS, RTD_RL_LARL_OO, RTD_RL_MAIN);

	//RT_ST
	float3 RTD_SHAT_COL;
	float RTD_STIAL;
	float RTD_ST_IS;
	float3 RTD_ST_LAF;
	float RTD_ST = RT_ST ( input.uv, input.positionWS.xyz, normalDirection, RTD_NDOTL, attenuation, RTD_LVLC, RTD_PT_COL, lig_col_int, RTD_SCT, RTD_OSC, RTD_PT, RTD_SHAT_COL, RTD_STIAL, RTD_ST_IS, RTD_ST_LAF );

	//RT_SS
	float RTD_SS = RT_SS( input.vertexColor , RTD_NDOTL , attenuation , DirLigDim );

	float3 RTD_R_OFF_OTHERS = lerp( lerp( RTD_ST_LAF, RTD_LAS, RTD_ST_IS) * RTD_RT_GI_Sha_FO , lerp( RTD_ST_LAF, lerp( lerp( RTD_MCIALO_IL * RTD_HL , RTD_GLO_COL , RTD_GLO_OTHERS) , RTD_RL_LARL_OO , RTD_RL_CHE_1 ) * lightColor.rgb, RTD_ST) , RTD_SS); //All mixed by lerp

	//RT_RELGI_SUB1
	float ref_int_val;
	float3 RTD_SL_OFF_OTHERS = RT_RELGI_SUB1(posInput, viewReflectDirection, viewDirection, RTD_GI_FS_OO, RTD_SHAT_COL, RTD_MCIALO, RTD_STIAL, RTD_RT_GI_Sha_FO, ref_int_val, (float3)0.0, false);

	//RT_R
	float3 RTD_R = RT_R( input.uv, viewDirection, normalDirection, input.positionWS.xyz, EnvRef, RTD_TEX_COL, RTD_R_OFF_OTHERS );

	//RT_SL
	float3 RTD_SL_CHE_1;
    float3 RTD_SL = RT_SL( input.uv, input.positionWS.xyz, normalDirection, RTD_SL_OFF_OTHERS, RTD_TEX_COL, RTD_R, RTD_SL_CHE_1 );

	//RT_RL_SUB1
	float3 RTD_RL = RT_RL_SUB1( RTD_SL_CHE_1 , RTD_RL_LARL_OO , RTD_RL_MAIN);

	float3 RTD_CA_OFF_OTHERS = (RTD_RL + RTD_SL);

	main_light_output = RTD_CA_OFF_OTHERS;
        

	//=========//
	//=========//

		
#if N_F_PAL_ON

if (LIGHTFEATUREFLAGS_PUNCTUAL)
{

	float4 PunLigCol = float4(1.0,1.0,1.0,1.0);
	float3 PuncLigDir = float3(0.0,0.0,0.0);
	float PuncSha = 1.0;
	float PuncLF = 1.0;

	i=0;
	i_en=0;

	#ifdef LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
		GetCountAndStart(posInput, LIGHTCATEGORY_PUNCTUAL, i, i_en);
	#else
		i = 0;
		i_en = _PunctualLightCount;
	#endif

    for (i = 0; i < i_en; ++i)
    {
        LightData Plight = FetchLight(i);

		if (IsMatchingLightLayer(Plight.lightLayers, builtinData.renderingLayers))
		{
						
			//RT_PL
			RT_PL( context , Plight , input.positionWS , PunLigCol , PuncLigDir , PuncSha , PuncLF , posInput , builtinData );

			//=============================

		float3 lightDirection = PuncLigDir;


		#if N_F_NLASOBF_ON
			float3 lightColor = lerp( (float3)0.0,PunLigCol.rgb * Plight.diffuseDimmer,isFrontFace);
		#else
			float3 lightColor = PunLigCol.rgb * Plight.diffuseDimmer;
		#endif

		float RTD_LVLC = RTD_LVLC_F(lightColor.rgb * 0.2);

		float3 halfDirection = normalize(viewDirection+lightDirection);

		#if N_F_HPSAS_ON
			float attenuation = 1.0; 
		#else
			float dlshmin = lerp(0.0,0.6,_ShadowHardness);
			float dlshmax = lerp(1.0,0.6,_ShadowHardness);

			#if N_F_NLASOBF_ON
				float FB_Check = lerp(1.0,PuncSha,isFrontFace);
			#else
				float FB_Check = PuncSha;
			#endif

			float attenuation = smoothstep(dlshmin, dlshmax ,FB_Check);
		#endif

		float lightfos = smoothstep(0.0, _LightFalloffSoftness ,PuncLF);

		float3 lig_col_int = (_LightIntensity * lightColor.rgb);

		float3 RTD_LAS = lerp ((ss_col * 7) * RTD_LVLC, ( ss_col * lig_col_int ), _LightAffectShadow);
		float3 RTD_HL = (_HighlightColor.rgb*_HighlightColorPower+_PointSpotlightIntensity);

		float3 RTD_MCIALO = lerp(RTD_TEX_COL , lerp( lerp( (RTD_TEX_COL * _MainColor.rgb), (RTD_TEX_COL + _MainColor.rgb), _SPECMODE) , _MainTex_var.rgb * MCapOutP * _RTD_MVCOL * 0.7 , clamp((RTD_LVLC*1.0),0.0,1.0) ) , _MCIALO );

		//RT_GLO
		float RTD_GLO; 
		float3 RTD_GLO_COL;
		RT_GLO( input.uv, RTD_VD_Cal, halfDirection, normalDirection, viewDirection, input.positionWS.xyz, Plight.specularDimmer, RTD_GLO, RTD_GLO_COL );
		float3 RTD_GLO_OTHERS = RTD_GLO;

		//RT_RL
		float3 RTD_RL_LARL_OO;
		float RTD_RL_MAIN;
		float RTD_RL_CHE_1 = RT_RL( viewDirection , normalDirection , lightColor, sceneUVs.xy, input.positionCS, RTD_RL_LARL_OO , RTD_RL_MAIN );

		//RT_CLD
		float3 RTD_CLD = RT_CLD( lightDirection );

		float3 RTD_ST_SS_AVD_OO = lerp( RTD_CLD, viewDirection, _SelfShadowShadowTAtViewDirection );
		float RTD_NDOTL = 0.5*dot(RTD_ST_SS_AVD_OO, float3(RTD_SON.x, RTD_SON.y * (1 - _LigIgnoYNorDir), RTD_SON.z))+0.5;

		//RT_ST
		float3 RTD_SHAT_COL;
		float RTD_STIAL;
		float RTD_ST_IS;
		float3 RTD_ST_LAF;
		float RTD_ST = RT_ST ( input.uv, input.positionWS.xyz, normalDirection, RTD_NDOTL, lightfos, RTD_LVLC, RTD_PT_COL, lig_col_int, RTD_SCT, RTD_OSC, RTD_PT, RTD_SHAT_COL, RTD_STIAL, RTD_ST_IS, RTD_ST_LAF );

		//RT_SS
		float RTD_SS = RT_SS( input.vertexColor , RTD_NDOTL , attenuation , Plight.shadowDimmer );


		float3 RTD_R_OFF_OTHERS = lerp( lerp( RTD_ST_LAF, RTD_LAS, RTD_ST_IS) * RTD_RT_GI_Sha_FO , lerp( RTD_ST_LAF, lerp( lerp( RTD_MCIALO_IL * RTD_HL , RTD_GLO_COL , RTD_GLO_OTHERS) , RTD_RL_LARL_OO , RTD_RL_CHE_1 ) * lightColor.rgb, RTD_ST) , RTD_SS );


		//RT_R
		float3 RTD_R = RT_R( input.uv, viewDirection, normalDirection, input.positionWS.xyz, EnvRef, RTD_TEX_COL, RTD_R_OFF_OTHERS );
					
		//RT_SL
		//float3 RTD_SL_CHE_1;
		float3 RTD_SL = RT_SL( input.uv, input.positionWS.xyz, normalDirection, (float3)0.0, RTD_TEX_COL, RTD_R, RTD_SL_CHE_1 );

		//RT_RL_SUB1
		float3 RTD_RL = RT_RL_SUB1( RTD_SL_CHE_1 , RTD_RL_LARL_OO , RTD_RL_MAIN);

		float3 RTD_CA_OFF_OTHERS = (RTD_RL + RTD_SL);

		float3 punctual_light_output = RTD_CA_OFF_OTHERS * lightfos;


		#if N_F_USETLB_ON
			A_L_O += punctual_light_output;
		#else
			A_L_O = max (punctual_light_output,A_L_O);
		#endif

		}

	}
			
}


#endif

	//=========//
	//=========//

#if SHADEROPTIONS_AREA_LIGHTS

#if N_F_AL_ON

	if (LIGHTFEATUREFLAGS_AREA)
	{

		float4 ALigCol = float4(1.0,1.0,1.0,1.0);
		float3 ALigDir = float3(0.0,0.0,0.0);
		float ASha = 1.0;
		float ALF = 1.0;
		float ALF2 = 0.0;
		float3 ALCk = float3(1.0,1.0,1.0);

		uint NPLC = 0;
		uint NALC = 0;

		NALC = _AreaLightCount;
		NPLC = _PunctualLightCount;

		if (NALC > 0)
		{

			i=0;

			uint last = NALC - 1;
			LightData Alight = _LightDatas[NPLC+i];

			while (i <= last )
			{

				if(IsMatchingLightLayer(Alight.lightLayers, builtinData.renderingLayers))
				{
								
					//RT_AL
					RT_AL( context , Alight , posInput , viewDirection , normalDirection , input.positionWS , objPos , ALigCol , ALigDir , ASha , ALF , ALF2 , ALCk , builtinData );

					float3 lightDirection = ALigDir;

					#if N_F_NLASOBF_ON
						float3 lightColor = lerp( (float3)0.0,ALigCol.rgb * Alight.diffuseDimmer ,isFrontFace);
					#else
						float3 lightColor = ALigCol.rgb * Alight.diffuseDimmer;
					#endif

					float RTD_LVLC = RTD_LVLC_F(lightColor.rgb * 0.2);

					float3 halfDirection = normalize(viewDirection+lightDirection);

					#if N_F_HPSAS_ON
						float attenuation = 1.0; 
					#else
						float dlshmin = lerp(0.0,0.6,_ShadowHardness);
						float dlshmax = lerp(1.0,0.6,_ShadowHardness);

						#if N_F_NLASOBF_ON
							float FB_Check = lerp(1.0,ASha,isFrontFace);
						#else
							float FB_Check = ASha;
						#endif

						float attenuation = smoothstep(dlshmin, dlshmax ,FB_Check);
					#endif

					float lightfos = (ALF * ALF2);

					float3 lig_col_int = (_LightIntensity * lightColor.rgb);

					float3 RTD_LAS = lerp ((ss_col * 7) * RTD_LVLC, ( ss_col * lig_col_int ), _LightAffectShadow);
					float3 RTD_HL = (_HighlightColor.rgb*_HighlightColorPower+_ALIntensity);

					float3 RTD_MCIALO = lerp(RTD_TEX_COL , lerp( lerp( (RTD_TEX_COL * _MainColor.rgb), (RTD_TEX_COL + _MainColor.rgb), _SPECMODE) , _MainTex_var.rgb * MCapOutP * _RTD_MVCOL * 0.7 , clamp((RTD_LVLC*1.0),0.0,1.0) ) , _MCIALO );

					//RT_GLO
					float RTD_GLO; 
					float3 RTD_GLO_COL;
					RT_GLO( input.uv, RTD_VD_Cal, halfDirection, normalDirection, viewDirection, input.positionWS.xyz, Alight.specularDimmer, RTD_GLO, RTD_GLO_COL );

					float RTD_GLO_OTHERS = RTD_GLO;

					//RT_RL
					float3 RTD_RL_LARL_OO;
					float RTD_RL_MAIN;
					float RTD_RL_CHE_1 = RT_RL( viewDirection , normalDirection , lightColor, sceneUVs.xy, input.positionCS, RTD_RL_LARL_OO , RTD_RL_MAIN );

					//RT_CLD
					float3 RTD_CLD = RT_CLD( lightDirection );

					float3 RTD_ST_SS_AVD_OO = lerp( RTD_CLD, viewDirection, _SelfShadowShadowTAtViewDirection );
					float RTD_NDOTL = 0.5*dot(RTD_ST_SS_AVD_OO, float3(RTD_SON.x, RTD_SON.y * (1 - _LigIgnoYNorDir), RTD_SON.z))+0.5;

					//RT_ST
					float3 RTD_SHAT_COL;
					float RTD_STIAL;
					float RTD_ST_IS;
					float3 RTD_ST_LAF;
					float RTD_ST = RT_ST ( input.uv, input.positionWS.xyz, normalDirection, RTD_NDOTL, lightfos, RTD_LVLC, RTD_PT_COL, lig_col_int, RTD_SCT, RTD_OSC, RTD_PT, RTD_SHAT_COL, RTD_STIAL, RTD_ST_IS, RTD_ST_LAF );

					//RT_SS
					float RTD_SS = RT_SS( input.vertexColor , RTD_NDOTL , attenuation , Alight.shadowDimmer );


					float3 RTD_R_OFF_OTHERS = lerp( lerp( RTD_ST_LAF, RTD_LAS, RTD_ST_IS) * RTD_RT_GI_Sha_FO, lerp( RTD_ST_LAF, lerp( lerp( RTD_MCIALO_IL * RTD_HL , RTD_GLO_COL , RTD_GLO_OTHERS) , RTD_RL_LARL_OO , RTD_RL_CHE_1 ) * lightColor.rgb, RTD_ST) , RTD_SS );


					//RT_R
					float3 RTD_R = RT_R( input.uv, viewDirection, normalDirection, input.positionWS.xyz, EnvRef, RTD_TEX_COL, RTD_R_OFF_OTHERS );

					//RT_SL
					float3 RTD_SL_CHE_1 = (float3)1.0;
					float3 RTD_SL = RT_SL( input.uv, input.positionWS.xyz, normalDirection, (float3)0.0, RTD_TEX_COL, RTD_R, RTD_SL_CHE_1 );

					//RT_RL_SUB1
					float3 RTD_RL = RT_RL_SUB1( RTD_SL_CHE_1 , RTD_RL_LARL_OO , RTD_RL_MAIN);

					float3 RTD_CA_OFF_OTHERS = (RTD_RL + RTD_SL);

					float3 area_light_output = RTD_CA_OFF_OTHERS * lightfos;


					#if N_F_USETLB_ON
						Ar_L_O += area_light_output;
					#else
						Ar_L_O = max (area_light_output,Ar_L_O);
					#endif

				}

				Alight = _LightDatas[NPLC+min(++i, last)];

			}
		}
	}

#endif

#endif

	//=========//
	//=========//

	#if N_F_USETLB_ON
		color = main_light_output + A_L_O + Ar_L_O;
	#else
		color = max(max(main_light_output ,A_L_O), Ar_L_O);
	#endif


	#if N_F_TRANS_ON

		float Trans_Val = 1.0;
					
		#ifndef N_F_CO_ON

			Trans_Val = RTD_TRAN_OPA_Sli;

		#endif
					
	#else

		float Trans_Val = 1.0;

	#endif

	#ifdef DEBUG_DISPLAY
		color = RTD_TEX_COL * GetCurrentExposureMultiplier();
	#endif

	//RT_CA
        float3 RTD_CA = RT_CA( color * SSAmOc + GLO_OUT);

//SSOL
//#ifdef UNITY_COLORSPACE_GAMMA//SSOL
//_OutlineColor=float4(LinearToGamma22(_OutlineColor.rgb),_OutlineColor.a);//SSOL
//#endif//SSOL
//#if N_F_O_ON//SSOL
//float3 SSOLi=(float3)EdgDet(sceneUVs.xy,input.positionCS);//SSOL
//#if N_F_O_MOTTSO_ON//SSOL
//float3 Init_FO=(RTD_CA*RTD_SON_CHE_1)*lerp((float3)1.0,_OutlineColor.rgb,SSOLi);//SSOL
//float4 finalRGBA=float4(Init_FO,Trans_Val);//FOP
//#else//SSOL
//float3 Init_FO=lerp((RTD_CA*RTD_SON_CHE_1),_OutlineColor.rgb,SSOLi);//SSOL
//float4 finalRGBA=float4(Init_FO,Trans_Val);//FOP2
//#endif//SSOL
//#else//SSOL
//float4 finalRGBA=float4((RTD_CA*RTD_SON_CHE_1),Trans_Val);//FOP
//#endif//SSOL

float4 finalRGBA=float4(RTD_CA*RTD_SON_CHE_1,Trans_Val);//FOP

	//RT_NFD
	#ifdef N_F_NFD_ON
		RT_NFD(input.positionCS.xy);
	#endif

	#ifdef UNITY_VIRTUAL_TEXTURING
		input.outVTFeedback = builtinData.vtPackedFeedback;
	#endif

	//
	#ifdef _DEPTHOFFSET_ON 
		outputDepth = posInput.deviceDepth;
	#endif
	//
		
    outColor = EL_AT_SC(posInput, viewDirection, finalRGBA);

	//
	#ifdef UNITY_VIRTUAL_TEXTURING
		float vtAlphaValue = builtinData.opacity;
		outVTFeedback = PackVTFeedbackWithAlpha(builtinData.vtPackedFeedback, input.positionSS.xy, finalRGBA);
	#endif
	//
}

//SSOL_NU

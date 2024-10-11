//RealToon HDRP RT - GBuPas
//MJQStudioWorks

#include "Assets/Plugins/RealToon/RealToon Shaders/RealToon Core/HDRP/RT_HDRP_Core.hlsl"
#include "Assets/Plugins/RealToon/RealToon Shaders/RealToon Core/HDRP/RT_HDRP_Lit_Func_RT.hlsl"

[shader("closesthit")]
void ClosestHitGBuffer(inout RayIntersectionGBuffer rayIntersectionGbuffer : SV_RayPayload, AttributeData attributeData : SV_IntersectionAttributes)
{
	UNITY_XR_ASSIGN_VIEW_INDEX(DispatchRaysIndex().z);

	IntersectionVertex currentVertex;
    FragInputs fragInput;
    GetCurrentVertexAndBuildFragInputs(attributeData, currentVertex, fragInput);

	const float3 incidentDir = WorldRayDirection();

	PositionInputs posInput;
	posInput.positionWS = fragInput.positionRWS;
	posInput.positionSS = uint2(0, 0);

	SurfaceData surfaceData;
	BuiltinData builtinData;
	bool isVisible;
	RayCone cone;
	cone.width = 0.0;
	cone.spreadAngle = 0.0;
	GetSurfaceAndBuiltinData(fragInput, -incidentDir, posInput, surfaceData, builtinData, currentVertex, cone, isVisible);

	float SWEXPO = saturate(GetCurrentExposureMultiplier());
		
#ifdef HAS_LIGHTLOOP
	//light cal

	float3 smoNorm = float3(0.0, 0.0, 0.0);

	#if N_F_SON_ON
		float3 ObPos = TransformWorldToObject(fragInput.positionRWS).xyz;
		smoNorm = calcNorm(float3(-ObPos.x + ((-_XYZPosition.x) * 0.1), ObPos.y + ((_XYZPosition.y) * 0.1), (-ObPos.z) + ((-_XYZPosition.z) * 0.1)));
	#endif

	//RT_NM
	float3 normalLocal = RT_NM( fragInput.texCoord0.xy, fragInput.positionRWS.xyz, fragInput.tangentToWorld, surfaceData.normalWS );
	
	float isFrontFace = (fragInput.isFrontFace >= 0 ? 1 : 0 );
	float4 objPos = mul ( GetObjectToWorldMatrix(), float4(0.0,0.0,0.0,1.0) );
	float4 projPos = ComScrPos( TransformWorldToHClip( WorldRayOrigin() ) );
	float2 sceneUVs = (projPos.xy / projPos.w);

    float3 viewDirection = -incidentDir;
    float3 normalDirection = SafeNormalize(mul( normalLocal, fragInput.tangentToWorld ));
    float3 viewReflectDirection = reflect( -viewDirection, normalDirection );

	float2 RTD_OB_VP_CAL = distance(objPos.xyz, GetCurrentViewPosition());
	float2 RTD_VD_Cal = -(float2((sceneUVs.x * 2.0 - 1.0)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2.0 - 1.0).xy * RTD_OB_VP_CAL );

	float2 RTD_TC_TP_OO = lerp( fragInput.texCoord0.xy, RTD_VD_Cal, _TexturePatternStyle );

	float3 color = float3(0.0,0.0,0.0);
	float3 A_L_O = float3(0.0,0.0,0.0);
	float3 Ar_L_O = float3(0.0,0.0,0.0);

	//=========//
	//=========//

	#ifdef USE_LIGHT_CLUSTER
		float3 actualWSPos = posInput.positionWS;
	#endif

	LightLoopContext context;
	context.shadowContext  = InitShadowContext();
	context.shadowValue = 1;			
	context.sampleReflection = 0;
	//context.splineVisibility = -1;
#ifdef APPLY_FOG_ON_SKY_REFLECTIONS
	context.positionWS = posInput.positionWS;
#endif
	
	uint i=0;
	uint cellIndex;

	InitContactShadow(posInput, context);

	#ifdef SHADOWS_SHADOWMASK
		float4 shaMask = SampleShadowMask(posInput.positionWS, fragInput.texCoord0.xy.xy);
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
	float3 EnvRef = RT_REF_ENV_RT(builtinData, normalDirection, viewReflectDirection, posInput, context, float3(0.0,0.0,0.0) , true);

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

		for (i = 0; i < _DirectionalLightCount; ++i)
		{

			DirectionalLightData light = _DirectionalLightDatas[i];

										
			if (IsMatchingLightLayer(light.lightLayers, builtinData.renderingLayers))
			{
				//RT_DL_RT
				RT_DL_RT( context, light , DirLigCol , DirLigDir , DirSha , DirLigDim , DirDifDim , DirSpecDim , posInput );
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

	float RTD_LVLC = RTD_LVLC_F(lightColor.rgb * 0.2);
	float3 lig_col_int = (_LightIntensity * lightColor.rgb);
	
	
	#ifdef N_F_TP_ON
		float4 _MainTex_var = RT_Tripl_Default(_MainTex, sampler_MainTex, fragInput.positionRWS.xyz, normalDirection);
	#else
		float4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex , TRANSFORM_TEX(RTD_TC_TP_OO, _MainTex) );
	#endif
	
	
	float3 _RTD_MVCOL = lerp(1, fragInput.color.rgb, _MVCOL);


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_OverallShadowColor = float4(LinearToGamma22(_OverallShadowColor.rgb), _OverallShadowColor.a);
	#endif

	float3 RTD_OSC = (_OverallShadowColor.rgb*_OverallShadowColorPower);
	//


	//RT_MCAP
	float3 MCapOutP = RT_MCAP( fragInput.texCoord0.xy , normalDirection );


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_MainColor = float4(LinearToGamma22(_MainColor.rgb),_MainColor.a);
	#endif

	//RT_MCAP_SUB1
	float3 RTD_TEX_COL;
	float3 RTD_MCIALO_IL = RT_MCAP_SUB1( MCapOutP , _MainTex_var, _RTD_MVCOL , RTD_TEX_COL);
	//


	//RT_TRANS_CO
	float RTD_TRAN_OPA_Sli;
	bool bo_co_val;
	float RTD_CO;
	float3 GLO_OUT = (float3)0.0;
	RT_TRANS_CO( fragInput.texCoord0.xy, _MainTex_var, RTD_TRAN_OPA_Sli, RTD_CO, bo_co_val, true, fragInput.positionRWS.xyz, normalDirection, fragInput.positionSS.xy, GLO_OUT);


	//RT_SON
	float3 RTD_SON_CHE_1;
	float3 RTD_SON = RT_SON( fragInput.color , smoNorm , normalDirection , RTD_SON_CHE_1);

	//RT_RELGI
	float3 RTD_GI_FS_OO = RT_RELGI( RTD_SON );

	//RT_SCT
	float3 RTD_SCT = RT_SCT( fragInput.texCoord0.xy, fragInput.positionRWS.xyz, normalDirection, RTD_MCIALO_IL );

	//RT_PT
	float3 RTD_PT_COL;
	float RTD_PT = RT_PT( RTD_VD_Cal , RTD_PT_COL );


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_SelfShadowRealTimeShadowColor = float4(LinearToGamma22(_SelfShadowRealTimeShadowColor.rgb), _SelfShadowRealTimeShadowColor.a);
	#endif

	float3 ss_col = lerp( RTD_PT_COL, (_SelfShadowRealTimeShadowColor.rgb * _SelfShadowRealTimeShadowColorPower) * RTD_OSC * RTD_SCT, RTD_PT);
	//


	float3 RTD_LAS = lerp((ss_col * lerp(GetInverseCurrentExposureMultiplier(), 7.0, SWEXPO)) * RTD_LVLC, (ss_col * lig_col_int), _LightAffectShadow);


	//
	#ifdef UNITY_COLORSPACE_GAMMA
		_HighlightColor = float4(LinearToGamma22(_HighlightColor.rgb), _HighlightColor.a);
	#endif

	float3 RTD_HL = (_HighlightColor.rgb*_HighlightColorPower+_DirectionalLightIntensity);
	//


	float3 RTD_MCIALO = lerp(RTD_TEX_COL , lerp( lerp( (RTD_TEX_COL * (_MainColor.rgb * _MaiColPo)), (RTD_TEX_COL + (_MainColor.rgb * _MaiColPo)), _SPECMODE) , _MainTex_var.rgb * MCapOutP * _RTD_MVCOL * 0.7 , clamp((RTD_LVLC*1.0),0.0,1.0) ) , _MCIALO );


	//RT_GLO
	float RTD_GLO; 
	float3 RTD_GLO_COL;
	RT_GLO( fragInput.texCoord0.xy, RTD_VD_Cal, halfDirection, normalDirection, viewDirection, fragInput.positionRWS.xyz, DirSpecDim, RTD_GLO, RTD_GLO_COL);
	float3 RTD_GLO_OTHERS = RTD_GLO;

	//RT_RL
	float3 RTD_RL_LARL_OO;
	float RTD_RL_MAIN;
	float RTD_RL_CHE_1 = RT_RL( viewDirection , normalDirection , lightColor , RTD_RL_LARL_OO , RTD_RL_MAIN );

	//RT_CLD
	float3 RTD_CLD = RT_CLD( lightDirection );

	float3 RTD_ST_SS_AVD_OO = lerp( RTD_CLD, viewDirection, _SelfShadowShadowTAtViewDirection );
	float RTD_NDOTL = 0.5*dot(RTD_ST_SS_AVD_OO, float3(RTD_SON.x, RTD_SON.y * (1 - _LigIgnoYNorDir), RTD_SON.z))+0.5;

	//RT_ST
	float3 RTD_SHAT_COL;
	float RTD_STIAL;
	float RTD_ST_IS;
	float3 RTD_ST_LAF;
	float RTD_ST = RT_ST ( fragInput.texCoord0.xy, fragInput.positionRWS.xyz, normalDirection, RTD_NDOTL, attenuation, RTD_LVLC, RTD_PT_COL, lig_col_int, RTD_SCT, RTD_OSC, RTD_PT, RTD_SHAT_COL, RTD_STIAL, RTD_ST_IS, RTD_ST_LAF );

	//RT_RELGI_SUB1
	float ref_int_val;
	float3 RTD_SL_OFF_OTHERS = RT_RELGI_SUB1( posInput, viewReflectDirection, viewDirection, RTD_GI_FS_OO , RTD_SHAT_COL , RTD_MCIALO , RTD_STIAL , RTD_RT_GI_Sha_FO , ref_int_val , (float3)0.0 , true);

	//RT_SS
	float RTD_SS = RT_SS( fragInput.color , RTD_NDOTL , attenuation , DirLigDim );
				

	float3 RTD_R_OFF_OTHERS = lerp( lerp( RTD_ST_LAF, RTD_LAS, RTD_ST_IS) * RTD_RT_GI_Sha_FO , lerp( RTD_ST_LAF, lerp( lerp( RTD_MCIALO_IL * RTD_HL , RTD_GLO_COL , RTD_GLO_OTHERS) , RTD_RL_LARL_OO , RTD_RL_CHE_1 ) * lightColor.rgb, RTD_ST) , RTD_SS ) ; //All mixed by lerp


	//RT_R
	float3 RTD_R = RT_R( fragInput.texCoord0.xy, viewDirection, normalDirection, fragInput.positionRWS.xyz, EnvRef, RTD_TEX_COL, RTD_R_OFF_OTHERS );

	//RT_SL
	float3 RTD_SL_CHE_1;
	float3 RTD_SL = RT_SL( fragInput.texCoord0.xy, fragInput.positionRWS.xyz, normalDirection, RTD_SL_OFF_OTHERS, RTD_TEX_COL, RTD_R, RTD_SL_CHE_1 );

	//RT_RL_SUB1
	float3 RTD_RL = RT_RL_SUB1( RTD_SL_CHE_1 , RTD_RL_LARL_OO , RTD_RL_MAIN);

	float3 RTD_CA_OFF_OTHERS = (RTD_RL + RTD_SL);

	float3 main_light_output = RTD_CA_OFF_OTHERS;
        
	//=========//
	//=========//

#if N_F_PAL_ON

	if (LIGHTFEATUREFLAGS_PUNCTUAL)
	{

		float4 PunLigCol = float4(1.0,1.0,1.0,1.0);
		float3 PuncLigDir = float3(0.0,0.0,0.0);
		float PuncSha = 1.0;
		float PuncLF = 1.0;

		uint lightStart = 0, lightEnd = 0;

		#ifdef USE_LIGHT_CLUSTER
			GetLightCountAndStartCluster(actualWSPos, LIGHTCATEGORY_PUNCTUAL, lightStart, lightEnd, cellIndex);
		#else
			lightStart = 0;
			lightEnd = _PunctualLightCountRT;
		#endif

		for (i = lightStart; i < lightEnd; ++i)
		{

			#ifdef USE_LIGHT_CLUSTER
				LightData Plight = FetchClusterLightIndex(cellIndex, i);
			#else
				LightData Plight = _LightDatas[i];
			#endif

			if (IsMatchingLightLayer(Plight.lightLayers, builtinData.renderingLayers))
			{
						
				//RT_PL_RT
				RT_PL_RT( context , Plight , PunLigCol , PuncLigDir , PuncSha , PuncLF , posInput );

				#if N_F_NLASOBF_ON
					float3 lightColor = lerp((float3)0.0,PunLigCol.rgb * Plight.diffuseDimmer,isFrontFace);
				#else
					float3 lightColor = PunLigCol.rgb * Plight.diffuseDimmer;
				#endif

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

				float3 lightDirection = PuncLigDir;
				float3 halfDirection = normalize(viewDirection+lightDirection);

				float RTD_LVLC = RTD_LVLC_F(lightColor.rgb * 0.2);

				float lightfos = smoothstep(0.0, _LightFalloffSoftness ,PuncLF);

				float3 lig_col_int = (_LightIntensity * lightColor.rgb);

				float3 RTD_LAS = lerp((ss_col * lerp(GetInverseCurrentExposureMultiplier(), 7.0, SWEXPO)) * RTD_LVLC, (ss_col * lig_col_int), _LightAffectShadow);
				float3 RTD_HL = (_HighlightColor.rgb*_HighlightColorPower+_PointSpotlightIntensity);

				float3 RTD_MCIALO = lerp(RTD_TEX_COL , lerp( lerp( (RTD_TEX_COL * (_MainColor.rgb * _MaiColPo)), (RTD_TEX_COL + (_MainColor.rgb * _MaiColPo)), _SPECMODE) , _MainTex_var.rgb * MCapOutP * _RTD_MVCOL * 0.7 , clamp((RTD_LVLC*1.0),0.0,1.0) ) , _MCIALO );

				//RT_GLO
				float RTD_GLO; 
				float3 RTD_GLO_COL;
				RT_GLO( fragInput.texCoord0.xy, RTD_VD_Cal, halfDirection, normalDirection, viewDirection, fragInput.positionRWS.xyz, Plight.specularDimmer, RTD_GLO, RTD_GLO_COL );
				float3 RTD_GLO_OTHERS = RTD_GLO;

				//RT_RL
				float3 RTD_RL_LARL_OO;
				float RTD_RL_MAIN;
				float RTD_RL_CHE_1 = RT_RL( viewDirection , normalDirection , lightColor , RTD_RL_LARL_OO , RTD_RL_MAIN );

				//RT_CLD
				float3 RTD_CLD = RT_CLD( lightDirection );

				float3 RTD_ST_SS_AVD_OO = lerp( RTD_CLD, viewDirection, _SelfShadowShadowTAtViewDirection );
				float RTD_NDOTL = 0.5*dot(RTD_ST_SS_AVD_OO, float3(RTD_SON.x, RTD_SON.y * (1 - _LigIgnoYNorDir), RTD_SON.z))+0.5;

				//RT_ST
				float3 RTD_SHAT_COL;
				float RTD_STIAL;
				float RTD_ST_IS;
				float3 RTD_ST_LAF;
				float RTD_ST = RT_ST ( fragInput.texCoord0.xy, fragInput.positionRWS.xyz, normalDirection, RTD_NDOTL, lightfos, RTD_LVLC, RTD_PT_COL, lig_col_int, RTD_SCT, RTD_OSC, RTD_PT, RTD_SHAT_COL, RTD_STIAL, RTD_ST_IS, RTD_ST_LAF );

				//RT_SS
				float RTD_SS = RT_SS( fragInput.color , RTD_NDOTL , attenuation , Plight.shadowDimmer );


				float3 RTD_R_OFF_OTHERS = lerp( lerp( RTD_ST_LAF, RTD_LAS, RTD_ST_IS) * RTD_RT_GI_Sha_FO, lerp( RTD_ST_LAF, lerp( lerp( RTD_MCIALO_IL * RTD_HL , RTD_GLO_COL , RTD_GLO_OTHERS) , RTD_RL_LARL_OO , RTD_RL_CHE_1 ) * lightColor.rgb, RTD_ST) , RTD_SS );


				//RT_R
				float3 RTD_R = RT_R( fragInput.texCoord0.xy, viewDirection, normalDirection, fragInput.positionRWS.xyz, EnvRef, RTD_TEX_COL, RTD_R_OFF_OTHERS );
					
				//RT_SL
				float3 RTD_SL_CHE_1;
				float3 RTD_SL = RT_SL( fragInput.texCoord0.xy, fragInput.positionRWS.xyz, normalDirection, (float3)0.0, RTD_TEX_COL, RTD_R, RTD_SL_CHE_1 );

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
			float3 ALCk = float3(0.0,0.0,0.0);

			uint NPLC = 0;
			uint NALC = 0;
					
			#ifdef USE_LIGHT_CLUSTER
				GetLightCountAndStartCluster(actualWSPos, LIGHTCATEGORY_AREA, NPLC, NALC, cellIndex);
			#else
				NPLC = _PunctualLightCountRT;
				NALC = _AreaLightCountRT;
			#endif

			if (NALC > 0)
			{

				i=0;

				uint last = NALC - 1;

				#ifdef USE_LIGHT_CLUSTER
					LightData Alight = FetchClusterLightIndex(cellIndex, NPLC+i);
				#else
					LightData Alight = _LightDatas[NPLC+i];
				#endif

				while (i <= last )
				{

					if(IsMatchingLightLayer(Alight.lightLayers, builtinData.renderingLayers))
					{
						//RT_AL_RT
						RT_AL_RT( context , Alight , posInput , viewDirection , normalDirection , objPos , ALigCol , ALigDir , ASha , ALF , ALF2 , ALCk );

						#if N_F_NLASOBF_ON
							float3 lightColor = lerp((float3)0.0,ALigCol.rgb * Alight.diffuseDimmer ,isFrontFace);
						#else
							float3 lightColor = ALigCol.rgb * Alight.diffuseDimmer;
						#endif

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

						float3 lightDirection = ALigDir;
						float3 halfDirection = normalize(viewDirection+lightDirection);

						float RTD_LVLC = RTD_LVLC_F(lightColor.rgb * 0.2);

						float lightfos = smoothstep(0.0, _LightFalloffSoftness ,ALF * ALF2); 


						float3 lig_col_int = (_LightIntensity * lightColor.rgb);

						float3 RTD_LAS = lerp((ss_col * lerp(GetInverseCurrentExposureMultiplier(), 7.0, SWEXPO)) * RTD_LVLC, (ss_col * lig_col_int), _LightAffectShadow);
						float3 RTD_HL = (_HighlightColor.rgb*_HighlightColorPower+_ALIntensity);

						float3 RTD_MCIALO = lerp(RTD_TEX_COL , lerp( lerp( (RTD_TEX_COL * (_MainColor.rgb * _MaiColPo)), (RTD_TEX_COL + (_MainColor.rgb * _MaiColPo)), _SPECMODE) , _MainTex_var.rgb * MCapOutP * _RTD_MVCOL * 0.7 , clamp((RTD_LVLC*1.0),0.0,1.0) ) , _MCIALO );

						//RT_GLO
						float RTD_GLO; 
						float3 RTD_GLO_COL;
						RT_GLO( fragInput.texCoord0.xy, RTD_VD_Cal, halfDirection, normalDirection, viewDirection, fragInput.positionRWS.xyz, Alight.specularDimmer, RTD_GLO, RTD_GLO_COL );

						float RTD_GLO_OTHERS = RTD_GLO;

						//RT_RL
						float3 RTD_RL_LARL_OO;
						float RTD_RL_MAIN;
						float RTD_RL_CHE_1 = RT_RL( viewDirection , normalDirection , lightColor , RTD_RL_LARL_OO , RTD_RL_MAIN );

						//RT_CLD
						float3 RTD_CLD = RT_CLD( lightDirection );

						float3 RTD_ST_SS_AVD_OO = lerp( RTD_CLD, viewDirection, _SelfShadowShadowTAtViewDirection );
						float RTD_NDOTL = 0.5*dot(RTD_ST_SS_AVD_OO, float3(RTD_SON.x, RTD_SON.y * (1 - _LigIgnoYNorDir), RTD_SON.z))+0.5;

						//RT_ST
						float3 RTD_SHAT_COL;
						float RTD_STIAL;
						float RTD_ST_IS;
						float3 RTD_ST_LAF;
						float RTD_ST = RT_ST ( fragInput.texCoord0.xy, fragInput.positionRWS.xyz, normalDirection, RTD_NDOTL, lightfos, RTD_LVLC, RTD_PT_COL, lig_col_int, RTD_SCT, RTD_OSC, RTD_PT, RTD_SHAT_COL, RTD_STIAL, RTD_ST_IS, RTD_ST_LAF );

						//RT_SS
						float RTD_SS = RT_SS( fragInput.color , RTD_NDOTL , attenuation , Alight.shadowDimmer );


						float3 RTD_R_OFF_OTHERS = lerp( lerp( RTD_ST_LAF, RTD_LAS, RTD_ST_IS) * RTD_RT_GI_Sha_FO , lerp( RTD_ST_LAF, lerp( lerp( RTD_MCIALO_IL * RTD_HL , RTD_GLO_COL , RTD_GLO_OTHERS) , RTD_RL_LARL_OO , RTD_RL_CHE_1 ) * lightColor.rgb, RTD_ST) , RTD_SS );

						//RT_R
						float3 RTD_R = RT_R( fragInput.texCoord0.xy, viewDirection, normalDirection, fragInput.positionRWS.xyz, EnvRef, RTD_TEX_COL, RTD_R_OFF_OTHERS );

						//RT_SL
						float3 RTD_SL_CHE_1;
						float3 RTD_SL = RT_SL( fragInput.texCoord0.xy, fragInput.positionRWS.xyz, normalDirection, (float3)0.0, RTD_TEX_COL, RTD_R, RTD_SL_CHE_1 );

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

					#ifdef USE_LIGHT_CLUSTER
						Alight = FetchClusterLightIndex(cellIndex, NPLC+(min(++i, last)));
					#else
						Alight = _LightDatas[NPLC+(min(++i, last))];
					#endif

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

	//RT_CA
	float3 RTD_CA = RT_CA(color + GLO_OUT);

	float4 finalRGBA = ApplyBlendMode(RTD_CA * RTD_SON_CHE_1, Trans_Val);

	//===================================================================

	rayIntersectionGbuffer.gbuffer0 = 0.0; //float4(finalRGBA.rgb,1.0); //Futher Checking

	NormalData normalData;
	normalData.normalWS = normalDirection;
	normalData.perceptualRoughness = 1.0;
	EncIntNormBu(normalData, uint2(0, 0), rayIntersectionGbuffer.gbuffer1);

	rayIntersectionGbuffer.gbuffer2 = float4(0.0,0.0,0.0,0.0);

	rayIntersectionGbuffer.gbuffer3 = float4(finalRGBA.rgb, 1.0) * GetCurrentExposureMultiplier();

	#ifdef LIGHT_LAYERS
		OUT_GBUFFER_LIGHT_LAYERS = float4(0.0, 0.0, 0.0, standardBSDFData.renderingLayers / 255.0);
	#endif

	//===================================================================

	#if N_F_SL_ON

		rayIntersectionGbuffer.t = -1; //-RayTCurrent()

	#else

		rayIntersectionGbuffer.t = RayTCurrent();

	#endif

	//===================================================================

#else

	rayIntersectionGbuffer.gbuffer3 = float4(1.0, 1.0, 1.0, 1.0);

#endif

}

[shader("anyhit")]
void AnyHitGBuffer(inout RayIntersectionGBuffer rayIntersectionGbuffer : SV_RayPayload, AttributeData attributeData : SV_IntersectionAttributes)
{
	#ifndef N_F_TRANS_ON
		IgnoreHit();
	#else
	
		UNITY_XR_ASSIGN_VIEW_INDEX(DispatchRaysIndex().z);

		IntersectionVertex currentVertex;
		FragInputs fragInput;
		GetCurrentVertexAndBuildFragInputs(attributeData, currentVertex, fragInput);

		const float3 incidentDir = WorldRayDirection();

		PositionInputs posInput;
		posInput.positionWS = fragInput.positionRWS;
		posInput.positionSS = uint2(0, 0);

		SurfaceData surfaceData;
		BuiltinData builtinData;
		bool isVisible;
		RayCone cone;
		cone.width = 0.0;
		cone.spreadAngle = 0.0;
		GetSurfaceAndBuiltinData(fragInput, -incidentDir, posInput, surfaceData, builtinData, currentVertex, cone, isVisible);

		float4 objPos = mul ( GetObjectToWorldMatrix(), float4(0.0,0.0,0.0,1.0) );
		float4 projPos = ComScrPos( TransformWorldToHClip( WorldRayOrigin() ) );
		float2 sceneUVs = (projPos.xy / projPos.w);

		float2 RTD_OB_VP_CAL = distance(objPos.xyz, GetCurrentViewPosition());
		float2 RTD_VD_Cal = -(float2((sceneUVs.x * 2.0 - 1.0)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2.0 - 1.0).xy * RTD_OB_VP_CAL );

		float2 RTD_TC_TP_OO = lerp( fragInput.texCoord0.xy, RTD_VD_Cal, _TexturePatternStyle );
	
		#ifdef N_F_TP_ON
			float4 _MainTex_var = RT_Tripl_Default(_MainTex, sampler_MainTex, fragInput.positionRWS.xyz, SafeNormalize( mul( float3(0.0,0.0,1.0), fragInput.tangentToWorld) ));
		#else
			float4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex , TRANSFORM_TEX(RTD_TC_TP_OO, _MainTex) );
		#endif

		//RT_TRANS_CO
		float RTD_TRAN_OPA_Sli;
		bool bo_co_val;
		float RTD_CO;	
		float3 GLO_OUT = (float3)0.0;
		RT_TRANS_CO(fragInput.texCoord0.xy, _MainTex_var, RTD_TRAN_OPA_Sli, RTD_CO, bo_co_val, true, fragInput.positionRWS.xyz, SafeNormalize( mul( float3(0.0,0.0,1.0), fragInput.tangentToWorld) ), fragInput.positionSS.xy, GLO_OUT);

		if(!bo_co_val)
		{
			IgnoreHit();
		}

#endif
}
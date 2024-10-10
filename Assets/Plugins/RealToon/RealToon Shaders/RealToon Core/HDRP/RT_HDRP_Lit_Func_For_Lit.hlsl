//RealToon HDRP - LIT_FUNC_FOR_LIT
//MJQStudioWorks

//=========================
//=========================

#ifndef REALTOON_HDRP_FOR_LIT_HLSL
#define REALTOON_HDRP_FOR_LIT_HLSL

//RT REF ENV
float3 RT_REF_ENV( BuiltinData builtinData , float3 Normal , float3 viewReflectDirection , PositionInputs posInput , LightLoopContext context , in float3 reflected , bool isRT)
{

	#if N_F_R_ON

		float3 EnvRef = float3(0.0, 0.0, 0.0);
		float4 RefEn = float4(0.0, 0.0, 0.0, 0.0); 
		float4 ERef = float4(0.0, 0.0, 0.0, 0.0);
		float4 RefSky = float4(1.0, 1.0, 1.0, 0.0);

		float3 SSROut = float3(1.0, 1.0, 1.0);
		float SSROutA = 0.0; 
		
		float reflectionHierarchyWeight = 0.0; 

		context.sampleReflection = 0;

		if (LIGHTFEATUREFLAGS_ENV )
		{

			float weight = 1.0;
			context.sampleReflection = SINGLE_PASS_CONTEXT_SAMPLE_REFLECTION_PROBES;

			float4 ssrLighting = LOAD_TEXTURE2D_X(_SsrLightingTexture, posInput.positionSS);
			InversePreExposeSsrLighting(ssrLighting);

			ApplyScreenSpaceReflectionWeight(ssrLighting);

			SSROut = ssrLighting.rgb;

			if (_EnableRecursiveRayTracing && _RecurRen == 1.0)
			{
				RefEn += float4(SSROut, 0.0);
			}
			else
			{

				#if N_F_ESSR_ON

					#if (!defined(N_F_TRANS_ON) || defined(N_F_CO_ON)) && !defined(N_F_FR_ON)

						if (_EnableRayTracedReflections)
						{
	
							//if(_Smoothness >= 0.9) //Will be remove once not in use anymore.
							//{
								RefEn += float4(SSROut,0.0);
							//}

						}
						else
						{

							SSROutA = ssrLighting.a;

						}

					#endif

				#endif

			}

			uint envLightStart , envLightCount;

			#ifndef LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
				GetCountAndStart(posInput, LIGHTCATEGORY_ENV, envLightStart, envLightCount);
			#else 
				envLightCount = _EnvLightCount;
				envLightStart = 0;
			#endif

			bool fastPath = false;
			#if SCALARIZE_LIGHT_LOOP
				uint envStartFirstLane;
				fastPath = IsFastPath(envLightStart, envStartFirstLane);
			#endif

			#if SCALARIZE_LIGHT_LOOP
				if (fastPath)
				{
					envLightStart = envStartFirstLane;
				}
			#endif

			uint v_envLightListOffset = 0;
			uint v_envLightIdx = envLightStart;

			while (v_envLightListOffset < envLightCount)
			{


				v_envLightIdx = FetchIndex(envLightStart, v_envLightListOffset);
#if SCALARIZE_LIGHT_LOOP
				uint s_envLightIdx = ScalarizeElementIndex(v_envLightIdx, fastPath);
#else
				uint s_envLightIdx = v_envLightIdx;
#endif
				if (s_envLightIdx == -1)
					break;

				EnvLightData ELD = FetchEnvLight(s_envLightIdx);

				if (s_envLightIdx >= v_envLightIdx)
				{
					v_envLightListOffset++;

					if ( IsMatchingLightLayer(ELD.lightLayers, builtinData.renderingLayers) )
					{
						float3 VED = viewReflectDirection;

						float intersectionDistance = EvaluateLight_EnvIntersection(posInput.positionWS, Normal, ELD, ELD.influenceShapeType, VED, weight);
						ERef = SampleEnv(context, ELD.envIndex, VED, (11.0 - (_Smoothness * 11.0)), ELD.rangeCompressionFactorCompensation, posInput.positionNDC);

						weight *= ERef.a;
						UpdateLightingHierarchyWeights(reflectionHierarchyWeight, weight);

						if (_EnableRecursiveRayTracing && _RecurRen == 1.0)
						{ }
						else
						{
							if (_EnableRayTracedReflections)
							{
	
								#ifdef N_F_ESSR_ON

									#if (!defined(N_F_TRANS_ON) || defined(N_F_CO_ON)) && !defined(N_F_FR_ON)

										//if(_Smoothness < 0.9) //Will be remove once not in use anymore.
										//{
											RefEn += (ERef * weight) * ELD.multiplier;
										//}

									#else

											RefEn += (ERef * weight) * ELD.multiplier;

									#endif

								#else

									RefEn += (ERef * weight) * ELD.multiplier;

								#endif

							}
							else
							{
								
								RefEn += (ERef * weight) * ELD.multiplier;

							}

						}
					}
				}
			}

		}

		if (LIGHTFEATUREFLAGS_SKY && _EnvLightSkyEnabled)
		{
			context.sampleReflection = SINGLE_PASS_CONTEXT_SAMPLE_SKY;
			EnvLightData ELS = InitSkyEnvLightData(0);

			float weight = 1.0;
			float3 VED = viewReflectDirection;

			float intersectionDistance = EvaluateLight_EnvIntersection(posInput.positionWS, Normal, ELS, ELS.influenceShapeType, VED, weight);
			RefSky = SampleEnv(context, ELS.envIndex, VED, (11.0 - (_Smoothness * 11.0)), ELS.rangeCompressionFactorCompensation, posInput.positionNDC);

			weight *= RefSky.a;
			UpdateLightingHierarchyWeights(reflectionHierarchyWeight, weight);

			if (_EnableRecursiveRayTracing && _RecurRen == 1.0)
			{ 
				if (_Smoothness <= 1.0)
				{
					EnvRef += RefSky.rgb * weight;
				}

			}
			else
			{
				if (!_EnableRayTracedReflections)
				{
					RefEn += RefSky * weight;
				}
				else
				{
					#ifdef N_F_ESSR_ON

						#if (!defined(N_F_TRANS_ON) || defined(N_F_CO_ON)) && !defined(N_F_FR_ON)

							//if (_Smoothness < 0.9)
							//{
								//RefEn += RefSky * weight;
							//}

						#else

							RefEn += RefSky * weight;

						#endif

					#else
							RefEn += RefSky * weight;
					#endif
				}

			}

		}


		if (_EnableRecursiveRayTracing && _RecurRen == 1.0)
		{ 
		
			EnvRef = SSROut.rgb * GetCurrentExposureMultiplier();
		
		}
		else
		{ 

			if (!_EnableRayTracedReflections)
			{
				EnvRef = lerp(SSROut.rgb * GetCurrentExposureMultiplier(), RefEn.rgb * GetCurrentExposureMultiplier(), (1.0 - SSROutA));
			}
			else
			{
				#ifdef N_F_ESSR_ON
					EnvRef = RefEn.rgb * GetCurrentExposureMultiplier();
				#else
					EnvRef = lerp(SSROut.rgb * GetCurrentExposureMultiplier(), RefEn.rgb * GetCurrentExposureMultiplier(), (1.0 - SSROutA));
				#endif
			}

		}

		return EnvRef;

	#else

		return float3(0.0,0.0,0.0);

	#endif

}

//RT_DL
void RT_DL( LightLoopContext context , DirectionalLightData light , out float4 DirLigCol , out float3 DirLigDir , out float DirSha , out float DirLigDim , out float DirDifDim , out float DirSpecDim , PositionInputs posInput , BuiltinData builtinData )
{

	DirLigDir = -light.forward;
	DirLigDim = light.shadowDimmer;
	DirDifDim = light.diffuseDimmer;
	DirSpecDim = light.specularDimmer;

	DirSha = 1.0;
	DirLigCol = float4(light.color * GetCurrentExposureMultiplier(), 1.0);

	#ifndef LIGHT_EVALUATION_NO_HEIGHT_FOG
						
		{
							
			float  cosZenithAngle = DirLigDir.y;
			float  fragmentHeight = posInput.positionWS.y;
			float3 oDepth = OpticalDepthHeightFog(_HeightFogBaseExtinction, _HeightFogBaseHeight,
													_HeightFogExponents, cosZenithAngle, fragmentHeight);
							
			float3 transm = TransmittanceFromOpticalDepth(oDepth);
			DirLigCol.rgb *= transm;
		}
	#endif

	#if SHADEROPTIONS_PRECOMPUTED_ATMOSPHERIC_ATTENUATION

	#else
						
		bool interactsWithSky = asint(light.distanceFromCamera) >= 0;

		if (interactsWithSky)
		{

			float3 X = GetAbsolutePositionWS(posInput.positionWS);
			float3 C = (float3)_PlanetCenterPosition;

			float r        = distance(X, C);
			float cosHoriz = ComputeCosineOfHorizonAngle(r);
			float cosTheta = dot(X - C, DirLigDir) * rcp(r); 

			if (cosTheta >= cosHoriz) 
			{
				float3 oDepth = ComputeAtmosphericOpticalDepth(r, cosTheta, true);
								
				float3 transm  = TransmittanceFromOpticalDepth(oDepth);
				float3 opacity = 1.0 - transm;
				DirLigCol.rgb *= 1.0 - (Desaturate(opacity, _AlphaSaturation) * _AlphaMultiplier);
			}
			else
			{
								
				DirLigCol = float4(0.0,0.0,0.0,0.0);
			}
		}

	#endif

	#ifndef LIGHT_EVALUATION_NO_COOKIE
		if (light.cookieMode != COOKIEMODE_NONE)
		{
			float3 lightToSample = posInput.positionWS - light.positionRWS;
			float3 cookie = EvaluateCookie_Directional(context, light, lightToSample);

			DirLigCol.rgb *= cookie;
		}
	#endif

		#if	N_F_ESSS_ON
								
			#if !defined(N_F_TRANS_ON) || defined(N_F_CO_ON)

					if ( (light.screenSpaceShadowIndex & SCREEN_SPACE_SHADOW_INDEX_MASK) != INVALID_SCREEN_SPACE_SHADOW )
					{	
						DirSha = lerp(1, (float)GetScreenSpaceColorShadow(posInput, light.screenSpaceShadowIndex) , light.shadowDimmer);
					}

					else

			#endif

		#endif

					{

		#ifndef LIGHT_EVALUATION_NO_SHADOWS

			float shadowMask = 1.0;

			float NdotL = 1.0;

			#ifdef SHADOWS_SHADOWMASK

				DirSha = shadowMask = (light.shadowMaskSelector.x >= 0.0 && NdotL > 0.0) ? dot( BUILTIN_DATA_SHADOW_MASK , light.shadowMaskSelector) : 1.0;

			#endif

			if ((light.shadowIndex >= 0) && (light.shadowDimmer > 0))
			{
				context.shadowValue = GetDirectionalShadowAttenuation(context.shadowContext, posInput.positionSS, posInput.positionWS, (float3)0.0 , light.shadowIndex, DirLigDir);
				DirSha = (float)context.shadowValue;

				#ifdef SHADOWS_SHADOWMASK

					uint  payloadOffset;
					real  fade;
					int cascadeCount;
					int shadowSplitIndex = 0;

					shadowSplitIndex = EvalShadow_GetSplitIndex(context.shadowContext, light.shadowIndex, posInput.positionWS, fade, cascadeCount);

					fade = ((shadowSplitIndex + 1) == cascadeCount) ? fade : saturate(-shadowSplitIndex);

					DirSha = DirSha - fade + fade * shadowMask;

					DirSha = light.nonLightMappedOnly ? min(shadowMask, DirSha) : DirSha;

				#endif

				DirSha = lerp(shadowMask, DirSha, light.shadowDimmer);
			}

			#ifndef N_F_CS_ON

				#if !defined(N_F_TRANS_ON) && !defined(LIGHT_EVALUATION_NO_CONTACT_SHADOWS)
					DirSha = min(DirSha, NdotL > 0.0 ? GetContactShadow(context, light.contactShadowMask, light.isRayTracedContactShadow) : 1.0); //float NdotL = dot(normalDirection, DirLigDir);
				#endif

			#endif

		#else 

			DirSha = 1.0;

		#endif
	}

}

//RT_PL
void RT_PL( LightLoopContext context , LightData Plight , float3 input , out float4 PunLigCol , out float3 PuncLigDir , out float PuncSha , out float PuncLF , PositionInputs posInput , BuiltinData builtinData )
{

	PunLigCol = float4(1.0,1.0,1.0,1.0);
	PuncLigDir = float3(0.0,0.0,0.0);
	PuncSha = 1.0;
	PuncLF = 1.0;

	float4 distances;

	float3 lightToSample = input - Plight.positionRWS;

	distances.w = dot(lightToSample, Plight.forward);

	if (Plight.lightType == GPULIGHTTYPE_PROJECTOR_BOX)
	{
		PuncLigDir = -Plight.forward;
		distances.xyz = float3(1.0,1.0,1.0); 
	}
	else
	{
		float3 unL     = -lightToSample;
		float  distSq  = dot(unL, unL);
		float  distRcp = rsqrt(distSq);
		float  dist    = distSq * distRcp;

		PuncLigDir = unL * distRcp;
		distances.xyz = float3(dist, distSq, distRcp);

		ModifyDistancesForFillLighting(distances, Plight.size.x);
	}

//=========================

PunLigCol = float4(Plight.color * GetCurrentExposureMultiplier(), 1.0) ;

PuncLF = PunctualLightAttenuation(distances, Plight.rangeAttenuationScale , Plight.rangeAttenuationBias, Plight.angleScale, Plight.angleOffset);

#ifndef LIGHT_EVALUATION_NO_HEIGHT_FOG

	{
		float cosZenithAngle = PuncLigDir.y;
		float distToLight = (Plight.lightType == GPULIGHTTYPE_PROJECTOR_BOX) ? distances.w : distances.x;
		float fragmentHeight = posInput.positionWS.y;
		PunLigCol.a *= TransmittanceHeightFog(_HeightFogBaseExtinction, _HeightFogBaseHeight,
											_HeightFogExponents, cosZenithAngle,
										fragmentHeight, distToLight);
	}
#endif

	if (Plight.cookieMode != COOKIEMODE_NONE)
	{
		float3 lightToSample = posInput.positionWS - Plight.positionRWS;
		float4 cookie = EvaluateCookie_Punctual(context, Plight, lightToSample);
                    
		PunLigCol *= cookie * cookie.a;
	}
//=============================

#ifndef LIGHT_EVALUATION_NO_SHADOWS

	float shadowMask = 1.0;

	//Not Really Needed
	float NdotL = 1.0;
						
	#ifdef SHADOWS_SHADOWMASK

		PuncSha = shadowMask = (Plight.shadowMaskSelector.x >= 0.0 && NdotL > 0.0) ? dot(BUILTIN_DATA_SHADOW_MASK, Plight.shadowMaskSelector) : 1.0;

	#endif

		#if	N_F_ESSS_ON

			#if !defined(N_F_TRANS_ON) || defined(N_F_CO_ON) // defined(SCREEN_SPACE_SHADOWS) &&  , && (SHADERPASS != SHADERPASS_VOLUMETRIC_LIGHTING)

				if( ( Plight.screenSpaceShadowIndex & SCREEN_SPACE_SHADOW_INDEX_MASK) != INVALID_SCREEN_SPACE_SHADOW )
				{
					PuncSha = GetScreenSpaceShadow(posInput, Plight.screenSpaceShadowIndex);
				}

				else

			#endif

		#endif

		{
			if ((Plight.shadowDimmer > 0)) //(Plight.shadowIndex >= 0) && 
			{
				PuncSha = GetPunctualShadowAttenuation(context.shadowContext, posInput.positionSS, posInput.positionWS, 0 , Plight.shadowIndex, PuncLigDir, distances.x, Plight.lightType == GPULIGHTTYPE_POINT, Plight.lightType != GPULIGHTTYPE_PROJECTOR_BOX);
					
				#ifdef SHADOWS_SHADOWMASK
	
					PuncSha = Plight.nonLightMappedOnly ? min(shadowMask, PuncSha) : PuncSha;

				#endif

				PuncSha = lerp(shadowMask, PuncSha, Plight.shadowDimmer);
			}

			#ifndef N_F_CS_ON
				#if !defined(N_F_TRANS_ON) && !defined(LIGHT_EVALUATION_NO_CONTACT_SHADOWS)
					PuncSha = min(PuncSha, 1 > 0.0 ? GetContactShadow(context, Plight.contactShadowMask, Plight.isRayTracedContactShadow) : 1.0); //float NdotL = dot(normalDirection, PuncLigDir);
				#endif
			#endif

			PuncSha;

		}

#else 

	PuncSha;

#endif

}

//RT_AL
void RT_AL( LightLoopContext context , LightData Alight , PositionInputs posInput , float3 viewDirection , float3 normalDirection , float3 input , float4 objPos , out float4 ALigCol , out float3 ALigDir , out float ASha , out float ALF , out float ALF2 , out float3 ALCk , BuiltinData builtinData )
{

	ALigCol = float4(1.0,1.0,1.0,1.0);
	ALigDir = float3(0.0,0.0,0.0);
	ASha = 1.0;
	ALF = 1.0;
	ALF2 = 0.0;
	ALCk = float3(1.0,1.0,1.0);

	if (Alight.lightType == GPULIGHTTYPE_TUBE)
	{

		float  len = Alight.size.x;
		float3 T   = Alight.right;

		float3 unL = Alight.positionRWS - posInput.positionWS;
		ALigDir = unL + T;

		float range          = Alight.range;
		float invAspectRatio = saturate(range / (range + (0.5 * len)));
		
        float3x3 orthoBasisViewNormal = GetOrthoBasisViewNormal(viewDirection, normalDirection, dot(normalDirection, viewDirection));

		ALF = EllipsoidalDistanceAttenuation(unL, T, invAspectRatio, Alight.rangeAttenuationScale, Alight.rangeAttenuationBias);

		if (ALF != 0.0)
		{
			Alight.positionRWS -= posInput.positionWS;
										
			#if N_F_ALSL_ON
			    float3 center = mul(orthoBasisViewNormal, unL);
				float3 axis = mul(orthoBasisViewNormal, T);
				ALF2 = 1.0;
			#else
				//Temporary Solution
				float dis_obj_lig = distance( ( input.xyz - objPos.xyz ) , unL ) * 0.08 * (_ALTuFo * 0.01);
				ALF2 = max( (1.0 - dis_obj_lig) , 0.0) * clamp( (0.5 * len) ,0,1);
				//==================
			#endif

			#if N_F_ALSL_ON
				ALigCol = float4(Alight.color * I_ltc_line( transpose(k_identity3x3) , center , axis , Alight.size.x * 0.5 ) *  GetCurrentExposureMultiplier() ,1.0);
			#else
				ALigCol = float4(Alight.color * ALF2 * GetCurrentExposureMultiplier() ,1.0);
			#endif
		}
						
	}
	else if (Alight.lightType == GPULIGHTTYPE_RECTANGLE)
	{

		#if SHADEROPTIONS_BARN_DOOR
			RectangularLightApplyBarnDoor(Alight, posInput.positionWS);
		#endif

		float3 unL = Alight.positionRWS - posInput.positionWS;
		ALigDir = unL;

		if (dot(Alight.forward, unL) < FLT_EPS)
		{
			float3x3 lightToWorld = float3x3(Alight.right, Alight.up, -Alight.forward);
			unL = mul(unL, transpose(lightToWorld));

			float halfWidth  = Alight.size.x * 0.5;
			float halfHeight = Alight.size.y * 0.5;

			float  range      = Alight.range;
			float3 invHalfDim = rcp(float3(range + halfWidth, range + halfHeight, range));

		#ifdef ELLIPSOIDAL_ATTENUATION

			ALF = EllipsoidalDistanceAttenuation(unL, invHalfDim, Alight.rangeAttenuationScale, Alight.rangeAttenuationBias);

		#else

			ALF = BoxDistanceAttenuation(unL, invHalfDim, Alight.rangeAttenuationScale, Alight.rangeAttenuationBias);

		#endif

			if (ALF != 0.0)
			{

				Alight.positionRWS -= posInput.positionWS;

				float4x3 lightVerts;

				lightVerts[0] = Alight.positionRWS + Alight.right * -halfWidth + Alight.up * -halfHeight;
				lightVerts[1] = Alight.positionRWS + Alight.right * -halfWidth + Alight.up *  halfHeight;
				lightVerts[2] = Alight.positionRWS + Alight.right *  halfWidth + Alight.up *  halfHeight;
				lightVerts[3] = Alight.positionRWS + Alight.right *  halfWidth + Alight.up * -halfHeight;

				#if N_F_ALSL_ON
					lightVerts = mul(lightVerts, transpose( GetOrthoBasisViewNormal(viewDirection, normalDirection, dot(normalDirection,viewDirection) ) ) );
				#else
					lightVerts = mul(lightVerts, transpose( (float3x3) lightVerts ) );
				#endif
																							
				float4x3 LD = mul(lightVerts, 1.0);
				float3 formFactor = (float3)0.01;

				#if N_F_ALSL_ON
					ALF2 = 1.0;

					float3 ALRTLF = PolygonIrradiance(LD,formFactor);

					if ( Alight.cookieMode != COOKIEMODE_NONE )
					{
						ALRTLF *= SampleAreaLightCookie(Alight.cookieScaleOffset, lightVerts, formFactor, 1.0);
					}

					ALigCol = float4( (Alight.color * ALRTLF) * GetCurrentExposureMultiplier() ,1.0);
				#else
					ALF2 = 1.0;

					ALCk =  Po_Ir(LD);

					if ( Alight.cookieMode != COOKIEMODE_NONE )
					{
						ALCk *= SampleAreaLightCookie(Alight.cookieScaleOffset, lightVerts, formFactor, 1.0); 
					}

					ALigCol = float4( (Alight.color * ALCk) * GetCurrentExposureMultiplier() ,1.0);
				#endif

			}

		}

			float shadowMask = 1.0;

			#ifdef SHADOWS_SHADOWMASK

				ASha = shadowMask = (Alight.shadowMaskSelector.x >= 0.0) ? dot(BUILTIN_DATA_SHADOW_MASK, Alight.shadowMaskSelector) : 1.0;

			#endif
										
				#if	N_F_ESSS_ON

					#if !defined(N_F_TRANS_ON) || defined(N_F_CO_ON) //defined(SCREEN_SPACE_SHADOWS) && 

						if ( (Alight.screenSpaceShadowIndex & SCREEN_SPACE_SHADOW_INDEX_MASK) != INVALID_SCREEN_SPACE_SHADOW )
						{
							ASha = GetScreenSpaceShadow(posInput, Alight.screenSpaceShadowIndex);
						}

						else

					#endif 

				#endif

				if (Alight.shadowIndex != -1)
				{

						ASha = GetRectAreaShadowAttenuation(context.shadowContext, posInput.positionSS, posInput.positionWS, normalDirection, Alight.shadowIndex, normalize(Alight.positionRWS), length(Alight.positionRWS)); //

						#ifdef SHADOWS_SHADOWMASK
									ASha = Alight.nonLightMappedOnly ? min(shadowMask, ASha) : ASha;
						#endif

						ASha = lerp(shadowMask, ASha, Alight.shadowDimmer);

				}

		}

}

#endif
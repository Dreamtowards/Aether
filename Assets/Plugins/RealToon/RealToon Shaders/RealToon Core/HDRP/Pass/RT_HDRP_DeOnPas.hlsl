//RealToon HDRP - DeOnPas
//MJQStudioWorks

#include "Assets/Plugins/RealToon/RealToon Shaders/RealToon Core/HDRP/RT_HDRP_Other.hlsl"

#if (SHADERPASS != SHADERPASS_DEPTH_ONLY && SHADERPASS != SHADERPASS_SHADOWS)
	#error SHADERPASS_is_not_correctly_define
#endif

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/VertMesh.hlsl"

PackedVaryingsType Vert(AttributesMesh inputMesh)
{
	VaryingsType varyingsType;

#if (SHADERPASS == SHADERPASS_DEPTH_ONLY) && defined(HAVE_RECURSIVE_RENDERING) && !defined(SCENESELECTIONPASS) && !defined(SCENEPICKINGPASS)

	if (_EnableRecursiveRayTracing && _RecurRen > 0.0)
	{
		ZERO_INITIALIZE(VaryingsType, varyingsType);
	}
	else
#endif
	{
		varyingsType.vmesh = VertMesh(inputMesh);
	}

	return PackVaryingsType(varyingsType);
}

			
void Frag(  PackedVaryingsToPS packedInput
            #if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
            , out float4 outColor : SV_Target0
            #else
                #ifdef WRITE_MSAA_DEPTH
                , out float4 depthColor : SV_Target0
                    #ifdef WRITE_NORMAL_BUFFER
                    , out float4 outNormalBuffer : SV_Target1
                    #endif
                #else
                    #ifdef WRITE_NORMAL_BUFFER
                    , out float4 outNormalBuffer : SV_Target0
                    #endif
                #endif
            #endif

            #if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
            , out float outputDepth : DEPTH_OFFSET_SEMANTIC
            #endif
        )
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
	FragInputs input = UnpackVaryingsMeshToFragInputs(packedInput);

	PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

	#ifdef VARYINGS_NEED_POSITION_WS
		float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);
	#else
		float3 V = float3(1.0, 1.0, 1.0);
	#endif
				
	SurfaceData surfaceData;
	BuiltinData builtinData;

	GetSurfaceAndBuiltinData(input, V, posInput, surfaceData, builtinData);

	//RT_NM
	#if N_F_NM_ON
		float3 normalTS = float3(0.0,0.0,0.0);
		normalTS = RT_NM(input.texCoord0.xy, input.positionRWS.xyz, input.tangentToWorld, surfaceData.normalWS );
		
		surfaceData.perceptualSmoothness = _Smoothness;
		surfaceData.normalWS = SafeNormalize(TransformTangentToWorld(normalTS, input.tangentToWorld));
	#endif

	//RT CO ONLY
    RT_CO_ONLY(input.texCoord0.xy, input.positionRWS.xyz, surfaceData.normalWS, input.positionSS.xy);

	#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
		outputDepth = posInput.deviceDepth;

		#if SHADERPASS == SHADERPASS_SHADOWS
			float bias = max(abs(ddx(posInput.deviceDepth)), abs(ddy(posInput.deviceDepth))) * _SlopeScaleDepthBias;
			outputDepth += bias;
		#endif
	#endif

	//RT_NFD
	#ifdef N_F_NFD_ON
		RT_NFD(packedInput.vmesh.positionCS.xy);
	#endif

	#ifdef SCENESELECTIONPASS
		outColor = float4(_ObjectId, _PassValue, 1.0, 1.0);
	#elif defined(SCENEPICKINGPASS)
		outColor = unity_SelectionID;
	#else
		#ifdef WRITE_MSAA_DEPTH
			depthColor = packedInput.vmesh.positionCS.z;
		#endif

		#if defined(WRITE_NORMAL_BUFFER)
			EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
		#endif
	#endif

}
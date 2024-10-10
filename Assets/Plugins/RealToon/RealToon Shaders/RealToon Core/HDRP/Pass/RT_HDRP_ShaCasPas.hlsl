//RealToon HDRP - ShaCasPas
//MJQStudioWorks

#include "Assets/RealToon/RealToon Shaders/RealToon Core/HDRP/RT_HDRP_Other.hlsl"

float4 _ShadowBias;
float3 _LightDirection;

sampler3D _DitherMaskLOD;
float dither;

struct Attributes
{

	float4 positionOS   : POSITION;
	float3 normalOS     : NORMAL;
    float4 tangentOS	: TANGENT;
	float2 texcoord     : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID

};

struct Varyings
{

	float2 uv           : TEXCOORD0;
    float3 normalWS		: TEXCOORD1;
    float4 tangentWS	: TEXCOORD2;
	float4 projPos		: TEXCOORD3;
	float3 positionWS	: TEXCOORD4;
	float4 positionCS   : SV_POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO

};

float4 GetShadowPositionHClip(Attributes input, float3 normalWS)
{

	float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);

	float invNdotL = 1.0 - saturate(dot(_LightDirection, positionWS));
	float scale = invNdotL * _ShadowBias.y;

	positionWS = _LightDirection * _ShadowBias.xxx + positionWS;
	positionWS = normalWS * scale.xxx + positionWS;
	float4 positionCS = TransformWorldToHClip( positionWS );

	#if UNITY_REVERSED_Z
		positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE) + - _ReduSha * 0.01;
	#else
		positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE) + _ReduSha * 0.01;
	#endif

	return positionCS;

}

Varyings ShadowPassVertex(Attributes input)
{

	Varyings output;
	ZERO_INITIALIZE(Varyings, output);

	UNITY_SETUP_INSTANCE_ID (input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

	output.uv = input.texcoord;

	float4 objPos = mul (GetObjectToWorldMatrix(), float4(0.0,0.0,0.0,1.0) );

    output.normalWS = TransformObjectToWorldDir(input.normalOS);
    output.tangentWS = float4(TransformObjectToWorldDir(input.tangentOS.xyz), input.tangentOS.w);
	output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
    output.positionCS = TransformWorldToHClip(TransformObjectToWorld(input.positionOS.xyz));
    output.projPos = ComputeScreenPos (output.positionCS);
	output.positionCS = GetShadowPositionHClip(input, output.normalWS);

	return output;

}

void ShadowPassFragment(Varyings input, out float4 outColor : SV_Target0)
{

	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	UNITY_SETUP_INSTANCE_ID (input);

	float4 objPos = mul ( GetObjectToWorldMatrix(), float4(0.0,0.0,0.0,1.0) );
    float2 sceneUVs = (input.projPos.xy / input.projPos.w);
	float3 viewDirection = GetWorldSpaceNormalizeViewDir(input.positionWS);

	float2 RTD_OB_VP_CAL = distance(objPos.xyz, GetCurrentViewPosition());
	float2 RTD_VD_Cal = (float2((sceneUVs.x * 2.0 - 1.0)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2.0 - 1.0).rg*RTD_OB_VP_CAL);

    float2 _TexturePatternStyle_var = lerp( input.uv, RTD_VD_Cal, _TexturePatternStyle );
		
	#ifdef N_F_TP_ON
		float4 _MainTex_var = RT_Tripl_Default(_MainTex, sampler_MainTex, input.positionWS.xyz, input.normalWS);
	#else
        float4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, TRANSFORM_TEX(_TexturePatternStyle_var, _MainTex));
	#endif

	#if !defined(SHADER_STAGE_RAY_TRACING) //&& !defined(_TESSELLATION_DISPLACEMENT)
		#ifdef LOD_FADE_CROSSFADE 
			LODDitheringTransition(ComputeFadeMaskSeed(viewDirection, input.positionCS.xy), unity_LODFade.x);
		#endif
	#endif

	#if N_F_TRANS_ON

		#if N_F_CO_ON
		
			#ifdef N_F_TP_ON
				float4 _SecondaryCutout_var = RT_Tripl_Default(_SecondaryCutout, sampler_SecondaryCutout, input.positionWS.xyz, SafeNormalize( mul( float3(0.0,0.0,1.0), input.tangentWS.xyz) ) );
			#else
				float4 _SecondaryCutout_var = SAMPLE_TEXTURE2D(_SecondaryCutout, sampler_SecondaryCutout , input.uv);
			#endif
		
			float RTD_CO_ON = lerp( (lerp((_MainTex_var.r*_SecondaryCutout_var.r),_SecondaryCutout_var.r,_UseSecondaryCutoutOnly)+lerp(0.5,(-1.0),_Cutout)), saturate(( (1.0 - _Cutout) > 0.5 ? (1.0-(1.0-2.0*((1.0 - _Cutout)-0.5))*(1.0-lerp((_MainTex_var.a*_SecondaryCutout_var.r),_SecondaryCutout_var.a,_UseSecondaryCutoutOnly))) : (2.0*(1.0 - _Cutout)*lerp((_MainTex_var.a*_SecondaryCutout_var.r),_SecondaryCutout_var.a,_UseSecondaryCutoutOnly)) )), _AlphaBasedCutout );
			float RTD_CO = RTD_CO_ON;
		
			#ifdef N_F_SCO_ON
				clip( -( RT_Dither_Out(input.positionCS.xy) - RTD_CO ));
			#else
				clip(RTD_CO - 0.5);
			#endif
            
		#else
		
			#ifdef N_F_TP_ON
				float4 _MaskTransparency_var = RT_Tripl_Default(_MaskTransparency, sampler_MaskTransparency, input.positionWS.xyz, SafeNormalize( mul( float3(0.0,0.0,1.0), input.tangentWS.xyz) ) );
			#else
				float4 _MaskTransparency_var = SAMPLE_TEXTURE2D(_MaskTransparency, sampler_MaskTransparency , input.uv);
			#endif

			float RTD_TRAN_MAS = (smoothstep(clamp(-20.0,1.0,_TransparentThreshold),1.0,_MainTex_var.a) *_MaskTransparency_var.r);
			float RTD_TRAN_OPA_Sli = lerp( RTD_TRAN_MAS, smoothstep(clamp(-20.0,1.0,_TransparentThreshold) , 1.0, _MainTex_var.a)  ,_Opacity);

			dither = tex3D(_DitherMaskLOD, float3(input.positionCS.xy * 0.25, RTD_TRAN_OPA_Sli * 0.99)).a;
			clip(saturate(( 0.74 > 0.5 ? (1.0-(1.0-2.0*(0.74-0.5))*(1.0-dither)) : (2.0*0.74*dither) )) - 0.5);

		#endif

	#endif

	//RT_NFD
	#ifdef N_F_NFD_ON
		RT_NFD(input.positionCS.xy);
	#endif

	outColor = 0;
}

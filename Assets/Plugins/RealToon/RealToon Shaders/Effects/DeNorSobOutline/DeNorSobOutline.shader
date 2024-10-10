//RealToon - DeNorSob Outline Effect (HDRP - Post Processing)
//MJQStudioWorks
//2024

Shader  "Hidden/HDRP/RealToon/Effects/DeNorSobOutline"
{

    HLSLINCLUDE
		
		#pragma shader_feature_local RENDER_OUTLINE_ALL
        #pragma shader_feature_local MIX_DENOR_SOB

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
		#include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/NormalBuffer.hlsl"

		struct Attributes {
            uint vertexID : SV_VertexID;
			UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings {
            float4 positionCS : SV_POSITION;
            float2 texcoord    : TEXCOORD1;
        };

        Varyings Vert (Attributes input) {

            Varyings output;
			ZERO_INITIALIZE(Varyings, output);

			UNITY_SETUP_INSTANCE_ID(input);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
			output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);

            return output;
        }

		TEXTURE2D_X(_InputTexture);

		float _OutlineWidth;

        float _DepthThreshold;
        float _NormalThreshold;
        float _NormalMin;
        float _NormalMax;

        float _SobOutSel;
        float _SobelOutlineThreshold;
        float _WhiThres;
        float _BlaThres;

        float3 _OutlineColor;
		float _OutlineColorIntensity;
		float _ColOutMiSel;

		float _OutOnSel;
        float _MixDeNorSob;

        float SamDep(float2 uv)
        {
            float output = min(max(LOAD_TEXTURE2D_X(_InputTexture, uv).r, _BlaThres), _WhiThres);

			return output;
        }

		float sob_fil (float CX, float2 uv) 
        {
            float2 d = float2(CX, CX);
                
            float hr = 0;
            float vt = 0;
                
            hr += SamDep(uv + float2(-1.0, -1.0) * d) *  1.0;
            hr += SamDep(uv + float2( 1.0, -1.0) * d) * -1.0;
            hr += SamDep(uv + float2(-1.0,  0.0) * d) *  2.0;
            hr += SamDep(uv + float2( 1.0,  0.0) * d) * -2.0;
            hr += SamDep(uv + float2(-1.0,  1.0) * d) *  1.0;
            hr += SamDep(uv + float2( 1.0,  1.0) * d) * -1.0;
                
            vt += SamDep(uv + float2(-1.0, -1.0) * d) *  1.0;
            vt += SamDep(uv + float2( 0.0, -1.0) * d) *  2.0;
            vt += SamDep(uv + float2( 1.0, -1.0) * d) *  1.0;
            vt += SamDep(uv + float2(-1.0,  1.0) * d) * -1.0;
            vt += SamDep(uv + float2( 0.0,  1.0) * d) * -2.0;
            vt += SamDep(uv + float2( 1.0,  1.0) * d) * -1.0;
               
            return sqrt( dot(hr,hr) + dot(vt,vt)) ;

        }

        //Most of the lines are based on unity hdrp example
        float SampleClampedDepth(float2 uv) { return SampleCameraDepth(clamp(uv, _ScreenSize.zw, 1 - _ScreenSize.zw)).r; }

        //Most of the lines are based on unity hdrp example
        float EdgeDetect(float2 uv, float4 input_pos_cs)
        {

            float2 _ScrSi = _OutlineWidth / float2(1920, 1080);

            float depth = LoadCameraDepth(input_pos_cs.xy);
            float obj_only = depth != UNITY_RAW_FAR_CLIP_VALUE;

            float halfScaleFloor = floor(_OutlineWidth * 0.5);
            float halfScaleCeil = ceil(_OutlineWidth * 0.5);

            float2 bottomLeftUV = uv - float2(_ScrSi.x, _ScrSi.y) * halfScaleFloor;
            float2 topRightUV = uv + float2(_ScrSi.x, _ScrSi.y) * halfScaleCeil;
            float2 bottomRightUV = uv + float2(_ScrSi.x * halfScaleCeil, -_ScrSi.y * halfScaleFloor);
            float2 topLeftUV = uv + float2(-_ScrSi.x * halfScaleFloor, _ScrSi.y * halfScaleCeil);

            float depth0 = SampleClampedDepth(bottomLeftUV);
            float depth1 = SampleClampedDepth(topRightUV);
            float depth2 = SampleClampedDepth(bottomRightUV);
            float depth3 = SampleClampedDepth(topLeftUV);

            float depthDerivative0 = depth1 - depth0;
            float depthDerivative1 = depth3 - depth2;

            float edgeDepth = sqrt(pow(depthDerivative0, 2) + pow(depthDerivative1, 2)) * 100;

            edgeDepth = edgeDepth > ( depth0 * (_DepthThreshold * 0.01) ) ? 1 : 0;

            NormalData normalData0, normalData1, normalData2, normalData3;
            DecodeFromNormalBuffer(_ScreenSize.xy * bottomLeftUV, normalData0);
            DecodeFromNormalBuffer(_ScreenSize.xy * topRightUV, normalData1);
            DecodeFromNormalBuffer(_ScreenSize.xy * bottomRightUV, normalData2);
            DecodeFromNormalBuffer(_ScreenSize.xy * topLeftUV, normalData3);

            float3 normalFiniteDifference0 = (normalData1.normalWS - normalData0.normalWS);
            float3 normalFiniteDifference1 = (normalData3.normalWS - normalData2.normalWS);

            float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
            edgeNormal = smoothstep(_NormalMin, _NormalMax, edgeNormal * _NormalThreshold);
            edgeNormal *= obj_only;

            float edgeSob = sob_fil(_OutlineWidth + 0.49, uv * _ScreenSize.xy) > ((_SobelOutlineThreshold * 0.01)) ? 1 : 0;
            edgeSob *= obj_only;

             #ifndef MIX_DENOR_SOB

                #ifdef RENDER_OUTLINE_ALL
                    return edgeSob;
                #else
                    return max(edgeDepth, edgeNormal);
                #endif

            #else

                #ifdef RENDER_OUTLINE_ALL
                    float edgeSob_Mix = edgeSob;
                #else
                    float edgeSob_Mix = 0.0;
                #endif

                return max(edgeDepth, max(edgeNormal, edgeSob_Mix));

            #endif

            return saturate(max(edgeDepth, edgeNormal));
        }

        float4 SobelOutlinePostProcess(Varyings input) : SV_Target
        {
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				uint2 positionSS = input.texcoord * _ScreenSize.xy;
                float3 ful_scr_so = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

                float denorsobOut = EdgeDetect(input.texcoord, input.positionCS);

				float3 coloutmix = lerp(_OutlineColor * _OutlineColorIntensity, lerp(ful_scr_so * ful_scr_so, ful_scr_so , _OutlineColorIntensity) * _OutlineColor, _ColOutMiSel);
                return float4(lerp(coloutmix, lerp(ful_scr_so, 1, _OutOnSel), (1.0 - denorsobOut)), 1);
        }

    ENDHLSL

    SubShader
    {
		Tags{ "RenderPipeline" = "HDRenderPipeline" }
        ZWrite Off
        ZTest Always
        Blend Off
        Cull Off

        Pass
        {
            HLSLPROGRAM

                #pragma vertex Vert
                #pragma fragment SobelOutlinePostProcess

            ENDHLSL
        }
    }

	Fallback Off
}

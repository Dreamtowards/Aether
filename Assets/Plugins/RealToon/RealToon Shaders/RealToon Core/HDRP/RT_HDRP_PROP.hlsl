//RealToon HDRP - RT_HDRP_PROP
//MJQStudioWorks


//===============================================================================
//CBUF
//===============================================================================

CBUFFER_START(UnityPerMaterial)

//==
//RealToon HDRP - CBUF
//==


//==  N_F_O_ON
uniform float4 _OutlineWidthControl_ST;
uniform float _OutlineWidth;

uniform float4 _ONormMap_ST;
uniform float _ONormMapInt;
//==


//==  N_F_O_SSOL
uniform float _DepthThreshold;

uniform float _NormalThreshold;
uniform float _NormalMin;
uniform float _NormalMax;
//==


//== Others
float4 _MainTex_ST;

uniform float4 _MainColor;
uniform float _MaiColPo;
uniform float _MVCOL;
uniform float _MCIALO;
uniform float _TexturePatternStyle;
uniform float4 _HighlightColor;
uniform float _HighlightColorPower;
uniform float _EnableTextureTransparent;

uniform float _ReduSha;
//==


//==  N_F_MC_ON
uniform float4 _MCap_ST;
uniform float4 _MCapMask_ST;

uniform float _MCapIntensity;
uniform float _SPECMODE;
uniform float _SPECIN;
//==


//==  N_F_TRANS_ON -> N_F_CO_ON
uniform float4 _SecondaryCutout_ST;

uniform float _Cutout;
uniform float _AlphaBasedCutout;
uniform float _UseSecondaryCutoutOnly;

uniform float _Glow_Edge_Width;
uniform float4 _Glow_Color;
//==


//==  N_F_TRANS_ON
float4 _MaskTransparency_ST;

uniform float _Opacity;
uniform float _TransparentThreshold;
uniform float _TOAO;
//==


//== N_F_NM_ON
uniform float4 _NormalMap_ST;
//==


//== N_F_CA_ON
uniform float _Saturation;
//==


//== N_F_SL_ON
float4 _MaskSelfLit_ST;

uniform float _SelfLitIntensity;
uniform float4 _SelfLitColor;
uniform float _SelfLitPower;
uniform float _TEXMCOLINT;
uniform float _SelfLitHighContrast;
//==


//== N_F_GLO_ON
float4 _MaskGloss_ST;

uniform float _GlossIntensity;
uniform float _Glossiness;
uniform float _GlossSoftness;
uniform float4 _GlossColor;
uniform float _GlossColorPower;
//==
 

//== N_F_GLO_ON -> N_F_GLOT_ON
float4 _GlossTexture_ST;

uniform float _GlossTextureSoftness;
uniform float _PSGLOTEX;
uniform float _GlossTextureRotate;
uniform float _GlossTextureFollowObjectRotation;
uniform float _GlossTextureFollowLight;
//==


//== Others
uniform float4 _OverallShadowColor;
uniform float _OverallShadowColorPower;

uniform float _SelfShadowShadowTAtViewDirection;

uniform float _ShadowHardness;
uniform float _SelfShadowRealtimeShadowIntensity;
//==


//== N_F_SS_ON
uniform float _SelfShadowThreshold;
uniform float _VertexColorGreenControlSelfShadowThreshold;
uniform float _SelfShadowHardness;
uniform float _LigIgnoYNorDir;
uniform float _SelfShadowAffectedByLightShadowStrength;
//==


//== Others
uniform float4 _SelfShadowRealTimeShadowColor;
uniform float _SelfShadowRealTimeShadowColorPower;
//==


//== N_F_SON_ON
uniform float _SmoothObjectNormal;
uniform float _VertexColorRedControlSmoothObjectNormal;
uniform float4 _XYZPosition;
uniform float _ShowNormal;
//==


//== N_F_SCT_ON
uniform float4 _ShadowColorTexture_ST;
uniform float _ShadowColorTexturePower;
//==


//== N_F_ST_ON
float4 _ShadowT_ST;

uniform float _ShadowTIntensity;
uniform float _ShadowTLightThreshold;
uniform float _ShadowTShadowThreshold;
uniform float4 _ShadowTColor;
uniform float _ShadowTColorPower;
uniform float _ShadowTHardness;
uniform float _STIL;
uniform float _ShowInAmbientLightShadowIntensity;
uniform float _ShowInAmbientLightShadowThreshold;
uniform float _LightFalloffAffectShadowT;
//==


//== N_F_PT_ON
uniform float4 _PTexture_ST;

uniform float4 _PTCol;
uniform float _PTexturePower;
//==


//== N_F_RELGI_ON
uniform float _GIFlatShade;
uniform float _GIShadeThreshold;
uniform float _EnvironmentalLightingIntensity;
//==


//== Others
uniform float _LightAffectShadow;
uniform float _LightIntensity;
uniform float _DirectionalLightIntensity;
uniform float _PointSpotlightIntensity;
uniform float _ALIntensity;
uniform float _ALTuFo;
uniform float _LightFalloffSoftness;
//==


//==N_F_CLD_ON
uniform float _CustomLightDirectionIntensity;
uniform float4 _CustomLightDirection;
uniform float _CustomLightDirectionFollowObjectRotation;
//==


//== N_F_R_ON
uniform float4 _MaskReflection_ST;

uniform float _ReflectionIntensity;
uniform float _ReflectionRoughtness;
uniform float _RefMetallic;
//==


//== N_F_R_ON -> N_F_FR_ON
uniform float4 _FReflection_ST;
//==


//== N_F_RL_ON
uniform float _RimLigInt;
uniform float _RimLightUnfill;
uniform float _RimLighThres;
uniform float _RimLightSoftness;
uniform float _LightAffectRimLightColor;
uniform float4 _RimLightColor;
uniform float _RimLightColorPower;
uniform float _RimLightInLight;
//==


//== N_F_O_ON
uniform float3 _OEM;
uniform int _OutlineExtrudeMethod;
uniform float3 _OutlineOffset;
uniform float _OutlineZPostionInCamera;
uniform float4 _OutlineColor;
uniform float _MixMainTexToOutline;
uniform float _NoisyOutlineIntensity;
uniform float _DynamicNoisyOutline;
uniform float _LightAffectOutlineColor;
uniform float _OutlineWidthAffectedByViewDistance;
uniform float _FarDistanceMaxWidth;
uniform float _VertexColorBlueAffectOutlineWitdh;
//==


//== N_F_NFD_ON
uniform float _MinFadDistance;
uniform float _MaxFadDistance;
//==


//== N_F_TP_ON
uniform float _TriPlaTile;
uniform float _TriPlaBlend;
//==


//== Others
uniform float4 _SSAOColor;
uniform float _RTGIShaFallo;
uniform float _RecurRen;
//==


//==Tessellation is still in development
//#ifdef TESSELLATION_ON
    //uniform float _TessellationSmoothness;
    //uniform half _TessellationTransition;
    //uniform half _TessellationNear;
    //uniform half _TessellationFar;
//#endif
//==


//==
//Unity HDRP Standard Prop - CBUF
//==

float _DistortionScale;
float _DistortionVectorScale;
float _DistortionVectorBias;
float _DistortionBlurScale;
float _DistortionBlurRemapMin;
float _DistortionBlurRemapMax;

float3 _EmissiveColor;
float _AlbedoAffectEmissive;
float _EmissiveExposureWeight;

float4 _BaseColor;
float4 _BaseColorMap_ST;
float4 _BaseColorMap_TexelSize;
float4 _BaseColorMap_MipInfo;

float _Metallic;
float _Smoothness;

float _NormalScale;

float4 _DetailMap_ST;
//float _DetailAlbedoScale;
//float _DetailNormalScale;
//float _DetailSmoothnessScale;

float _Anisotropy;

float _DiffusionProfileHash;
float _SubsurfaceMask;
float _Thickness;

float4 _SpecularColor;

float _TexWorldScale;
float4 _UVMappingMask;
float4 _UVDetailsMappingMask;
float4 _UVMappingMaskEmissive;
float _LinkDetailsWithBase;

float _AlphaRemapMin;
float _AlphaRemapMax;
float _ObjectSpaceUVMapping;
float _TransmissionMask; //added - Unity 2022.2.0b1

CBUFFER_END

//===============================================================================
//DotsInts
//===============================================================================

#if defined(UNITY_DOTS_INSTANCING_ENABLED)

UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)

//==  N_F_O_ON
UNITY_DOTS_INSTANCED_PROP(float, _OutlineWidth)
UNITY_DOTS_INSTANCED_PROP(float, _ONormMapInt)
//==

//==  N_F_O_SSOL
UNITY_DOTS_INSTANCED_PROP(float, _DepthThreshold)
UNITY_DOTS_INSTANCED_PROP(float, _NormalThreshold)
UNITY_DOTS_INSTANCED_PROP(float, _NormalMin)
UNITY_DOTS_INSTANCED_PROP(float, _NormalMax)
//==

//== Others
UNITY_DOTS_INSTANCED_PROP(float4, _MainColor)
UNITY_DOTS_INSTANCED_PROP(float, _MaiColPo)
UNITY_DOTS_INSTANCED_PROP(float, _MVCOL)
UNITY_DOTS_INSTANCED_PROP(float, _MCIALO)
UNITY_DOTS_INSTANCED_PROP(float, _TexturePatternStyle)
UNITY_DOTS_INSTANCED_PROP(float4, _HighlightColor)
UNITY_DOTS_INSTANCED_PROP(float, _HighlightColorPower)
UNITY_DOTS_INSTANCED_PROP(float, _EnableTextureTransparent)
UNITY_DOTS_INSTANCED_PROP(float, _ReduSha)
//==

//==  N_F_MC_ON
UNITY_DOTS_INSTANCED_PROP(float, _MCapIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _SPECMODE)
UNITY_DOTS_INSTANCED_PROP(float, _SPECIN)
//==

//==  N_F_TRANS_ON -> N_F_CO_ON
UNITY_DOTS_INSTANCED_PROP(float, _Cutout)
UNITY_DOTS_INSTANCED_PROP(float, _AlphaBasedCutout)
UNITY_DOTS_INSTANCED_PROP(float, _UseSecondaryCutoutOnly)

UNITY_DOTS_INSTANCED_PROP(float4, _Glow_Color)
UNITY_DOTS_INSTANCED_PROP(float, _Glow_Edge_Width)
//==

//==  N_F_TRANS_ON
UNITY_DOTS_INSTANCED_PROP(float, _Opacity)
UNITY_DOTS_INSTANCED_PROP(float, _TransparentThreshold)
UNITY_DOTS_INSTANCED_PROP(float, _TOAO)
//==

//== N_F_CA_ON
UNITY_DOTS_INSTANCED_PROP(float, _Saturation)
//==

//== N_F_SL_ON
UNITY_DOTS_INSTANCED_PROP(float, _SelfLitIntensity)
UNITY_DOTS_INSTANCED_PROP(float4, _SelfLitColor)
UNITY_DOTS_INSTANCED_PROP(float, _SelfLitPower)
UNITY_DOTS_INSTANCED_PROP(float, _TEXMCOLINT)
UNITY_DOTS_INSTANCED_PROP(float, _SelfLitHighContrast)
//==

//== N_F_GLO_ON
UNITY_DOTS_INSTANCED_PROP(float, _GlossIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _Glossiness)
UNITY_DOTS_INSTANCED_PROP(float, _GlossSoftness)
UNITY_DOTS_INSTANCED_PROP(float4, _GlossColor)
UNITY_DOTS_INSTANCED_PROP(float, _GlossColorPower)
//==

//== N_F_GLO_ON -> N_F_GLOT_ON
UNITY_DOTS_INSTANCED_PROP(float, _GlossTextureSoftness)
UNITY_DOTS_INSTANCED_PROP(float, _PSGLOTEX)
UNITY_DOTS_INSTANCED_PROP(float, _GlossTextureRotate)
UNITY_DOTS_INSTANCED_PROP(float, _GlossTextureFollowObjectRotation)
UNITY_DOTS_INSTANCED_PROP(float, _GlossTextureFollowLight)
//==

//== Others
UNITY_DOTS_INSTANCED_PROP(float4, _OverallShadowColor)
UNITY_DOTS_INSTANCED_PROP(float, _OverallShadowColorPower)
UNITY_DOTS_INSTANCED_PROP(float, _SelfShadowShadowTAtViewDirection)
UNITY_DOTS_INSTANCED_PROP(float, _ShadowHardness)
UNITY_DOTS_INSTANCED_PROP(float, _SelfShadowRealtimeShadowIntensity)
//==

//== Others
UNITY_DOTS_INSTANCED_PROP(float, _SelfShadowThreshold)
UNITY_DOTS_INSTANCED_PROP(float, _VertexColorGreenControlSelfShadowThreshold)
UNITY_DOTS_INSTANCED_PROP(float, _SelfShadowHardness)
UNITY_DOTS_INSTANCED_PROP(float, _LigIgnoYNorDir)
UNITY_DOTS_INSTANCED_PROP(float, _SelfShadowAffectedByLightShadowStrength)
//==

//== Others
UNITY_DOTS_INSTANCED_PROP(float4, _SelfShadowRealTimeShadowColor)
UNITY_DOTS_INSTANCED_PROP(float, _SelfShadowRealTimeShadowColorPower)
//==

//== N_F_SO_ON
UNITY_DOTS_INSTANCED_PROP(float, _SmoothObjectNormal)
UNITY_DOTS_INSTANCED_PROP(float, _VertexColorRedControlSmoothObjectNormal)
UNITY_DOTS_INSTANCED_PROP(float4, _XYZPosition)
UNITY_DOTS_INSTANCED_PROP(float, _ShowNormal)
//==

//== N_F_SCT_ON
UNITY_DOTS_INSTANCED_PROP(float, _ShadowColorTexturePower)
//==

//== N_F_ST_ON
UNITY_DOTS_INSTANCED_PROP(float, _ShadowTIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _ShadowTLightThreshold)
UNITY_DOTS_INSTANCED_PROP(float, _ShadowTShadowThreshold)
UNITY_DOTS_INSTANCED_PROP(float4, _ShadowTColor)
UNITY_DOTS_INSTANCED_PROP(float, _ShadowTColorPower)
UNITY_DOTS_INSTANCED_PROP(float, _ShadowTHardness)
UNITY_DOTS_INSTANCED_PROP(float, _STIL)
UNITY_DOTS_INSTANCED_PROP(float, _ShowInAmbientLightShadowIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _ShowInAmbientLightShadowThreshold)
UNITY_DOTS_INSTANCED_PROP(float, _LightFalloffAffectShadowT)
//==

//== N_F_PT_ON
UNITY_DOTS_INSTANCED_PROP(float4, _PTCol)
UNITY_DOTS_INSTANCED_PROP(float, _PTexturePower)
//==

//== N_F_RELGI_ON
UNITY_DOTS_INSTANCED_PROP(float, _GIFlatShade)
UNITY_DOTS_INSTANCED_PROP(float, _GIShadeThreshold)
UNITY_DOTS_INSTANCED_PROP(float, _EnvironmentalLightingIntensity)
//==

//== Others
UNITY_DOTS_INSTANCED_PROP(float, _LightAffectShadow)
UNITY_DOTS_INSTANCED_PROP(float, _LightIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _DirectionalLightIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _PointSpotlightIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _ALIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _ALTuFo)
UNITY_DOTS_INSTANCED_PROP(float, _LightFalloffSoftness)
//==

//==N_F_CLD_ON
UNITY_DOTS_INSTANCED_PROP(float, _CustomLightDirectionIntensity)
UNITY_DOTS_INSTANCED_PROP(float4, _CustomLightDirection)
UNITY_DOTS_INSTANCED_PROP(float, _CustomLightDirectionFollowObjectRotation)
//==

//== N_F_R_ON
UNITY_DOTS_INSTANCED_PROP(float, _ReflectionIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _ReflectionRoughtness) //remove soon
UNITY_DOTS_INSTANCED_PROP(float, _RefMetallic)
//==

//== N_F_RL_ON
UNITY_DOTS_INSTANCED_PROP(float, _RimLigInt)
UNITY_DOTS_INSTANCED_PROP(float, _RimLightUnfill)
UNITY_DOTS_INSTANCED_PROP(float, _RimLighThres)
UNITY_DOTS_INSTANCED_PROP(float, _RimLightSoftness)
UNITY_DOTS_INSTANCED_PROP(float, _LightAffectRimLightColor)
UNITY_DOTS_INSTANCED_PROP(float4, _RimLightColor)
UNITY_DOTS_INSTANCED_PROP(float, _RimLightColorPower)
UNITY_DOTS_INSTANCED_PROP(float, _RimLightInLight)
//==

//== N_F_O_ON
UNITY_DOTS_INSTANCED_PROP(int, _OutlineExtrudeMethod)
UNITY_DOTS_INSTANCED_PROP(float3, _OutlineOffset)
UNITY_DOTS_INSTANCED_PROP(float, _OutlineZPostionInCamera)
UNITY_DOTS_INSTANCED_PROP(float4, _OutlineColor)
UNITY_DOTS_INSTANCED_PROP(float, _MixMainTexToOutline)
UNITY_DOTS_INSTANCED_PROP(float, _NoisyOutlineIntensity)
UNITY_DOTS_INSTANCED_PROP(float, _DynamicNoisyOutline)
UNITY_DOTS_INSTANCED_PROP(float, _LightAffectOutlineColor)
UNITY_DOTS_INSTANCED_PROP(float, _OutlineWidthAffectedByViewDistance)
UNITY_DOTS_INSTANCED_PROP(float, _FarDistanceMaxWidth)
UNITY_DOTS_INSTANCED_PROP(float, _VertexColorBlueAffectOutlineWitdh)
//==

//== N_F_NFD_ON
UNITY_DOTS_INSTANCED_PROP(float, _MinFadDistance)
UNITY_DOTS_INSTANCED_PROP(float, _MaxFadDistance)
//==

//== N_F_TP_ON
UNITY_DOTS_INSTANCED_PROP(float, _TriPlaTile)
UNITY_DOTS_INSTANCED_PROP(float, _TriPlaBlend)
//==

//== Others
UNITY_DOTS_INSTANCED_PROP(float4, _SSAOColor)
UNITY_DOTS_INSTANCED_PROP(float, _RTGIShaFallo)
UNITY_DOTS_INSTANCED_PROP(float, _RecurRen)
UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMin)
UNITY_DOTS_INSTANCED_PROP(float, _AlphaRemapMax)
//==

//==Tessellation is still in development
//UNITY_DOTS_INSTANCED_PROP(float, _TessellationSmoothness)
//UNITY_DOTS_INSTANCED_PROP(float, _TessellationTransition)
//UNITY_DOTS_INSTANCED_PROP(float, _TessellationNear)
//UNITY_DOTS_INSTANCED_PROP(float, _TessellationFar)
//==

UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)




//==  N_F_O_ON
static float unity_DOTS_Sampled_OutlineWidth;
static float unity_DOTS_Sampled_ONormMapInt;
//==

//==  N_F_O_SSOL
static float unity_DOTS_Sampled_DepthThreshold;
static float unity_DOTS_Sampled_NormalThreshold;
static float unity_DOTS_Sampled_NormalMin;
static float unity_DOTS_Sampled_NormalMax;
//==

//== Others
static float4 unity_DOTS_Sampled_MainColor;
static float unity_DOTS_Sampled_MaiColPo;
static float unity_DOTS_Sampled_MVCOL;
static float unity_DOTS_Sampled_MCIALO;
static float unity_DOTS_Sampled_TexturePatternStyle;
static float4 unity_DOTS_Sampled_HighlightColor;
static float unity_DOTS_Sampled_HighlightColorPower;
static float unity_DOTS_Sampled_EnableTextureTransparent;
static float unity_DOTS_Sampled_ReduSha;
//==

//==  N_F_MC_ON
static float unity_DOTS_Sampled_MCapIntensity;
static float unity_DOTS_Sampled_SPECMODE;
static float unity_DOTS_Sampled_SPECIN;
//==

//==  N_F_TRANS_ON -> N_F_CO_ON
static float unity_DOTS_Sampled_Cutout;
static float unity_DOTS_Sampled_AlphaBasedCutout;
static float unity_DOTS_Sampled_UseSecondaryCutoutOnly;

static float4 unity_DOTS_Sampled_Glow_Color;
static float unity_DOTS_Sampled_Glow_Edge_Width;
//==

//==  N_F_TRANS_ON
static float unity_DOTS_Sampled_Opacity;
static float unity_DOTS_Sampled_TransparentThreshold;
static float unity_DOTS_Sampled_TOAO;
//==

//== N_F_CA_ON
static float unity_DOTS_Sampled_Saturation;
//==

//== N_F_SL_ON
static float unity_DOTS_Sampled_SelfLitIntensity;
static float4 unity_DOTS_Sampled_SelfLitColor;
static float unity_DOTS_Sampled_SelfLitPower;
static float unity_DOTS_Sampled_TEXMCOLINT;
static float unity_DOTS_Sampled_SelfLitHighContrast;
//==

//== N_F_GLO_ON
static float unity_DOTS_Sampled_GlossIntensity;
static float unity_DOTS_Sampled_Glossiness;
static float unity_DOTS_Sampled_GlossSoftness;
static float4 unity_DOTS_Sampled_GlossColor;
static float unity_DOTS_Sampled_GlossColorPower;
//==

//== N_F_GLO_ON -> N_F_GLOT_ON
static float unity_DOTS_Sampled_GlossTextureSoftness;
static float unity_DOTS_Sampled_PSGLOTEX;
static float unity_DOTS_Sampled_GlossTextureRotate;
static float unity_DOTS_Sampled_GlossTextureFollowObjectRotation;
static float unity_DOTS_Sampled_GlossTextureFollowLight;
//==

//== Others
static float4 unity_DOTS_Sampled_OverallShadowColor;
static float unity_DOTS_Sampled_OverallShadowColorPower;
static float unity_DOTS_Sampled_SelfShadowShadowTAtViewDirection;
static float unity_DOTS_Sampled_ShadowHardness;
static float unity_DOTS_Sampled_SelfShadowRealtimeShadowIntensity;
//==

//== Others
static float unity_DOTS_Sampled_SelfShadowThreshold;
static float unity_DOTS_Sampled_VertexColorGreenControlSelfShadowThreshold;
static float unity_DOTS_Sampled_SelfShadowHardness;
static float unity_DOTS_Sampled_LigIgnoYNorDir;
static float unity_DOTS_Sampled_SelfShadowAffectedByLightShadowStrength;
//==

//== Others
static float4 unity_DOTS_Sampled_SelfShadowRealTimeShadowColor;
static float unity_DOTS_Sampled_SelfShadowRealTimeShadowColorPower;
//==

//== N_F_SO_ON
static float unity_DOTS_Sampled_SmoothObjectNormal;
static float unity_DOTS_Sampled_VertexColorRedControlSmoothObjectNormal;
static float4 unity_DOTS_Sampled_XYZPosition;
static float unity_DOTS_Sampled_ShowNormal;
//==

//== N_F_SCT_ON
static float unity_DOTS_Sampled_ShadowColorTexturePower;
//==

//== N_F_ST_ON
static float unity_DOTS_Sampled_ShadowTIntensity;
static float unity_DOTS_Sampled_ShadowTLightThreshold;
static float unity_DOTS_Sampled_ShadowTShadowThreshold;
static float4 unity_DOTS_Sampled_ShadowTColor;
static float unity_DOTS_Sampled_ShadowTColorPower;
static float unity_DOTS_Sampled_ShadowTHardness;
static float unity_DOTS_Sampled_STIL;
static float unity_DOTS_Sampled_ShowInAmbientLightShadowIntensity;
static float unity_DOTS_Sampled_ShowInAmbientLightShadowThreshold;
static float unity_DOTS_Sampled_LightFalloffAffectShadowT;
//==

//== N_F_PT_ON
static float4 unity_DOTS_Sampled_PTCol;
static float unity_DOTS_Sampled_PTexturePower;
//==

//== N_F_RELGI_ON
static float unity_DOTS_Sampled_GIFlatShade;
static float unity_DOTS_Sampled_GIShadeThreshold;
static float unity_DOTS_Sampled_EnvironmentalLightingIntensity;
//==

//== Others
 static float unity_DOTS_Sampled_LightAffectShadow;
 static float unity_DOTS_Sampled_LightIntensity;
 static float unity_DOTS_Sampled_DirectionalLightIntensity;
 static float unity_DOTS_Sampled_PointSpotlightIntensity;
 static float unity_DOTS_Sampled_ALIntensity;
 static float unity_DOTS_Sampled_ALTuFo;
 static float unity_DOTS_Sampled_LightFalloffSoftness;
//==

//==N_F_CLD_ON
static float unity_DOTS_Sampled_CustomLightDirectionIntensity;
static float4 unity_DOTS_Sampled_CustomLightDirection;
static float unity_DOTS_Sampled_CustomLightDirectionFollowObjectRotation;
//==

//== N_F_R_ON
static float unity_DOTS_Sampled_ReflectionIntensity;
static float unity_DOTS_Sampled_ReflectionRoughtness;
static float unity_DOTS_Sampled_RefMetallic;
//==

//== N_F_RL_ON
static float unity_DOTS_Sampled_RimLigInt;
static float unity_DOTS_Sampled_RimLightUnfill;
static float unity_DOTS_Sampled_RimLighThres;
static float unity_DOTS_Sampled_RimLightSoftness;
static float unity_DOTS_Sampled_LightAffectRimLightColor;
static float4 unity_DOTS_Sampled_RimLightColor;
static float unity_DOTS_Sampled_RimLightColorPower;
static float unity_DOTS_Sampled_RimLightInLight;
//==

//== N_F_O_ON
static int unity_DOTS_Sampled_OutlineExtrudeMethod;
static float3 unity_DOTS_Sampled_OutlineOffset;
static float unity_DOTS_Sampled_OutlineZPostionInCamera;
static float4 unity_DOTS_Sampled_OutlineColor;
static float unity_DOTS_Sampled_MixMainTexToOutline;
static float unity_DOTS_Sampled_NoisyOutlineIntensity;
static float unity_DOTS_Sampled_DynamicNoisyOutline;
static float unity_DOTS_Sampled_LightAffectOutlineColor;
static float unity_DOTS_Sampled_OutlineWidthAffectedByViewDistance;
static float unity_DOTS_Sampled_FarDistanceMaxWidth;
static float unity_DOTS_Sampled_VertexColorBlueAffectOutlineWitdh;
//==

//== N_F_TP_ON
static float unity_DOTS_Sampled_TriPlaTile;
static float unity_DOTS_Sampled_TriPlaBlend;
//==

//== N_F_NFD_ON
static float unity_DOTS_Sampled_MinFadDistance;
static float unity_DOTS_Sampled_MaxFadDistance;
//==

//==Tessellation is still in development
//static int unity_DOTS_Sampled_TessellationSmoothness;
//static int unity_DOTS_Sampled_TessellationTransition;
//static int unity_DOTS_Sampled_TessellationNear;
//static int unity_DOTS_Sampled_TessellationFar;
//==

//== Others
static float4 unity_DOTS_Sampled_SSAOColor;
static float unity_DOTS_Sampled_RTGIShaFallo;
static float unity_DOTS_Sampled_RecurRen;
static float  unity_DOTS_Sampled_AlphaRemapMin;
static float  unity_DOTS_Sampled_AlphaRemapMax;
//==

void SetupDOTSLitMaterialPropertyCaches()
{

    //==  N_F_O_ON
    unity_DOTS_Sampled_OutlineWidth = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _OutlineWidth);
    unity_DOTS_Sampled_ONormMapInt = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ONormMapInt);
    //==

    //==  N_F_O_SSOL
    unity_DOTS_Sampled_DepthThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _DepthThreshold);
    unity_DOTS_Sampled_NormalThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _NormalThreshold);
    unity_DOTS_Sampled_NormalMin = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _NormalMin);
    unity_DOTS_Sampled_NormalMax = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _NormalMax);
    //==

    //== Others
    unity_DOTS_Sampled_MainColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _MainColor);
    unity_DOTS_Sampled_MaiColPo = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _MaiColPo);
    unity_DOTS_Sampled_MVCOL = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _MVCOL);
    unity_DOTS_Sampled_MCIALO = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _MCIALO);
    unity_DOTS_Sampled_TexturePatternStyle = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _TexturePatternStyle);
    unity_DOTS_Sampled_HighlightColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _HighlightColor);
    unity_DOTS_Sampled_HighlightColorPower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _HighlightColorPower);
    unity_DOTS_Sampled_EnableTextureTransparent = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _EnableTextureTransparent);
    unity_DOTS_Sampled_ReduSha = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ReduSha);
    //==

    //==  N_F_MC_ON
    unity_DOTS_Sampled_MCapIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _MCapIntensity);
    unity_DOTS_Sampled_SPECMODE = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SPECMODE);
    unity_DOTS_Sampled_SPECIN = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SPECIN);
    //==

    //==  N_F_TRANS_ON -> N_F_CO_ON
    unity_DOTS_Sampled_Cutout = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _Cutout);
    unity_DOTS_Sampled_AlphaBasedCutout = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _AlphaBasedCutout);
    unity_DOTS_Sampled_UseSecondaryCutoutOnly = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _UseSecondaryCutoutOnly);

    unity_DOTS_Sampled_Glow_Color = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _Glow_Color);
    unity_DOTS_Sampled_Glow_Edge_Width = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _Glow_Edge_Width);
    //==

    //==  N_F_TRANS_ON
    unity_DOTS_Sampled_Opacity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _Opacity);
    unity_DOTS_Sampled_TransparentThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _TransparentThreshold);
    unity_DOTS_Sampled_TOAO = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _TOAO);
    //==

    //== N_F_CA_ON
    unity_DOTS_Sampled_Saturation = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _Saturation);
    //==

    //== N_F_SL_ON
    unity_DOTS_Sampled_SelfLitIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfLitIntensity);
    unity_DOTS_Sampled_SelfLitColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _SelfLitColor);
    unity_DOTS_Sampled_SelfLitPower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfLitPower);
    unity_DOTS_Sampled_TEXMCOLINT = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _TEXMCOLINT);
    unity_DOTS_Sampled_SelfLitHighContrast = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfLitHighContrast);
    //==

    //== N_F_GLO_ON
    unity_DOTS_Sampled_GlossIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GlossIntensity);
    unity_DOTS_Sampled_Glossiness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _Glossiness);
    unity_DOTS_Sampled_GlossSoftness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GlossSoftness);
    unity_DOTS_Sampled_GlossColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _GlossColor);
    unity_DOTS_Sampled_GlossColorPower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GlossColorPower);
    //==

    //== N_F_GLO_ON -> N_F_GLOT_ON
    unity_DOTS_Sampled_GlossTextureSoftness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GlossTextureSoftness);
    unity_DOTS_Sampled_PSGLOTEX = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _PSGLOTEX);
    unity_DOTS_Sampled_GlossTextureRotate = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GlossTextureRotate);
    unity_DOTS_Sampled_GlossTextureFollowObjectRotation = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GlossTextureFollowObjectRotation);
    unity_DOTS_Sampled_GlossTextureFollowLight = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GlossTextureFollowLight);
    //==

    //== Others
    unity_DOTS_Sampled_OverallShadowColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _OverallShadowColor);
    unity_DOTS_Sampled_OverallShadowColorPower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _OverallShadowColorPower);
    unity_DOTS_Sampled_SelfShadowShadowTAtViewDirection = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfShadowShadowTAtViewDirection);
    unity_DOTS_Sampled_ShadowHardness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShadowHardness);
    unity_DOTS_Sampled_SelfShadowRealtimeShadowIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfShadowRealtimeShadowIntensity);
    //==

    //== Others
    unity_DOTS_Sampled_SelfShadowThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfShadowThreshold);
    unity_DOTS_Sampled_VertexColorGreenControlSelfShadowThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _VertexColorGreenControlSelfShadowThreshold);
    unity_DOTS_Sampled_SelfShadowHardness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfShadowHardness);
    unity_DOTS_Sampled_LigIgnoYNorDir = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _LigIgnoYNorDir);
    unity_DOTS_Sampled_SelfShadowAffectedByLightShadowStrength = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfShadowAffectedByLightShadowStrength);
    //==

    //== Others
    unity_DOTS_Sampled_SelfShadowRealTimeShadowColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _SelfShadowRealTimeShadowColor);
    unity_DOTS_Sampled_SelfShadowRealTimeShadowColorPower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SelfShadowRealTimeShadowColorPower);
    //==

    //== N_F_SO_ON
    unity_DOTS_Sampled_SmoothObjectNormal = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _SmoothObjectNormal);
    unity_DOTS_Sampled_VertexColorRedControlSmoothObjectNormal = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _VertexColorRedControlSmoothObjectNormal);
    unity_DOTS_Sampled_XYZPosition = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _XYZPosition);
    unity_DOTS_Sampled_ShowNormal = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShowNormal);
    //==

    //== N_F_SCT_ON
    unity_DOTS_Sampled_ShadowColorTexturePower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShadowColorTexturePower);
    //==

    //== N_F_ST_ON
    unity_DOTS_Sampled_ShadowTIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShadowTIntensity);
    unity_DOTS_Sampled_ShadowTLightThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShadowTLightThreshold);
    unity_DOTS_Sampled_ShadowTShadowThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShadowTShadowThreshold);
    unity_DOTS_Sampled_ShadowTColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _ShadowTColor);
    unity_DOTS_Sampled_ShadowTColorPower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShadowTColorPower);
    unity_DOTS_Sampled_ShadowTHardness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShadowTHardness);
    unity_DOTS_Sampled_STIL = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _STIL);
    unity_DOTS_Sampled_ShowInAmbientLightShadowIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShowInAmbientLightShadowIntensity);
    unity_DOTS_Sampled_ShowInAmbientLightShadowThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ShowInAmbientLightShadowThreshold);
    unity_DOTS_Sampled_LightFalloffAffectShadowT = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _LightFalloffAffectShadowT);
    //==

    //== N_F_PT_ON
    unity_DOTS_Sampled_PTCol = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _PTCol);
    unity_DOTS_Sampled_PTexturePower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _PTexturePower);
    //==

    //== N_F_RELGI_ON
    unity_DOTS_Sampled_GIFlatShade = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GIFlatShade);
    unity_DOTS_Sampled_GIShadeThreshold = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _GIShadeThreshold);
    unity_DOTS_Sampled_EnvironmentalLightingIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _EnvironmentalLightingIntensity);
    //==

    //== Others
    unity_DOTS_Sampled_LightAffectShadow = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _LightAffectShadow);
    unity_DOTS_Sampled_LightIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _LightIntensity);
    unity_DOTS_Sampled_DirectionalLightIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _DirectionalLightIntensity);
    unity_DOTS_Sampled_PointSpotlightIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _PointSpotlightIntensity);
    unity_DOTS_Sampled_ALIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ALIntensity);
    unity_DOTS_Sampled_ALTuFo = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ALTuFo);
    unity_DOTS_Sampled_LightFalloffSoftness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _LightFalloffSoftness);
    //==

    //==N_F_CLD_ON
    unity_DOTS_Sampled_CustomLightDirectionIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _CustomLightDirectionIntensity);
    unity_DOTS_Sampled_CustomLightDirection = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _CustomLightDirection);
    unity_DOTS_Sampled_CustomLightDirectionFollowObjectRotation = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _CustomLightDirectionFollowObjectRotation);
    //==

    //== N_F_R_ON
    unity_DOTS_Sampled_ReflectionIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ReflectionIntensity);
    unity_DOTS_Sampled_ReflectionRoughtness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _ReflectionRoughtness);
    unity_DOTS_Sampled_RefMetallic = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RefMetallic);
    //==

    //== N_F_RL_ON
    unity_DOTS_Sampled_RimLigInt = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RimLigInt);
    unity_DOTS_Sampled_RimLightUnfill = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RimLightUnfill);
    unity_DOTS_Sampled_RimLighThres = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RimLighThres);
    unity_DOTS_Sampled_RimLightSoftness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RimLightSoftness);
    unity_DOTS_Sampled_LightAffectRimLightColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _LightAffectRimLightColor);
    unity_DOTS_Sampled_RimLightColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _RimLightColor);
    unity_DOTS_Sampled_RimLightColorPower = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RimLightColorPower);
    unity_DOTS_Sampled_RimLightInLight = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RimLightInLight);
    //==

    //== N_F_O_ON
    unity_DOTS_Sampled_OutlineExtrudeMethod = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(int, _OutlineExtrudeMethod);
    unity_DOTS_Sampled_OutlineOffset = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float3, _OutlineOffset);
    unity_DOTS_Sampled_OutlineZPostionInCamera = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _OutlineZPostionInCamera);
    unity_DOTS_Sampled_OutlineColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _OutlineColor);
    unity_DOTS_Sampled_MixMainTexToOutline = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _MixMainTexToOutline);
    unity_DOTS_Sampled_NoisyOutlineIntensity = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _NoisyOutlineIntensity);
    unity_DOTS_Sampled_DynamicNoisyOutline = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _DynamicNoisyOutline);
    unity_DOTS_Sampled_LightAffectOutlineColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _LightAffectOutlineColor);
    unity_DOTS_Sampled_OutlineWidthAffectedByViewDistance = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _OutlineWidthAffectedByViewDistance);
    unity_DOTS_Sampled_FarDistanceMaxWidth = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _FarDistanceMaxWidth);
    unity_DOTS_Sampled_VertexColorBlueAffectOutlineWitdh = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _VertexColorBlueAffectOutlineWitdh);
    //==

    //== N_F_TP_ON
    unity_DOTS_Sampled_TriPlaTile = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _TriPlaTile);
    unity_DOTS_Sampled_TriPlaBlend = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _TriPlaBlend);
    //==

    //== N_F_NFD_ON
    unity_DOTS_Sampled_MinFadDistance = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _MinFadDistance);
    unity_DOTS_Sampled_MaxFadDistance = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _MaxFadDistance);
    //==

    //==Tessellation is still in development
    //unity_DOTS_Sampled_TessellationSmoothness = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(int, _TessellationSmoothness);
    //unity_DOTS_Sampled_TessellationTransition = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(int, _TessellationTransition);
    //unity_DOTS_Sampled_TessellationNear = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(int, _TessellationNear);
    //unity_DOTS_Sampled_TessellationFar = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(int, _TessellationFar);
    //==

    //== Others
    unity_DOTS_Sampled_SSAOColor = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _SSAOColor);
    unity_DOTS_Sampled_RTGIShaFallo = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RTGIShaFallo);
    unity_DOTS_Sampled_RecurRen = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _RecurRen);
    unity_DOTS_Sampled_AlphaRemapMin = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMin);
    unity_DOTS_Sampled_AlphaRemapMax = UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _AlphaRemapMax);
    //==

}




#undef UNITY_SETUP_DOTS_MATERIAL_PROPERTY_CACHES
#define UNITY_SETUP_DOTS_MATERIAL_PROPERTY_CACHES() SetupDOTSLitMaterialPropertyCaches()


//==  N_F_O_ON
#define _OutlineWidth                                       unity_DOTS_Sampled_OutlineWidth
#define _ONormMapInt                                        unity_DOTS_Sampled_ONormMapInt
//==

//==  N_F_O_SSOL
#define _DepthThreshold                                     unity_DOTS_Sampled_DepthThreshold
#define _NormalThreshold                                    unity_DOTS_Sampled_NormalThreshold
#define _NormalMin                                          unity_DOTS_Sampled_NormalMin
#define _NormalMax                                          unity_DOTS_Sampled_NormalMax
//==

//== Others
#define _MainColor                                          unity_DOTS_Sampled_MainColor
#define _MaiColPo										    unity_DOTS_Sampled_MaiColPo
#define _MVCOL                                              unity_DOTS_Sampled_MVCOL
#define _MCIALO                                             unity_DOTS_Sampled_MCIALO
#define _TexturePatternStyle                                unity_DOTS_Sampled_TexturePatternStyle
#define _HighlightColor                                     unity_DOTS_Sampled_HighlightColor
#define _HighlightColorPower                                unity_DOTS_Sampled_HighlightColorPower
#define _EnableTextureTransparent                           unity_DOTS_Sampled_EnableTextureTransparent
#define _ReduSha                                            unity_DOTS_Sampled_ReduSha
//==

//==  N_F_MC_ON
#define _MCapIntensity                                      unity_DOTS_Sampled_MCapIntensity
#define _SPECMODE                                           unity_DOTS_Sampled_SPECMODE
#define _SPECIN                                             unity_DOTS_Sampled_SPECIN
//==

//==  N_F_TRANS_ON -> N_F_CO_ON
#define _Cutout                                             unity_DOTS_Sampled_Cutout
#define _AlphaBasedCutout                                   unity_DOTS_Sampled_AlphaBasedCutout
#define _UseSecondaryCutoutOnly                             unity_DOTS_Sampled_UseSecondaryCutoutOnly

#define _Glow_Color										    unity_DOTS_Sampled_Glow_Color
#define _Glow_Edge_Width								    unity_DOTS_Sampled_Glow_Edge_Width
//==

//==  N_F_TRANS_ON
#define _Opacity                                            unity_DOTS_Sampled_Opacity
#define _TransparentThreshold                               unity_DOTS_Sampled_TransparentThreshold
#define _TOAO                                               unity_DOTS_Sampled_TOAO
//==

//== N_F_CA_ON
#define _Saturation                                         unity_DOTS_Sampled_Saturation
//==

//== N_F_SL_ON
#define _SelfLitIntensity                                   unity_DOTS_Sampled_SelfLitIntensity
#define _SelfLitColor                                       unity_DOTS_Sampled_SelfLitColor
#define _SelfLitPower                                       unity_DOTS_Sampled_SelfLitPower
#define _TEXMCOLINT                                         unity_DOTS_Sampled_TEXMCOLINT
#define _SelfLitHighContrast                                unity_DOTS_Sampled_SelfLitHighContrast
//==

//== N_F_GLO_ON
#define _GlossIntensity                                     unity_DOTS_Sampled_GlossIntensity
#define _Glossiness                                         unity_DOTS_Sampled_Glossiness
#define _GlossSoftness                                      unity_DOTS_Sampled_GlossSoftness
#define _GlossColor                                         unity_DOTS_Sampled_GlossColor
#define _GlossColorPower                                    unity_DOTS_Sampled_GlossColorPower
//==

//== N_F_GLO_ON -> N_F_GLOT_ON
#define _GlossTextureSoftness                               unity_DOTS_Sampled_GlossTextureSoftness
#define _PSGLOTE                                            unity_DOTS_Sampled_PSGLOTEX
#define _GlossTextureRotate                                 unity_DOTS_Sampled_GlossTextureRotate
#define _GlossTextureFollowObjectRotation                   unity_DOTS_Sampled_GlossTextureFollowObjectRotation
#define _GlossTextureFollowLight                            unity_DOTS_Sampled_GlossTextureFollowLight
//==

//== Others
#define _OverallShadowColor                                 unity_DOTS_Sampled_OverallShadowColor
#define _OverallShadowColorPower                            unity_DOTS_Sampled_OverallShadowColorPower
#define _SelfShadowShadowTAtViewDirection                   unity_DOTS_Sampled_SelfShadowShadowTAtViewDirection
#define _ShadowHardness                                     unity_DOTS_Sampled_ShadowHardness
#define _SelfShadowRealtimeShadowIntensity                  unity_DOTS_Sampled_SelfShadowRealtimeShadowIntensity
//==

//== Others
#define _SelfShadowThreshold                                unity_DOTS_Sampled_SelfShadowThreshold
#define _VertexColorGreenControlSelfShadowThreshold         unity_DOTS_Sampled_VertexColorGreenControlSelfShadowThreshold
#define _SelfShadowHardness                                 unity_DOTS_Sampled_SelfShadowHardness
#define _LigIgnoYNorDir									    unity_DOTS_Sampled_LigIgnoYNorDir
#define _SelfShadowAffectedByLightShadowStrength            unity_DOTS_Sampled_SelfShadowAffectedByLightShadowStrength
//==

//== Others
#define _SelfShadowRealTimeShadowColor                      unity_DOTS_Sampled_SelfShadowRealTimeShadowColor
#define _SelfShadowRealTimeShadowColorPower                 unity_DOTS_Sampled_SelfShadowRealTimeShadowColorPower
//==

//== N_F_SO_ON
#define _SmoothObjectNormal                                 unity_DOTS_Sampled_SmoothObjectNormal
#define _VertexColorRedControlSmoothObjectNormal            unity_DOTS_Sampled_VertexColorRedControlSmoothObjectNormal
#define _XYZPosition                                        unity_DOTS_Sampled_XYZPosition
#define _ShowNormal                                         unity_DOTS_Sampled_ShowNormal
//==

//== N_F_SCT_ON
#define _ShadowColorTexturePower                            unity_DOTS_Sampled_ShadowColorTexturePower
//==

//== N_F_ST_ON
#define _ShadowTIntensity                                   unity_DOTS_Sampled_ShadowTIntensity
#define _ShadowTLightThreshol                               unity_DOTS_Sampled_ShadowTLightThreshold
#define _ShadowTShadowThreshold                             unity_DOTS_Sampled_ShadowTShadowThreshold
#define _ShadowTColor                                       unity_DOTS_Sampled_ShadowTColor
#define _ShadowTColorPower                                  unity_DOTS_Sampled_ShadowTColorPower
#define _ShadowTHardness                                    unity_DOTS_Sampled_ShadowTHardness
#define _STIL                                               unity_DOTS_Sampled_STIL
#define _ShowInAmbientLightShadowIntensity                  unity_DOTS_Sampled_ShowInAmbientLightShadowIntensity
#define _ShowInAmbientLightShadowThreshold                  unity_DOTS_Sampled_ShowInAmbientLightShadowThreshold
#define _LightFalloffAffectShadowT                          unity_DOTS_Sampled_LightFalloffAffectShadowT
//==

//== N_F_PT_ON
#define _PTCol                                              unity_DOTS_Sampled_PTCol
#define _PTexturePower                                      unity_DOTS_Sampled_PTexturePower
//==

//== N_F_RELGI_ON
#define _GIFlatShade                                        unity_DOTS_Sampled_GIFlatShade
#define _GIShadeThreshold                                   unity_DOTS_Sampled_GIShadeThreshold
#define _EnvironmentalLightingIntensity                     unity_DOTS_Sampled_EnvironmentalLightingIntensity
//==

//== Others
#define _LightAffectShadow                                  unity_DOTS_Sampled_LightAffectShadow
#define _LightIntensity                                     unity_DOTS_Sampled_LightIntensity
#define _DirectionalLightIntensity                          unity_DOTS_Sampled_DirectionalLightIntensity
#define _PointSpotlightIntensity                            unity_DOTS_Sampled_PointSpotlightIntensity
#define _ALIntensity                                        unity_DOTS_Sampled_ALIntensity
#define __ALTuFo                                            unity_DOTS_Sampled_ALTuFo
#define _LightFalloffSoftness                               unity_DOTS_Sampled_LightFalloffSoftness
//==

//==N_F_CLD_ON
#define _CustomLightDirectionIntensity                      unity_DOTS_Sampled_CustomLightDirectionIntensity
#define _CustomLightDirection                               unity_DOTS_Sampled_CustomLightDirection
#define _CustomLightDirectionFollowObjectRotation           unity_DOTS_Sampled_CustomLightDirectionFollowObjectRotation
//==

//== N_F_R_ON
#define _ReflectionIntensity                                unity_DOTS_Sampled_ReflectionIntensity
#define _ReflectionRoughtness                               unity_DOTS_Sampled_ReflectionRoughtness
#define _RefMetallic                                        unity_DOTS_Sampled_RefMetallic
//==

//== N_F_RL_ON
#define _RimLigInt                                          unity_DOTS_Sampled_RimLigInt
#define _RimLightUnfill                                     unity_DOTS_Sampled_RimLightUnfill
#define _RimLighThres                                       unity_DOTS_Sampled_RimLighThres
#define _RimLightSoftness                                   unity_DOTS_Sampled_RimLightSoftness
#define _LightAffectRimLightColor                           unity_DOTS_Sampled_LightAffectRimLightColor
#define _RimLightColor                                      unity_DOTS_Sampled_RimLightColor
#define _RimLightColorPower                                 unity_DOTS_Sampled_RimLightColorPower
#define _RimLightInLight                                    unity_DOTS_Sampled_RimLightInLight
//==

//== N_F_O_ON
#define _OutlineExtrudeMethod                               unity_DOTS_Sampled_OutlineExtrudeMethod
#define _OutlineOffset                                      unity_DOTS_Sampled_OutlineOffset
#define _OutlineZPostionInCamera                            unity_DOTS_Sampled_OutlineZPostionInCamera
#define _OutlineColor                                       unity_DOTS_Sampled_OutlineColor
#define _MixMainTexToOutline                                unity_DOTS_Sampled_MixMainTexToOutline
#define _NoisyOutlineIntensity                              unity_DOTS_Sampled_NoisyOutlineIntensity
#define _DynamicNoisyOutline                                unity_DOTS_Sampled_DynamicNoisyOutline
#define _LightAffectOutlineColor                            unity_DOTS_Sampled_LightAffectOutlineColor
#define _OutlineWidthAffectedByViewDistance                 unity_DOTS_Sampled_OutlineWidthAffectedByViewDistance
#define _FarDistanceMaxWidth                                unity_DOTS_Sampled_FarDistanceMaxWidth
#define _VertexColorBlueAffectOutlineWitdh                  unity_DOTS_Sampled_VertexColorBlueAffectOutlineWitdh
//==

//== N_F_NFD_ON
#define _MinFadDistance                                     unity_DOTS_Sampled_MinFadDistance
#define _MaxFadDistance                                     unity_DOTS_Sampled_MaxFadDistance
//==

//== N_F_TP_ON
#define _TriPlaTile                                         unity_DOTS_Sampled_TriPlaTile
#define _TriPlaBlend                                        unity_DOTS_Sampled_TriPlaBlend
//==

//==Tessellation is still in development
// #define _TessellationSmoothness                          unity_DOTS_Sampled_TessellationSmoothness
// #define _TessellationTransition                          unity_DOTS_Sampled_TessellationTransition
// #define _TessellationNear                                unity_DOTS_Sampled_TessellationNear
// #define _TessellationFar                                 unity_DOTS_Sampled_TessellationFar
//==

//== Others
#define _SSAOColor                                          unity_DOTS_Sampled_SSAOColor
#define _RTGIShaFallo                                       unity_DOTS_Sampled_RTGIShaFallo
#define _RecurRen                                           unity_DOTS_Sampled_RecurRen
#define _AlphaRemapMin                                      unity_DOTS_Sampled_AlphaRemapMin
#define _AlphaRemapMax                                      unity_DOTS_Sampled_AlphaRemapMax
//==

#endif

//===============================================================================
//Non CBUF
//===============================================================================

//==
//RealToon HDRP - Non CBUF
//==

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

TEXTURE2D(_OutlineWidthControl);
SAMPLER(sampler_OutlineWidthControl);

#if N_F_O_NM_ON
    TEXTURE2D(_ONormMap);
    SAMPLER(sampler_ONormMap);
#endif

#if N_F_MC_ON

    TEXTURE2D(_MCap);
    SAMPLER(sampler_MCap);

    TEXTURE2D(_MCapMask);
    SAMPLER(sampler_MCapMask);

#endif

#if N_F_TRANS_ON

    #if N_F_CO_ON

        TEXTURE2D(_SecondaryCutout);
        SAMPLER(sampler_SecondaryCutout);

    #else

        TEXTURE2D(_MaskTransparency);
        SAMPLER(sampler_MaskTransparency);

    #endif

#endif

#if N_F_SL_ON

    TEXTURE2D(_MaskSelfLit);
    SAMPLER(sampler_MaskSelfLit);

#endif

#if N_F_GLO_ON

    TEXTURE2D(_MaskGloss);
    SAMPLER(sampler_MaskGloss);

#endif

#if N_F_GLO_ON

    #if N_F_GLOT_ON

        TEXTURE2D(_GlossTexture);
        SAMPLER(sampler_GlossTexture);

    #endif

#endif

#if N_F_SCT_ON

    TEXTURE2D(_ShadowColorTexture);
    SAMPLER(sampler_ShadowColorTexture);

#endif

#if N_F_ST_ON

    TEXTURE2D(_ShadowT);
    SAMPLER(sampler_ShadowT);

#endif

#if N_F_PT_ON

    TEXTURE2D(_PTexture);
    SAMPLER(sampler_PTexture);

#endif

#if N_F_R_ON

    TEXTURE2D(_MaskReflection);
    SAMPLER(sampler_MaskReflection);

#endif

#if N_F_R_ON

    #if N_F_FR_ON

        TEXTURE2D(_FReflection);
        SAMPLER(sampler_FReflection);

    #endif

#endif


//==
//Unity HDRP Standard Prop - Non CBUF
//==

TEXTURE2D(_DistortionVectorMap);
SAMPLER(sampler_DistortionVectorMap);

TEXTURE2D(_BaseColorMap);
SAMPLER(sampler_BaseColorMap);

TEXTURE2D(_NormalMap);
SAMPLER(sampler_NormalMap);

TEXTURE2D(_HeightMap);
SAMPLER(sampler_HeightMap);

int _ObjectId;
int _PassValue;
float4 _SelectionID;




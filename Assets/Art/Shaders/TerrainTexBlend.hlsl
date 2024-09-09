

int MaxIdx(float3 v) {
    float a=v.x;
    float b=v.y;
    float c=v.z;
    return a > b ? (a > c ? 0 : 2) : (b > c ? 1 : 2);
}

float mod(float v, float n)
{
	float f = v % n;
	return f < 0 ? f + n : f;
	//return v-n*floor(v/n);
}
	
float4 TexTriplanar(UnityTexture2D tex, float3 p, float TexId, float3 blendWeights, float TexCap) 
{
	float E = 0.02 / TexCap;  // intoduce Epsilon to fix Mipmap Error (and Float-point Error) on Tex Boundary
	float TexSizeX = 1.0 / TexCap;
	float TexPosX  = TexId / TexCap;	
	float2 uvX = float2(mod(p.z * TexSizeX, TexSizeX-E*2) + TexPosX+E, p.y);
	float2 uvY = float2(mod(p.x * TexSizeX, TexSizeX-E*2) + TexPosX+E, p.z);
	float2 uvZ = float2(mod(p.x * TexSizeX, TexSizeX-E*2) + TexPosX+E, p.y);

	//SAMPLE_TEXTURE2D(tex, TexSampleState, uvX) * weights.x +
    return tex2D(tex, uvX) * blendWeights.x +
           tex2D(tex, uvY) * blendWeights.y +
           tex2D(tex, uvZ) * blendWeights.z;
}

void TexBlend_float(
	float3 TexIds,
	float3 WorldPos,
	float3 WorldNorm,
	float3 BaryCoord,
	UnityTexture2D TexDiff,
	UnityTexture2D TexNorm,
	UnityTexture2D TexDRAM,
	UnitySamplerState TexSamplerState,
	float TexScale,
	float TexCount,
	float TexTriplanarBlendPow,
	float TexHeightmapBlendPow, 
	float TexIdOffset,

	out float3 outAlbedo,
	out float3 outNormal, 
	out float outMetallic,
	out float outSmoothness,
	out float3 outEmission,
	out float outAO)
{
	// int idxMaxBary = MaxIdx(BaryCoord);
	//int idxMaxNorm = MaxIdx(WorldNorm);
	
    // use Norm AbsVal as weights
    float3 BlendTrip = pow(abs(WorldNorm), TexTriplanarBlendPow);
    BlendTrip /= dot(BlendTrip, 1.0);  // make sure the weights sum up to 1 (divide by sum of x+y+z)

	TexIds += TexIdOffset;

	float3 PosTrip = WorldPos / TexScale;

	float4 triDRAM[3] = { 
		TexTriplanar(TexDRAM, PosTrip, TexIds[0], BlendTrip, TexCount),
		TexTriplanar(TexDRAM, PosTrip, TexIds[1], BlendTrip, TexCount),
		TexTriplanar(TexDRAM, PosTrip, TexIds[2], BlendTrip, TexCount),
	};
	
	float3 _bhm = pow(abs(BaryCoord), TexHeightmapBlendPow);  // BlendHeightmap. Pow: littler=mix, greater=distinct, opt 0.3 - 0.6, 0.48 = nature
	int idxMaxHigh = MaxIdx(float3(triDRAM[0].x * _bhm.x, triDRAM[1].x * _bhm.y, triDRAM[2].x * _bhm.z));
	// int idxVertTex = idxMaxHigh;  // triangle vertex idx of Current Frag Mtl. usually = i_MaxBary, or i_MaxHigh

	float4 DRAM = triDRAM[idxMaxHigh];

	outAlbedo = 
	TexTriplanar(TexDiff, PosTrip, TexIds[idxMaxHigh], BlendTrip, TexCount).xyz;
					 
	outNormal =
	TexTriplanar(TexNorm, PosTrip, TexIds[idxMaxHigh], BlendTrip, TexCount).xyz;

	
	outEmission = 0;
	outSmoothness = 1.0 - DRAM.y;
	outAO = DRAM.z;
	outMetallic = 0;
}
#ifndef LIGHTWEIGHT_SIMPLE_ANISTROPY_INPUT_INCLUDED
#define LIGHTWEIGHT_SIMPLE_ANISTROPY_INPUT_INCLUDED

//#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
//#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half4 _BaseColor;
half4 _SpecColor;
half4 _EmissionColor;
half _Cutoff;
half _AnistropyShift;
half _Smoothness;


half _RimPower;
half _Denier;
//float _Smoothness;
//float _Metallic;
half _FresnelScale;
half _Fresnel0;
half _Fresnel1;
float4 _SkinTint;
float4 _StockingTint;
half _FactorML;
half _FactorGI;

//sampler2D _DenierTex;
//sampler2D _SkinTex;
//sampler2D _StockingTex;
// 
//sampler2D _NormalTex;

CBUFFER_END
TEXTURE2D(_DenierTex);       SAMPLER(sampler_DenierTex);
//TEXTURE2D(_SkinTex);       SAMPLER(sampler_SkinTex);
TEXTURE2D(_StockingTex);       SAMPLER(sampler_StockingTex);

TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);


half4 SampleSpecularMaskShift(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
{
    half4 SpecularMask_Shift = half4(0.0h, 0.0h, 0.0h, 1.0h);
#ifdef _SPECGLOSSMAP
	SpecularMask_Shift = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv);
#elif defined(_SPECULAR_COLOR)
	SpecularMask_Shift = half4(1.0,0.0,0.0,1.0);
#endif
    return SpecularMask_Shift;
}

#endif
#ifndef UNIVERSAL_SIMPLE_LIT_INPUT_INCLUDED
#define UNIVERSAL_SIMPLE_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
    float4 _BaseMap_ST;
    half4 _BaseColor;
    half4 _SpecColor;
    //half4 _EmissionColor;
    half _Cutoff;
    half _Surface;
    half _Occ;
    //half _Exposure;
    //half _Tonemap;
    //half _BumpScale;
    //half _NormalBlurredBias;
    ////half4 _CurvatureTex_LUT_Color;
    //half4 _ShadowTex_LUT_Color;

    float _BumpScale;
    /*half4 _Color;
    half4 _Specular;
    half4 _tuneNormalBlur;*/
    float _Brightness;
    float _Roughness;
    float _CurveFactor;
    half _LutRemap0;
    half _LutRemap1;
    half _SpcRemap0;
    half _SpcRemap1;
    half _Alpha0;
    half _Alpha1;
    half _Shadow0;
    half _Shadow1;
CBUFFER_END

#ifdef UNITY_DOTS_INSTANCING_ENABLED
    UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
        UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
        UNITY_DOTS_INSTANCED_PROP(float4, _SpecColor)
        //UNITY_DOTS_INSTANCED_PROP(float4, _EmissionColor)
        UNITY_DOTS_INSTANCED_PROP(float , _Cutoff)
        UNITY_DOTS_INSTANCED_PROP(float , _Surface)
        
        //UNITY_DOTS_INSTANCED_PROP(float, _Exposure)
        //UNITY_DOTS_INSTANCED_PROP(float, _Tonemap)
        //UNITY_DOTS_INSTANCED_PROP(float, _BumpScale)
        //UNITY_DOTS_INSTANCED_PROP(float, _NormalBlurredBias)
        ////UNITY_DOTS_INSTANCED_PROP(float4, _CurvatureTex_LUT_Color)
        //UNITY_DOTS_INSTANCED_PROP(float4, _ShadowTex_LUT_Color)
    UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

    #define _BaseColor          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__BaseColor)
    #define _SpecColor          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__SpecColor)
    //#define _EmissionColor      UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__EmissionColor)
    #define _Cutoff             UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata__Cutoff)
    #define _Surface            UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata__Surface)

    //#define _Exposure            UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata__Exposure)
    //#define _Tonemap            UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata__Tonemap)
    //#define _BumpScale            UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata__BumpScale)
    //#define _NormalBlurredBias            UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata__NormalBlurredBias)
    ////#define _CurvatureTex_LUT_Color          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__CurvatureTex_LUT_Color)
    //#define _ShadowTex_LUT_Color          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__ShadowTex_LUT_Color)
#endif

TEXTURE2D(_OccGlossMap);       SAMPLER(sampler_OccGlossMap);
//TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
//TEXTURE2D(_ShadowTex_LUT_Map);       SAMPLER(sampler_ShadowTex_LUT_Map);
TEXTURE2D(_SpecularLUT);           SAMPLER(sampler_SpecularLUT);
//TEXTURE2D(_SpecularLUT);           SamplerState sampler_linear_repeat_SpecularLUT;
TEXTURE2D(_CurveTex);            SAMPLER(sampler_CurveTex);
TEXTURE2D(_SSSLUT);            SAMPLER(sampler_SSSLUT);

//half4 SampleSpecularSmoothness(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
//{
//    half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
//#ifdef _SPECGLOSSMAP
//    specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
//#elif defined(_SPECULAR_COLOR)
//    specularSmoothness = specColor;
//#endif
//
//#ifdef _GLOSSINESS_FROM_BASE_ALPHA
//    specularSmoothness.a = exp2(10 * alpha + 1);
//#else
//    specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
//#endif
//
//    return specularSmoothness;
//}
float3 RGBConvertToHSV(float3 rgb)
{
    float R = rgb.x, G = rgb.y, B = rgb.z;
    float3 hsv;
    float max1 = max(R, max(G, B));
    float min1 = min(R, min(G, B));
    if (R == max1)
    {
        hsv.x = (G - B) / (max1 - min1);
    }
    if (G == max1)
    {
        hsv.x = 2 + (B - R) / (max1 - min1);
    }
    if (B == max1)
    {
        hsv.x = 4 + (R - G) / (max1 - min1);
    }
    hsv.x = hsv.x * 60.0;
    if (hsv.x < 0)
        hsv.x = hsv.x + 360;
    hsv.z = max1;
    hsv.y = (max1 - min1) / max1;
    return hsv;
}
inline void InitializeSimpleLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    outSurfaceData = (SurfaceData)0;

    half4 albedoAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.alpha = albedoAlpha.a * _BaseColor.a;
    AlphaDiscard(outSurfaceData.alpha, _Cutoff);

    outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
#ifdef _ALPHAPREMULTIPLY_ON
    outSurfaceData.albedo *= outSurfaceData.alpha;
#endif

    //half4 specularSmoothness = SampleSpecularSmoothness(uv, outSurfaceData.alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
    outSurfaceData.metallic = 0.0; // unused
    outSurfaceData.specular = 1.0;// specularSmoothness.rgb;
    outSurfaceData.smoothness = 1.0;//specularSmoothness.a;
    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
    outSurfaceData.occlusion = 1.0; // unused
    outSurfaceData.emission = 0.0f;// SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

    //SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb;
}

#endif

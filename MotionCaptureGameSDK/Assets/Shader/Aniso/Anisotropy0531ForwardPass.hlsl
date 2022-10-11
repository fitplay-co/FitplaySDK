#ifndef ANISTROPY_0531_PASS_INCLUDED
#define ANISTROPY_0531_PASS_INCLUDED

//#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
//#include "Assets/ThirdParty/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


struct Attributes
{
    float4 positionOS    : POSITION;
    float3 normalOS      : NORMAL;
    float4 tangentOS     : TANGENT;
    float2 texcoord      : TEXCOORD0;
    float2 staticLightmapUV    : TEXCOORD1;
    float2 dynamicLightmapUV    : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv                       : TEXCOORD0;

    float3 positionWS                  : TEXCOORD1;    // xyz: posWS

    //#ifdef _NORMALMAP
        half4 normalWS                 : TEXCOORD2;    // xyz: normal, w: viewDir.x
        half4 tangentWS                : TEXCOORD3;    // xyz: tangent, w: viewDir.y
        half4 bitangentWS              : TEXCOORD4;    // xyz: bitangent, w: viewDir.z
    //#else
    //    half3  normalWS                : TEXCOORD2;
    //#endif

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        half4 fogFactorAndVertexLight  : TEXCOORD5; // x: fogFactor, yzw: vertex light
    #else
        half  fogFactor                 : TEXCOORD5;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        float4 shadowCoord             : TEXCOORD6;
    #endif

    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 7);

#ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV : TEXCOORD8; // Dynamic lightmap UVs
#endif

    float4 positionCS                  : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
{
    inputData = (InputData)0;

    inputData.positionWS = input.positionWS;

    //#ifdef _NORMALMAP
        half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
        inputData.tangentToWorld = half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz);
        inputData.normalWS = TransformTangentToWorld(normalTS, inputData.tangentToWorld);
    //#else
    //    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(inputData.positionWS);
    //    inputData.normalWS = input.normalWS;
    //#endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
    viewDirWS = SafeNormalize(viewDirWS);

    inputData.viewDirectionWS = viewDirWS;

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        inputData.shadowCoord = input.shadowCoord;
    #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
        inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
    #else
        inputData.shadowCoord = float4(0, 0, 0, 0);
    #endif

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        inputData.fogCoord = InitializeInputDataFog(float4(inputData.positionWS, 1.0), input.fogFactorAndVertexLight.x);
        inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
    #else
        inputData.fogCoord = InitializeInputDataFog(float4(inputData.positionWS, 1.0), input.fogFactor);
        inputData.vertexLighting = half3(0, 0, 0);
    #endif

#if defined(DYNAMICLIGHTMAP_ON)
    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.dynamicLightmapUV, input.vertexSH, inputData.normalWS);
#else
    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.normalWS);
#endif

    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    inputData.shadowMask = 1;// SAMPLE_SHADOWMASK(input.staticLightmapUV);

    #if defined(DEBUG_DISPLAY)
    #if defined(DYNAMICLIGHTMAP_ON)
    inputData.dynamicLightmapUV = input.dynamicLightmapUV.xy;
    #endif
    #if defined(LIGHTMAP_ON)
    inputData.staticLightmapUV = input.staticLightmapUV;
    #else
    inputData.vertexSH = input.vertexSH;
    #endif
    #endif
}

//float3  positionWS;
//half3   normalWS;
//half3   viewDirectionWS;
//float4  shadowCoord;
//half    fogCoord;
//half3   vertexLighting;
//half3   bakedGI;
//float2  normalizedScreenSpaceUV;
//half4   shadowMask;

half3 ShiftTangent1(half3 T, half3 N, half shift)
{
    half3 shiftedT = T + shift * N;
    return SafeNormalize(shiftedT);
}

half StrandSpecular(half3 T, half3 V, half3 L, half exponent)
{
	half3 H = normalize(L + V);
	float dotTH = dot(T, H);
	float sinTH = sqrt(1 - (dotTH * dotTH));
	float dirAtten = smoothstep(-1, 0, dotTH);
	return dirAtten * pow(sinTH, exponent);
}

half3 AnistropySpecular (half3 lightColor, half3 lightDir, half3 normal, half3 tangent, half3 viewDir, half4 specularColor, half smoothness,half shift)
{
    //half secondaryShift = 0.3;
    // shift tangents
    half3 t1 = ShiftTangent1(tangent, normal, _AnistropyShift + shift);
    //half3 t2 = ShiftTangent(tangent, normal, _AnistropyShift + secondaryShift);

    //half specExp2 = smoothness;
    //half3 specularColor2 = specularColor.rgb * 0.5;
    // specular lighting
    half3 specular = specularColor.rgb * StrandSpecular(t1, viewDir, lightDir, smoothness);
    // add second specular term
    //half specMask = 1;//tex2D(tSpecMask, uv); 
    //specular += specularColor2 * specMask * StrandSpecular(t2, viewDir, lightDir, specExp2);

    return specular * lightColor * saturate(dot(normal,lightDir))* _AnistropyScale;
}
half3 LightingLambert2(half3 lightColor, half3 lightDir, half3 normal)
{
    half NdotL = saturate(dot(normal, lightDir));
    //return lightColor * NdotL;
    return lerp(_LambertRemap0, _LambertRemap1, lightColor * NdotL);
}
half4 LightweightFragmentAnistropyBlinnPhong(InputData inputData, half3 diffuse, half4 specularGloss, half smoothness, half3 normalTS,half3 btangent,half shift,half alpha)
{
    Light mainLight = GetMainLight(inputData.shadowCoord);
    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));

    half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
    half3 diffuseColor = inputData.bakedGI + LightingLambert2(attenuatedLightColor, mainLight.direction, inputData.normalWS);
    half3 specularColor = AnistropySpecular(attenuatedLightColor, mainLight.direction,inputData.normalWS, btangent, inputData.viewDirectionWS, specularGloss, smoothness,shift);

#ifdef _ADDITIONAL_LIGHTS
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, inputData.positionWS);
        half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        diffuseColor += LightingLambert2(attenuatedLightColor, light.direction, inputData.normalWS);
        specularColor += AnistropySpecular(attenuatedLightColor, -light.direction, inputData.normalWS, normalTS, inputData.viewDirectionWS, specularGloss, smoothness, shift);
    }
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX
    diffuseColor += inputData.vertexLighting;
#endif

    half3 finalColor = diffuseColor* diffuse;

#if defined(_SPECGLOSSMAP) || defined(_SPECULAR_COLOR)
    finalColor += specularColor;
#endif

    return half4(finalColor, alpha);
}

///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////

// Used in Standard (Simple Lighting) shader
Varyings LitPassVertexSimple(Attributes input)
{
    Varyings output = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

#if defined(_FOG_FRAGMENT)
        half fogFactor = 0;
#else
        half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
#endif

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.positionWS.xyz = vertexInput.positionWS;
    output.positionCS = vertexInput.positionCS;

//#ifdef _NORMALMAP
    half3 viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
    output.normalWS = half4(normalInput.normalWS, viewDirWS.x);
    output.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);
//#else
//    output.normalWS = NormalizeNormalPerVertex(normalInput.normalWS);
//#endif

    OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
#ifdef DYNAMICLIGHTMAP_ON
    output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif
    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
        output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
    #else
        output.fogFactor = fogFactor;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        output.shadowCoord = GetShadowCoord(vertexInput);
    #endif

    return output;
}

// Used for StandardSimpleLighting shader
half4 LitPassFragmentSimple(Varyings input) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    //SurfaceData surfaceData;
    //InitializeSimpleLitSurfaceData(input.uv, surfaceData);

    //InputData inputData;
    //InitializeInputData(input, surfaceData.normalTS, inputData);
    //SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);
    //////////////////////////////////////////////////////////////////
//    SurfaceData surfaceData;
//    InitializeSimpleLitSurfaceData(input.uv, surfaceData);

//    InputData inputData;
//    InitializeInputData(input, surfaceData.normalTS, inputData);
//    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);

//#ifdef _DBUFFER
    //ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
//#endif

    float2 uv = input.uv;
	//"Specular (R) Spec Shift (G) Spec Mask (B)"
    half4 rgba = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));

	half3 diffuse = rgba.rgb * _BaseColor.rgb;
    half alpha = rgba.a * _BaseColor.a;
    AlphaDiscard(alpha, _Cutoff);
#ifdef _ALPHAPREMULTIPLY_ON
    diffuse *= alpha;
#endif

    half3 normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
    //half3 emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

    half4 maskShift = SampleSpecularMaskShift(uv, alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
	half4 specular = _SpecColor;
    half smoothness = exp2(10 * _Smoothness + 1);
	half shift = maskShift.g - 0.5;

    //shift = 1;

    InputData inputData;
    InitializeInputData(input, normalTS, inputData);

	half3 btangent = -normalize(cross(inputData.normalWS.xyz, input.tangentWS.xyz));
    //half4 color = half4(inputData.normalWS.xyz,1);
    //color = 0.5;
    half4 color = LightweightFragmentAnistropyBlinnPhong(inputData, diffuse, specular, smoothness, inputData.normalWS.xyz, btangent, shift,alpha);
    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    color.a = OutputAlpha(color.a, _Surface);
    return color;
};

#endif

#ifndef ANISTROPY_0531_PASS_INCLUDED
#define ANISTROPY_0531_PASS_INCLUDED

//#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


struct Attributes
{
    float4 positionOS    : POSITION;
    float3 normalOS      : NORMAL;
    float4 tangentOS     : TANGENT;
    float2 texcoord      : TEXCOORD0;
    float2 lightmapUV    : TEXCOORD1;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv                       : TEXCOORD0;
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

    float3 posWS                    : TEXCOORD2;    // xyz: posWS

#ifdef _NORMALMAP
    half4 normal                    : TEXCOORD3;    // xyz: normal, w: viewDir.x
    half4 tangent                   : TEXCOORD4;    // xyz: tangent, w: viewDir.y
    half4 bitangent                  : TEXCOORD5;    // xyz: bitangent, w: viewDir.z
#else
    half3  normal                   : TEXCOORD3;
    half3 viewDir                   : TEXCOORD4;
#endif

    half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light

#ifdef _MAIN_LIGHT_SHADOWS
    float4 shadowCoord              : TEXCOORD7;
#endif

    float4 positionCS               : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
{
    inputData.positionWS = input.posWS;

#ifdef _NORMALMAP
    half3 viewDirWS = half3(input.normal.w, input.tangent.w, input.bitangent.w);
    inputData.normalWS = TransformTangentToWorld(normalTS,
        half3x3(input.tangent.xyz, input.bitangent.xyz, input.normal.xyz));
#else
    half3 viewDirWS = input.viewDir;
    inputData.normalWS = input.normal;
#endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
    viewDirWS = SafeNormalize(viewDirWS);

    inputData.viewDirectionWS = viewDirWS;
#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
    inputData.shadowCoord = input.shadowCoord;
#else
    inputData.shadowCoord = float4(0, 0, 0, 0);
#endif
    inputData.fogCoord = input.fogFactorAndVertexLight.x;
    inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
    inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    inputData.shadowMask = 0;// SAMPLE_SHADOWMASK(input.lightmapUV);
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
half3 ShiftTangent(half3 T, half3 N, half shift)
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
    half3 t1 = ShiftTangent(tangent, normal, _AnistropyShift + shift);
    //half3 t2 = ShiftTangent(tangent, normal, _AnistropyShift + secondaryShift);

    //half specExp2 = smoothness;
    //half3 specularColor2 = specularColor.rgb * 0.5;
    // specular lighting
    half3 specular = specularColor.rgb * StrandSpecular(t1, viewDir, lightDir, smoothness);
    // add second specular term
    //half specMask = 1;//tex2D(tSpecMask, uv); 
    //specular += specularColor2 * specMask * StrandSpecular(t2, viewDir, lightDir, specExp2);

    return specular * lightColor * saturate(dot(normal,lightDir));
}

half4 LightweightFragmentAnistropyBlinnPhong(InputData inputData, half3 diffuse, half4 specularGloss, half smoothness, half3 normalTS,half3 btangent,half shift,half alpha,half4 stockingColor)
{
    Light mainLight = GetMainLight(inputData.shadowCoord);
    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));

    half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
    half3 diffuseColor = inputData.bakedGI + LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS);

    half3 specularColor = AnistropySpecular(attenuatedLightColor, mainLight.direction,inputData.normalWS, btangent, inputData.viewDirectionWS, specularGloss, smoothness,shift);

#ifdef _ADDITIONAL_LIGHTS
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, inputData.positionWS);
        half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        diffuseColor += LightingLambert(attenuatedLightColor, light.direction, inputData.normalWS);
        specularColor += AnistropySpecular(attenuatedLightColor, -light.direction, inputData.normalWS, normalTS, inputData.viewDirectionWS, specularGloss, smoothness, shift);
    }
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX
    diffuseColor += inputData.vertexLighting;
#endif

    ////////////////////////////////////////////////////////////////////
    //o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));  //得到法线
    //float4 skinColor = tex2D(_SkinTex, IN.uv_SkinTex) * _SkinTint * _SkinTint.a;    //内颜色
    //float4 stockingColor = tex2D(_StockingTex, IN.uv_StockingTex) * _StockingTint * _StockingTint.a;    //外颜色
    //float4 stockingColor = SAMPLE_TEXTURE2D(_StockingTex, sampler_StockingTex, uv) * _StockingTint * _StockingTint.a;
    //float rim = pow(1 - dot(normalize(IN.viewDir), o.Normal), _RimPower / 10);   //边缘光
    //float oneMndl = max(0, 1 - dot(normalize(inputData.viewDirectionWS), normal));
    //float rim = pow(oneMndl, _RimPower / 10);   //边缘光
    //float fresnel = pow(oneMndl, _FresnelScale);    //菲涅尔
    //float denier = (_Denier - 5) / 115;    //丹尼尔参数
    //float density = max(rim, (denier * (1 - tex2D(_DenierTex, IN.uv_DenierTex))));  //lerp参数
    //diffuse = lerp(diffuse, stockingColor, density);
    //diffuse = diffuse * lerp(_Fresnel0, _Fresnel1, 1 - fresnel);
    ////////////////////////////////////////////////////////////////////

    half3 finalColor = diffuseColor * diffuse;

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
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    half3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
    half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.posWS.xyz = vertexInput.positionWS;
    output.positionCS = vertexInput.positionCS;

#ifdef _NORMALMAP
    output.normal = half4(normalInput.normalWS, viewDirWS.x);
    output.tangent = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangent = half4(normalInput.bitangentWS, viewDirWS.z);
#else
    output.normal = NormalizeNormalPerVertex(normalInput.normalWS);
    output.viewDir = viewDirWS;
#endif

    OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
    OUTPUT_SH(output.normal.xyz, output.vertexSH);

    output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
    output.shadowCoord = GetShadowCoord(vertexInput);
#endif

    return output;
}
half4 UniversalFragmentBlinnPhongStocking(InputData inputData, half3 diffuse, half4 specularGloss, half smoothness, half3 emission, half alpha)
{
    // To ensure backward compatibility we have to avoid using shadowMask input, as it is not present in older shaders
#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
    half4 shadowMask = inputData.shadowMask;
#elif !defined (LIGHTMAP_ON)
    half4 shadowMask = unity_ProbesOcclusion;
#else
    half4 shadowMask = half4(1, 1, 1, 1);
#endif

    Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, shadowMask);
    //针对当前灯光和GI做系数值
    mainLight.color *= _FactorML;
    //inputData.bakedGI = 0;
#if defined(_SCREEN_SPACE_OCCLUSION)
    AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(inputData.normalizedScreenSpaceUV);
    mainLight.color *= aoFactor.directAmbientOcclusion;
    inputData.bakedGI *= aoFactor.indirectAmbientOcclusion;
#endif

    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI);
    //inputData.bakedGI = 0;
    half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
    inputData.bakedGI *= _FactorGI;
    half3 diffuseColor = inputData.bakedGI + LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS);
    half3 specularColor = LightingSpecular(attenuatedLightColor, mainLight.direction, inputData.normalWS, inputData.viewDirectionWS, specularGloss, smoothness);

#ifdef _ADDITIONAL_LIGHTS
    uint pixelLightCount = GetAdditionalLightsCount();
    for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
    {
        Light light = GetAdditionalLight(lightIndex, inputData.positionWS, shadowMask);
#if defined(_SCREEN_SPACE_OCCLUSION)
        light.color *= aoFactor.directAmbientOcclusion;
#endif
        half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        diffuseColor += LightingLambert(attenuatedLightColor, light.direction, inputData.normalWS);
        specularColor += LightingSpecular(attenuatedLightColor, light.direction, inputData.normalWS, inputData.viewDirectionWS, specularGloss, smoothness);
    }
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX
    diffuseColor += inputData.vertexLighting;
#endif

    half3 finalColor = diffuseColor * diffuse + emission;

#if defined(_SPECGLOSSMAP) || defined(_SPECULAR_COLOR)
    finalColor += specularColor;
#endif

    return half4(finalColor, alpha);
}
// Used for StandardSimpleLighting shader
half4 LitPassFragmentSimple(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv = input.uv;
	//"Specular (R) Spec Shift (G) Spec Mask (B)"
    half4 rgba = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));

	
    //half alpha = rgba.a * _BaseColor.a;
    half alpha = _BaseColor.a;
    half3 diffuse = rgba.rgb * _BaseColor.rgb*alpha ;
    //AlphaDiscard(alpha, _Cutoff);
//#ifdef _ALPHAPREMULTIPLY_ON
//    diffuse *= alpha;
//#endif

    half3 normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
    half3 emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

    half4 maskShift = SampleSpecularMaskShift(uv, alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
	half4 specular = _SpecColor;
    half smoothness = exp2(10 * _Smoothness + 1);
	half shift = maskShift.g - 0.5;


    InputData inputData;
    InitializeInputData(input, normalTS, inputData);
	half3 btangent = -normalize(cross(inputData.normalWS, input.tangent.xyz));
    ////////////////////////////////////////////////////////////////////
    half4 stockingColor = SAMPLE_TEXTURE2D(_StockingTex, sampler_StockingTex, uv) * _StockingTint * _StockingTint.a;
    float oneMndl = max(0, 1 - dot(normalize(inputData.viewDirectionWS), inputData.normalWS));
    float rim = pow(oneMndl, _RimPower / 10);   //边缘光
    float fresnel = pow(oneMndl, _FresnelScale);    //菲涅尔
    float denier = (_Denier - 5) / 115;    //丹尼尔参数
    //todo:改成世界坐标的Gradient，省去一张采样
    float density = max(rim, (denier * (1 - SAMPLE_TEXTURE2D(_DenierTex, sampler_DenierTex, uv))));  //lerp参数
    diffuse = lerp(diffuse, stockingColor, density);
    diffuse = diffuse * lerp(_Fresnel0, _Fresnel1, 1 - fresnel);
    ////////////////////////////////////////////////////////////////////
     

    //half4 color = LightweightFragmentAnistropyBlinnPhong(inputData, diffuse, specular, smoothness, inputData.normalWS, btangent, shift,alpha, stockingColor);
    half4 color = UniversalFragmentBlinnPhongStocking(inputData, diffuse, specular, smoothness, emission, alpha);
    //half4 color = half4(diffuse,1);
    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    return color;
};

#endif

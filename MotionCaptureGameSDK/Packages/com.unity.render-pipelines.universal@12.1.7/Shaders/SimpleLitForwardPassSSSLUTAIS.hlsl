#ifndef UNIVERSAL_SIMPLE_LIT_PASS_INCLUDED
#define UNIVERSAL_SIMPLE_LIT_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

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
    //目前皮肤需求都是要法线的
        half4 normalWS                 : TEXCOORD2;    // xyz: normal, w: viewDir.x
        half4 tangentWS                : TEXCOORD3;    // xyz: tangent, w: viewDir.y
        half4 bitangentWS              : TEXCOORD4;    // xyz: bitangent, w: viewDir.z
//#else
//    float3  normal                  : TEXCOORD3;
//    float3 viewDir                  : TEXCOORD4;
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
//struct InputDataSSSLUT
//{
//    float3  positionWS;
//    half3   normalWS;
//    half3   viewDirectionWS;
//    float4  shadowCoord;
//    half    fogCoord;
//    half3   vertexLighting;
//    half3   bakedGI;
//    float2  normalizedScreenSpaceUV;
//    half4   shadowMask;
//    float4  tangent;
//};

//精简版本2
void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
{
    inputData = (InputData)0;

    inputData.positionWS = input.positionWS;
//#ifdef _NORMALMAP
        half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
        inputData.tangentToWorld = half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz);
        inputData.normalWS = TransformTangentToWorld(normalTS, inputData.tangentToWorld);
//#else
        //half3 viewDirWS = GetWorldSpaceNormalizeViewDir(inputData.positionWS);
        //inputData.normalWS = input.normalWS;
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
    inputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);

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
    half3 viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);

#if defined(_FOG_FRAGMENT)
        half fogFactor = 0;
#else
        half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
#endif

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.positionWS.xyz = vertexInput.positionWS;
    output.positionCS = vertexInput.positionCS;

//#ifdef _NORMALMAP
    output.normalWS = half4(normalInput.normalWS, viewDirWS.x);
    output.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);
//#else
//    output.normalWS = NormalizeNormalPerVertex(normalInput.normalWS);
//    output.viewDir = viewDirWS;
//#endif
    /*half3 binormal = cross(normalInput.tangentWS, normalInput.tangentWS) * input.tangentOS.w;
    output.TtoW0 = float4(normalInput.tangentWS.x, binormal.x, normalInput.normalWS.x, vertexInput.positionWS.x);
    output.TtoW1 = float4(normalInput.tangentWS.y, binormal.y, normalInput.normalWS.y, vertexInput.positionWS.y);
    output.TtoW2 = float4(normalInput.tangentWS.z, binormal.z, normalInput.normalWS.z, vertexInput.positionWS.z);*/


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

//
//float4 Tonemap(float3 rgb)
//{
//#if _TONEMAP_ON
//    rgb *= _Exposure;
//    rgb = max(0, rgb - 0.004);
//    rgb = (rgb * (6.2 * rgb + 0.5)) / (rgb * (6.2 * rgb + 1.7) + 0.06);
//    rgb = pow(rgb, 2.2);
//#endif
//
//    return float4(rgb, 1);
//}
//float2 Remap(float2 original_value, float2 original_min,
//    float2 original_max, float2 new_min, float2 new_max)
//{
//    return new_min + (((original_value - original_min) /
//        (original_max - original_min)) * (new_max - new_min));
//
//}
//
float fresnelReflectance(float3 H, float3 V, float F0)
{
    float base = 1.0 - dot(V, H);
    float exponential = pow(base, 5.0);
    return exponential + F0 * (1.0 - exponential);
}
half DirectBRDFSpecular2(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
{
    float3 halfDir = SafeNormalize(float3(lightDirectionWS)+float3(viewDirectionWS));

    float NoH = saturate(dot(normalWS, halfDir));
    half LoH = saturate(dot(lightDirectionWS, halfDir));

    // GGX Distribution multiplied by combined approximation of Visibility and Fresnel
    // BRDFspec = (D * V * F) / 4.0
    // D = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2
    // V * F = 1.0 / ( LoH^2 * (roughness + 0.5) )
    // See "Optimizing PBR for Mobile" from Siggraph 2015 moving mobile graphics course
    // https://community.arm.com/events/1155

    // Final BRDFspec = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2 * (LoH^2 * (roughness + 0.5) * 4.0)
    // We further optimize a few light invariant terms
    // brdfData.normalizationTerm = (roughness + 0.5) * 4.0 rewritten as roughness * 4.0 + 2.0 to a fit a MAD.
    float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;

    half LoH2 = LoH * LoH;
    half specularTerm = brdfData.roughness2 / ((d * d) * max(0.1h, LoH2) * brdfData.normalizationTerm);

    // On platforms where half actually means something, the denominator has a risk of overflow
    // clamp below was added specifically to "fix" that, but dx compiler (we convert bytecode to metal/gles)
    // sees that specularTerm have only non-negative terms, so it skips max(0,..) in clamp (leaving only min(100,...))
#if defined (SHADER_API_MOBILE) || defined (SHADER_API_SWITCH)
    specularTerm = specularTerm - HALF_MIN;
    specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
#endif

    return specularTerm;
}
inline void InitializeBRDFDataDirect2(half3 diffuse, half3 specular, half reflectivity, half oneMinusReflectivity, half smoothness, inout half alpha, out BRDFData outBRDFData)
{
    outBRDFData = (BRDFData)0;
    outBRDFData.albedo = half3(0.0, 0.0, 0.0);
    outBRDFData.diffuse = diffuse;
    outBRDFData.specular = specular;
    outBRDFData.reflectivity = reflectivity;

    outBRDFData.perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(smoothness);
    outBRDFData.roughness = max(PerceptualRoughnessToRoughness(outBRDFData.perceptualRoughness), HALF_MIN_SQRT);
    outBRDFData.roughness2 = max(outBRDFData.roughness * outBRDFData.roughness, HALF_MIN);
    outBRDFData.grazingTerm = saturate(smoothness + reflectivity);
    outBRDFData.normalizationTerm = outBRDFData.roughness * half(4.0) + half(2.0);
    outBRDFData.roughness2MinusOne = outBRDFData.roughness2 - half(1.0);

#ifdef _ALPHAPREMULTIPLY_ON
    outBRDFData.diffuse *= alpha;
    alpha = alpha * oneMinusReflectivity + reflectivity; // NOTE: alpha modified and propagated up.
#endif
}
inline void InitializeBRDFData2(half3 albedo, half metallic, half3 specular, half smoothness, inout half alpha, out BRDFData outBRDFData)
{
    //#ifdef _SPECULAR_SETUP
    //    half reflectivity = ReflectivitySpecular(specular);
    //    half oneMinusReflectivity = 1.0 - reflectivity;
    //    half3 brdfDiffuse = albedo * (half3(1.0h, 1.0h, 1.0h) - specular);
    //    half3 brdfSpecular = specular;
    //#else
    half oneMinusReflectivity = OneMinusReflectivityMetallic(metallic);
    half reflectivity = 1.0 - oneMinusReflectivity;
    half3 brdfDiffuse = albedo * oneMinusReflectivity;
    half3 brdfSpecular = lerp(kDieletricSpec.rgb, albedo, metallic);
    //#endif

    InitializeBRDFDataDirect2(brdfDiffuse, brdfSpecular, reflectivity, oneMinusReflectivity, smoothness, alpha, outBRDFData);
}
half4 LitPassFragmentSimpleSSSLUT(Varyings input) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv = input.uv;
    float4 diffuseAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
#ifdef _DIFFUSE_OFF
    diffuseAlpha.rgb = 0;
#endif
    float3 diffuse = diffuseAlpha.rgb * _BaseColor.rgb;
    float fR = diffuseAlpha.a;
    //现在变成了粗糙度图，也就不用反置了。反置occgloss的b去
//#ifdef _DIFFUSEA_RVT
//    fR = 1 - fR;
//#endif
    /*float alpha = lerp(_Alpha0, _Alpha1, fR);
    alpha = alpha * _BaseColor.a;*/
    //暂时没图，先这么着吧
    //float alpha = lerp(_Alpha0, _Alpha1, fR * _BaseColor.a);
    //FR现在是光泽度了，头身目前分开的，所以不能共用
    //后面等真正除了通透图再弄上去
    //float alpha = lerp(_Alpha0, _Alpha1, 1*_BaseColor.a);
    /*float alpha = lerp(_Alpha0, _Alpha1, 1 );*/
    
    /*float alpha = diffuseAlpha.a * _BaseColor.a;*/
    //float alpha = diffuseAlpha.a ;
    //_BaseColor.a
    //AlphaDiscard(alpha, _Cutoff);
    //暂不考虑cutoff
    //#ifdef _ALPHAPREMULTIPLY_ON
    //    diffuse *= alpha;
    //#endif

    float3 normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
    //float3 emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
    float3 emission = 0;// diffuse * 2;// *float3(0.43529, 0.43529, 0.43529);
#ifdef _SPECULAR_COLOR
    emission = diffuseAlpha.rgb * _SpecColor.rgb * _SpecColor.a;
    //emission = diffuseAlpha.rgb * _SpecColor ;
    //emission = diffuseAlpha.rgb * _SpecColor * 0.1;
#endif
    //暂时屏蔽高光接口采样
    /*float4 specular = SampleSpecularSmoothness(uv, alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
    float smoothness = specular.a;*/

    InputData inputData;
    //InputDataSSSLUT inputData;
    InitializeInputData(input, normalTS, inputData);
SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);

#ifdef _DBUFFER
    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
#endif



#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
    half4 shadowMask = inputData.shadowMask;
#elif !defined (LIGHTMAP_ON)
    half4 shadowMask = unity_ProbesOcclusion;
#else
    half4 shadowMask = half4(1, 1, 1, 1);
#endif
    Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, shadowMask);
    float3 N_low = inputData.normalWS; // float3(i.TtoW0.z, i.TtoW1.z, i.TtoW2.z);
    float3 lightDir = mainLight.direction;
    float3 NdotL3 = dot(N_low, lightDir);
    float NdotL = NdotL3;
    //SSS DIFFUSE
    half3 diffuseSSS;
    half wrappedNdL = NdotL * 0.5 + 0.5;
    half3 h3Gloss = SAMPLE_TEXTURE2D(_OccGlossMap, sampler_OccGlossMap, uv);

    //diffuseSSS = NdotL;
    //diffuseSSS = alpha;
    //mainLight.color += diffuseSSS;
    //没lightmap，目前也是可以不用的
    //MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI);
    //diffuseColor 其实是光， diffuse才是真正的颜色
    //half3 albedo = diffuseColor * diffuse;
    float3 albedo = diffuse;
    /////////////////////////////////////////
    float shadow = mainLight.shadowAttenuation;// SHADOW_ATTENUATION(i);
    float3 worldPos = inputData.positionWS;// float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
    //float3 N_low = inputData.normalWS; // float3(i.TtoW0.z, i.TtoW1.z, i.TtoW2.z);
    //float3 NdotL = dot(N_low, lightDir);
    //float3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
    //float3 lightDir = mainLight.direction;
    //float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
    //float3 viewDir = SafeNormalize(_WorldSpaceCameraPos - worldPos);
    float3 viewDir = inputData.viewDirectionWS;

    
    float3 tangentNormal;
    tangentNormal = normalTS;// UnpackNormal(bumpColor);
    //tangentNormal.xy *= _BumpScale;
    tangentNormal.z = normalize(sqrt(1 - saturate(dot(tangentNormal.xy, tangentNormal.xy))));
    
    float3x3 t2wMatrix = float3x3(input.tangentWS.x, input.bitangentWS.x, input.normalWS.x
                                , input.tangentWS.y, input.bitangentWS.y, input.normalWS.y
                                , input.tangentWS.z, input.bitangentWS.z, input.normalWS.z);
    float3 N_high = float3(mul(t2wMatrix, tangentNormal));
    //float3 N_high = NdotL;
    float NoL = NdotL;

    //Ambient 暂时没有用到
    float3 ambient = clamp(UNITY_LIGHTMODEL_AMBIENT.xyz, 0, 0.2) * albedo;

    //SPECULAR
    float3 halfDir = normalize(viewDir + lightDir);
    float ndh = dot(normalize(N_high), halfDir);
    //ndh = clamp(ndh, -1, 0);
    //ndh = -abs(ndh)-0.001;//builtin
    //ndh = -clamp(abs(ndh), 0.001, 0.999);
    //ndh = (ndh)*step(ndh, 0)+ (1-ndh) *step(ndh, 0);
    //half4 alpha3 = tex2D(sampler_SpecularLUT, float2(ndh, _Roughness));
    //half4 alpha3 = SAMPLE_TEXTURE2D(_SpecularLUT, sampler_SpecularLUT, float2(1, 0));
    //half4 alpha3 = SAMPLE_TEXTURE2D(_SpecularLUT, sampler_SpecularLUT, float2(ndh, _Roughness));
    //half4 alpha3 = SAMPLE_TEXTURE2D(_SpecularLUT, sampler_SpecularLUT, float2(ndh, diffuseAlpha.a));
    //half4 alpha3 = SAMPLE_TEXTURE2D(_SpecularLUT, sampler_SpecularLUT, float2(ndh, 0));
    //alpha3.w = diffuseAlpha.a;
    //alpha3.w = ndh;
    //默认反采

    //float4 alpha3 = SAMPLE_TEXTURE2D(_SpecularLUT, sampler_SpecularLUT, float2(ndh, saturate(fR*(1-_Roughness))));
    //暂时没有曲率图
    float4 alpha3 = SAMPLE_TEXTURE2D(_SpecularLUT, sampler_SpecularLUT, float2(ndh, 0 ));

    //float4 alpha3 = _SpecularLUT.Sample(sampler_linear_repeat_SpecularLUT, float2(ndh, (1 - _Roughness) * diffuseAlpha.a));//builtin
    ///////////////////////////////////////
    // //旧高光
    //float4 alpha3 = SAMPLE_TEXTURE2D(_SpecularLUT, sampler_SpecularLUT, float2(ndh, _Roughness));
    //alpha3.w = 1;
    //alpha3.w = 0;
    //alpha3.w *= step(ndh,0);
    float ph = pow(2.0 * alpha3.w, 10);
    //float ph = pow(2.0*0.2, 10);
    float F = fresnelReflectance(halfDir, viewDir, 0.28);
    float frSpec = (1-fR) * max(ph * F / dot(halfDir, halfDir), 0);//builtin
    //float frSpec = min(ph * F / dot(halfDir, halfDir), 0);
    //float frSpec = max(ph * F / dot(halfDir, halfDir), 0);// ph* F / dot(halfDir, halfDir);
    //float frSpec = alpha3.w;
    ////////////////////////////////////////////////////////////////
    //兰伯特
    //float res = mainLight.color * shadow * saturate(NoL) * _Brightness * frSpec;
    //float res = _LightColor0.rgb * shadow * saturate(NoL) * _Brightness * frSpec; 
    //half3 specular3 = inputData.bakedGI + half3(res, res, res);
    //half3 specular3 = mainLight.color * shadow * saturate(NoL) * _Brightness * frSpec;
    half3 specular3 = mainLight.color ;
    
    
    half occ = 1;// h3Gloss.g;
    //half occ = SAMPLE_TEXTURE2D(_OccGlossMap, sampler_OccGlossMap, uv).g;
    /*LerpWhiteTo(occ, _Smoothness);*/

    
    half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
    //half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * 1);// mainLight.shadowAttenuation);
    //half3 diffuseColor = inputData.bakedGI * occ * _Occ + LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS);
    half3 diffuseColor =  LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS);
    diffuseColor = lerp(_Shadow0, _Shadow1, diffuseColor);
    diffuseColor = diffuseColor + inputData.bakedGI * occ * _Occ;
    //float alpha = lerp(_Alpha0, _Alpha1, h3Gloss.r);
    float alpha = lerp(_Alpha0, _Alpha1, 1);
#ifndef _SIMPLIFY_OFF
    //float3 lookup = NdotL3 * 0.5 + 0.5;
    float3 lookup = diffuseColor.rgb * 0.4;
    //half curve = tex2D(sampler_CurveTex, i.uv.xy).r;
    //half curve = tex2D(sampler_CurveTex, uv).r;
    //half curve = SAMPLE_TEXTURE2D(_CurveTex, sampler_CurveTex,uv).r;
    half curve = alpha;// diffuseAlpha.r;
    /*diffuseSSS.r = tex2D(sampler_SSSLUT, float2(lookup.r, curve * _CurveFactor)).r;
    diffuseSSS.g = tex2D(sampler_SSSLUT, float2(lookup.g, curve * _CurveFactor)).g;
    diffuseSSS.b = tex2D(sampler_SSSLUT, float2(lookup.b, curve * _CurveFactor)).b;*/
    curve = curve * _CurveFactor;
    diffuseSSS.r = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(lookup.r, curve)).r;
    diffuseSSS.g = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(lookup.g, curve)).g;;
    diffuseSSS.b = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(lookup.b, curve)).b;
    //diffuseSSS = 0;
#else
    //    //精简版2
        //diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(NoL * 0.5 + 0.5, 1/alpha)).xyz;
    //diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(wrappedNdL, alpha)).xyz;
    //diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(wrappedNdL, alpha)).xyz;
    //todo:去掉不必要的计算
    diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(RGBConvertToHSV(diffuseColor.rgb * 0.4).z, alpha)).xyz;
    //diffuseColor * 0.4(这时候细节最充分);
    //diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(NoL, alpha)).xyz;
    //diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(NoL * 0.5 + 0.5, 1.0)).xyz;
    //diffuseSSS = diffuse + diffuseSSS;

    //diffuseSSS = 1;
#endif
    //diffuseSSS = 0.3 + diffuseSSS;
    //diffuseSSS = lerp(0.3, 0.9, diffuseSSS);
    //diffuseSSS = lerp(_LutRemap0, _LutRemap1, saturate(diffuseSSS));
    //diffuseSSS.r = saturate(diffuseSSS.r *4);
    //diffuseSSS.g = saturate(diffuseSSS.g * 2);
    //float alpha = lerp(_Alpha0, _Alpha1, h3Gloss.r);
    diffuseSSS = lerp(_LutRemap0, _LutRemap1 * 1, (diffuseSSS));
    //diffuseSSS = lerp(_LutRemap0, _LutRemap1* h3Gloss.r, (diffuseSSS));
    
    //diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(ndh * attenuatedLightColor.r, alpha)).xyz;
    //diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(diffuseColor.r + (ddx(diffuseColor.r) + ddy(diffuseColor.r))/4, alpha)).xyz;
    //diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(RGBConvertToHSV(diffuseColor.rgb*0.4).z, alpha)).xyz;
    
    ////diffuseColor * 0.4(这时候细节最充分);
    //diffuseSSS = lerp(_LutRemap0, _LutRemap1, (diffuseSSS));


    //specular3 = specular3 * shadow * saturate(NoL) * _Brightness * frSpec;
    //specular3 = SafeNormalize(specular3) * shadow * saturate(NoL) * _Brightness * frSpec;
    //specular3 = diffuseColor * saturate(NoL) * _Brightness * frSpec;
    half hSFR = _Smoothness *fR;
    /////////////////////////////////////////////
    //旧高光
    specular3 = saturate(NoL) * _Brightness * frSpec;
    //////////////////////////////////////
    half oneMinusReflectivity = OneMinusReflectivityMetallic(hSFR);
    half reflectivity = 1.0 - oneMinusReflectivity;
    half3 brdfDiffuse = albedo * oneMinusReflectivity;
    half3 brdfSpecular = lerp(kDieletricSpec.rgb, albedo, hSFR);

    BRDFData brdfData;
    half hTemp5 = 0;
    half hTemp6 = 1;
    half3 h3Temp7 = half3(0.0h, 0.0h, 0.0h);
    InitializeBRDFData2(albedo, hTemp5, h3Temp7, hSFR, hTemp6, brdfData);
    BRDFData brdfDataClearCoat = (BRDFData)0;

    //SampleMetallicSpecGloss mask后面一个参数
    
    //specular3 = lerp(_SpcRemap0, _SpcRemap1, (1 - fR))*brdfSpecular* DirectBRDFSpecular2(brdfData, inputData.normalWS, mainLight.direction, inputData.viewDirectionWS);
    //specular3 = (1 - fR) * brdfSpecular * DirectBRDFSpecular2(brdfData, inputData.normalWS, mainLight.direction, inputData.viewDirectionWS);
#ifdef _DIFFUSEA_RVT
    specular3 = (1 - h3Gloss.b) * brdfSpecular * DirectBRDFSpecular2(brdfData, inputData.normalWS, mainLight.direction, inputData.viewDirectionWS);
#else
    specular3 = h3Gloss.b * brdfSpecular * DirectBRDFSpecular2(brdfData, inputData.normalWS, mainLight.direction, inputData.viewDirectionWS);
#endif
    
    
    //specular3 = brdfSpecular * DirectBRDFSpecular2(brdfData, inputData.normalWS, mainLight.direction, inputData.viewDirectionWS);
    //specular3 = brdfSpecular*LightingSpecular(attenuatedLightColor, mainLight.direction, inputData.normalWS, inputData.viewDirectionWS, SampleMetallicSpecGloss(uv, 1), hSFR);
    //specular3 = brdfData.specular * DirectBRDFSpecular(brdfData, normalWS, lightDirectionWS, viewDirectionWS);
    ////////////////////////////////////////////////////////
    //specular3 = SafeNormalize(specular3) *  saturate(NoL) * _Brightness * frSpec;
    specular3 = lerp(_SpcRemap0, _SpcRemap1, specular3);
    //暂时拿去调其他的
    //specular3 = lerp(0, 0.482, specular3);
    //half3 specular3 = half3(res, res, res);

   
    //diffuseSSS = 0;
    //fixed3 diffuse = _LightColor0.rgb * albedo * diffuseSSS * shadow;
    //half3 diffuse3 =  mainLight.color * albedo * diffuseSSS ;
    //half3 diffuse3 = albedo *  diffuseColor* diffuseSSS;
    //half3 diffuse3 = albedo * diffuseColor;
    //half3 diffuse3 = albedo * diffuseColor* _BaseColor.a;
    half3 diffuse3 = albedo * saturate(diffuseColor) * _BaseColor.a;
    //half3 diffuse3 = albedo *  diffuseColor * saturate(NoL);
    //half3 diffuse3 = albedo *  diffuseSSS;
    //half3 diffuse3 = albedo;
    //half3 diffuse3 = min(albedo ,diffuseSSS);
    //diffuse3 = alpha;
    //diffuse3 = ndh;
    //half3 diffuse3 = (1- (NoL * 0.5 + 0.5))*2*mainLight.color * albedo * diffuseSSS * shadow;
    //half3 diffuse3 = mainLight.color * albedo * shadow;
    //return half4(diffuse + ambient + specular, 1.0);
    /////////////////////////////////////////
    //叠加法
    //half3 finalColor = diffuse3 + ambient + specular3 + emission;
    //融合法
    /*half3 finalColor = diffuse3 + ambient + emission;
    finalColor = finalColor * (1 - diffuseAlpha.a) + specular3 * (diffuseAlpha.a);*/
    //half3 finalColor = diffuse3+ specular3 + ambient + emission;
    //finalColor = finalColor + lerp(0, 1, ambient + emission);
    /*half3 finalColor = diffuse3 + ambient + emission;*/
    //finalColor = lerp(finalColor, specular3, _Roughness);
    //finalColor = min(diffuseColor*0.6,lerp(finalColor , specular3, _Roughness) + albedo*diffuseSSS*(1- NdotL));
    //finalColor = SafeNormalize(diffuseColor*0.8) * lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - NdotL);
    //finalColor =  lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - NdotL);
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS ;
    /*finalColor = albedo * diffuseSSS * (1 - NdotL);
    finalColor = albedo * diffuseSSS * (1 - diffuseColor);*/
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - NdotL);
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * SafeNormalize(1 - NdotL);
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * SafeNormalize(1 - NdotL) * SafeNormalize(1 - SafeNormalize(diffuseColor));
    //finalColor = albedo * diffuseSSS * (1 - NdotL);
    //finalColor = albedo * diffuseSSS * (1 - attenuatedLightColor);
    //finalColor = albedo * diffuseSSS * (1 - diffuseColor);
    half3 finalColor = diffuse3 + ambient + emission;
    /*half oneMDC = 1 - diffuseColor;
    half stepDC = step(0,oneMDC);*/
    //diffuseColor = (1 - SafeNormalize(diffuseColor));
    //diffuseColor = lerp(_SpcRemap0, _SpcRemap1, diffuseColor * _Roughness);
    //finalColor = diffuseColor;
    //diffuseColor = lerp(_SpcRemap0, _SpcRemap1, diffuseColor* _Roughness);
    //finalColor = _SpecColor.rgb * albedo + albedo * diffuseSSS * diffuseColor;
    finalColor = _SpecColor.rgb * albedo + albedo * diffuseSSS  *(1 - SafeNormalize(diffuseColor));
    //finalColor = finalColor+albedo * diffuseSSS * (1 - saturate(diffuseColor));
    //finalColor = lerp(finalColor* saturate(diffuseColor), specular3, _Roughness);
    //finalColor = lerp(finalColor * SafeNormalize(diffuseColor), specular3 + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor)), _Roughness);
    //finalColor = lerp(finalColor * SafeNormalize(diffuseColor), min(specular3 , finalColor), _Roughness);
    ////////////////////////////
    //1
    //finalColor =  max(specular3 , finalColor);
    //////////////////////////////
    //half NdotL = saturate(dot(normalWS, lightDirectionWS));
    NdotL = saturate(NdotL);
    //half3 radiance = mainLight.color * (lightAttenuation * NdotL);
    half3 radiance = attenuatedLightColor * NdotL;
    finalColor = radiance*specular3+finalColor;
    //finalColor = max(specular3, finalColor);
    //specular3 = radiance * specular3;
    finalColor = lerp(finalColor, specular3, _Roughness);

    //finalColor = max(radiance * specular3, finalColor);
    //////////////////////////////
    
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor));
    
    //finalColor = lerp(finalColor, specular3, _Roughness) * diffuseSSS ;
    //finalColor =  lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor));
    //finalColor = (1 - SafeNormalize(diffuseColor));
    //finalColor = (1 - saturate(diffuseColor));
    //finalColor = diffuseColor;
    //diffuseColor.r * 0.4
    //finalColor = ndh* mainLight.shadowAttenuation;
    //finalColor = attenuatedLightColor.b;
    //finalColor = NdotL;
    //finalColor = wrappedNdL;
    //finalColor = RGBConvertToHSV(diffuseColor.rgb).b;
    //finalColor = RGBConvertToHSV(diffuseColor.rgb).g;
    /*finalColor = saturate(ndh)* attenuatedLightColor;
    finalColor = saturate(NdotL) * attenuatedLightColor;
    finalColor = LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS);
    finalColor = diffuseColor;*/
    // 
    //finalColor = mainLight.shadowAttenuation;
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - saturate(diffuseColor));
    //finalColor = (1 - (mainLight.shadowAttenuation));
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - (mainLight.shadowAttenuation));
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * SafeNormalize((1 - diffuseColor));
    //finalColor = albedo * diffuseSSS * SafeNormalize((1 - diffuseColor));
    //finalColor = albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor));
    //finalColor = albedo * diffuseSSS *(stepDC* oneMDC + (1 - stepDC));
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (stepDC * oneMDC + (1) * (1 - stepDC));
    //finalColor = NdotL;
    //finalColor = albedo * diffuseSSS*2;
    //finalColor = clamp(finalColor, 0, 1);
    //finalColor = finalColor* (1+specular3.r);
    //finalColor = diffuseColor* diffuseSSS;
    //finalColor = diffuseColor;
    //finalColor = diffuse3 + ambient ;
    //finalColor = diffuse3;
    //finalColor = diffuseSSS;
    //finalColor = SafeNormalize(diffuseSSS);
    //finalColor = diffuseSSS;
    //finalColor = albedo;
    //finalColor = albedo* saturate(diffuseColor)* _BaseColor.a;
    //finalColor = lerp(finalColor, inputData.bakedGI * occ, _Roughness);
    //finalColor = lerp(finalColor, occ, _Roughness);
    
    //finalColor = lerp(finalColor, specular3, _Roughness)* attenuatedLightColor;
    //finalColor = lerp(finalColor, specular3, _Roughness) * diffuseColor;
    //finalColor = max(finalColor,specular3);
	//half3 finalColor = inputData.bakedGI + diffuse3 + ambient;
    //half3 finalColor = diffuseColor * diffuse2;
    //half3 finalColor = diffuseColor * diffuse2 + emission;

//#if defined(_SPECGLOSSMAP) || defined(_SPECULAR_COLOR)
//    finalColor += specularColor;
//#endif

    half4 color = half4(finalColor, alpha);
    //color = half4(specular3, alpha);
    
    //color = alpha3.w;
    //color = fR;
    //color = frSpec;
    //color = diffuseAlpha.a;
    //half4 color = UniversalFragmentBlinnPhong(inputData, diffuse, specular, smoothness, emission, alpha);
    //half4 color = UniversalFragmentBlinnPhong2(inputData, input,normalTS, diffuse, specular, smoothness, emission, alpha);
    //half4 color = UniversalFragmentBlinnPhongSSSLUT(inputData, diffuse, specular, smoothness, emission, alpha, uv);
    //half4 color = UniversalFragmentBlinnPhong(inputData, diffuse, specular, smoothness, emission, alpha);
    //color.rgb += specular3 * diffuseSSS;
    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    color.a = OutputAlpha(color.a, _Surface);

    return color;
}

#endif

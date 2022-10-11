#ifndef UNIVERSAL_SIMPLE_LIT_PASS_INCLUDED
#define UNIVERSAL_SIMPLE_LIT_PASS_INCLUDED

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

//#ifdef _NORMALMAP
    //目前皮肤需求都是要法线的
    float4 normal                   : TEXCOORD3;    // xyz: normal, w: viewDir.x
    float4 tangent                  : TEXCOORD4;    // xyz: tangent, w: viewDir.y
    float4 bitangent                : TEXCOORD5;    // xyz: bitangent, w: viewDir.z
//#else
//    float3  normal                  : TEXCOORD3;
//    float3 viewDir                  : TEXCOORD4;
//#endif

    half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord              : TEXCOORD7;
#endif
    //float4 TtoW0              : TEXCOORD8;
    //float4 TtoW1              : TEXCOORD9;
    //float4 TtoW2             : TEXCOORD10;
    float4 positionCS               : SV_POSITION;
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
    inputData.positionWS = input.posWS;

//#ifdef _NORMALMAP
    half3 viewDirWS = half3(input.normal.w, input.tangent.w, input.bitangent.w);
    inputData.normalWS = TransformTangentToWorld(normalTS,
        half3x3(input.tangent.xyz, input.bitangent.xyz, input.normal.xyz));
//#else
//    half3 viewDirWS = input.viewDir;
//    inputData.normalWS = input.normal;
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

    inputData.fogCoord = input.fogFactorAndVertexLight.x;
    inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
    inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
    //inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    //inputData.bakedGI = 0;
    inputData.normalizedScreenSpaceUV = 0;
    inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
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
    half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.posWS.xyz = vertexInput.positionWS;
    output.positionCS = vertexInput.positionCS;

//#ifdef _NORMALMAP
    output.normal = half4(normalInput.normalWS, viewDirWS.x);
    output.tangent = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangent = half4(normalInput.bitangentWS, viewDirWS.z);
//#else
//    output.normal = NormalizeNormalPerVertex(normalInput.normalWS);
//    output.viewDir = viewDirWS;
//#endif
    /*half3 binormal = cross(normalInput.tangentWS, normalInput.tangentWS) * input.tangentOS.w;
    output.TtoW0 = float4(normalInput.tangentWS.x, binormal.x, normalInput.normalWS.x, vertexInput.positionWS.x);
    output.TtoW1 = float4(normalInput.tangentWS.y, binormal.y, normalInput.normalWS.y, vertexInput.positionWS.y);
    output.TtoW2 = float4(normalInput.tangentWS.z, binormal.z, normalInput.normalWS.z, vertexInput.positionWS.z);*/

    //LM
    //OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
    //// 球谐辐照
    //OUTPUT_SH(output.normal.xyz, output.vertexSH);
    //雾都考虑不要了，远距离动态物体看清楚一点也没啥
    output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

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
//real SampleShadowmapFilteredOM(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 shadowCoord, ShadowSamplingData samplingData)
//{
//    real attenuation; 
//
//#if defined(SHADER_API_MOBILE) || defined(SHADER_API_SWITCH)
//    // 4-tap hardware comparison
//    real4 attenuation4;
//    attenuation4.x = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset0.xyz);
//    attenuation4.y = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset1.xyz);
//    attenuation4.z = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset2.xyz);
//    attenuation4.w = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset3.xyz);
//    attenuation = dot(attenuation4, 0.25);
//#else
//    float fetchesWeights[9];
//    float2 fetchesUV[9];
//    SampleShadow_ComputeSamples_Tent_5x5(samplingData.shadowmapSize, shadowCoord.xy, fetchesWeights, fetchesUV);
//
//    attenuation = fetchesWeights[0] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[0].xy, shadowCoord.z));
//    attenuation += fetchesWeights[1] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[1].xy, shadowCoord.z));
//    attenuation += fetchesWeights[2] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[2].xy, shadowCoord.z));
//    attenuation += fetchesWeights[3] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[3].xy, shadowCoord.z));
//    attenuation += fetchesWeights[4] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[4].xy, shadowCoord.z));
//    attenuation += fetchesWeights[5] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[5].xy, shadowCoord.z));
//    attenuation += fetchesWeights[6] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[6].xy, shadowCoord.z));
//    attenuation += fetchesWeights[7] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[7].xy, shadowCoord.z));
//    attenuation += fetchesWeights[8] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[8].xy, shadowCoord.z));
//#endif
//
//    return attenuation;
//}
//real SampleShadowmapOM(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 shadowCoord, ShadowSamplingData samplingData, half4 shadowParams, bool isPerspectiveProjection = true)
//{
//    // Compiler will optimize this branch away as long as isPerspectiveProjection is known at compile time
//    if (isPerspectiveProjection)
//        shadowCoord.xyz /= shadowCoord.w;
//
//    real attenuation;
//    real shadowStrength = shadowParams.x;
//
//    // TODO: We could branch on if this light has soft shadows (shadowParams.y) to save perf on some platforms.
//#ifdef _SHADOWS_SOFT
//    attenuation = SampleShadowmapFilteredOM(TEXTURE2D_SHADOW_ARGS(ShadowMap, sampler_ShadowMap), shadowCoord, samplingData);
//#else
//    // 1-tap hardware comparison
//    attenuation = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz);
//#endif
//
//    attenuation = LerpWhiteTo(attenuation, shadowStrength);
//
//    // Shadow coords that fall out of the light frustum volume must always return attenuation 1.0
//    // TODO: We could use branch here to save some perf on some platforms.
//    return BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : attenuation;
//}
//
//half MainLightRealtimeShadowSSSOM(float4 shadowCoord)
//{
//#if !defined(MAIN_LIGHT_CALCULATE_SHADOWS)
//    return 1.0h;
//#endif
//
//    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
//    half4 shadowParams = GetMainLightShadowParams();
//    //shadowParams = 0;
//    return SampleShadowmapOM(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData, shadowParams, false);
//}
//half MainLightShadowSSSOM(float4 shadowCoord, float3 positionWS, half4 shadowMask, half4 occlusionProbeChannels)
//{
//    half realtimeShadow = MainLightRealtimeShadowSSSOM(shadowCoord);
//
//#ifdef CALCULATE_BAKED_SHADOWS
//    half bakedShadow = BakedShadow(shadowMask, occlusionProbeChannels);
//#else
//    half bakedShadow = 1.0h;
//#endif
//
//#ifdef MAIN_LIGHT_CALCULATE_SHADOWS
//    half shadowFade = GetShadowFade(positionWS);
//#else
//    half shadowFade = 1.0h;
//#endif
//
//#if defined(_MAIN_LIGHT_SHADOWS_CASCADE) && defined(CALCULATE_BAKED_SHADOWS)
//    // shadowCoord.w represents shadow cascade index
//    // in case we are out of shadow cascade we need to set shadow fade to 1.0 for correct blending
//    // it is needed when realtime shadows gets cut to early during fade and causes disconnect between baked shadow
//    shadowFade = shadowCoord.w == 4 ? 1.0h : shadowFade;
//#endif
//    //shadowFade = 0;
//    return MixRealtimeAndBakedShadows(realtimeShadow, bakedShadow, shadowFade);
//}
//Light GetMainLightSSSOM(float4 shadowCoord, float3 positionWS, half4 shadowMask)
//{
//    Light light = GetMainLight();
//    //light.shadowAttenuation = 0.5*MainLightShadowFabric(shadowCoord, positionWS, shadowMask, _MainLightOcclusionProbes);
//    light.shadowAttenuation = MainLightShadowSSSOM(shadowCoord, positionWS, shadowMask, _MainLightOcclusionProbes);
//    return light;
//}
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
#ifdef _DIFFUSEA_RVT
    fR = 1 - fR;
#endif
    /*float alpha = lerp(_Alpha0, _Alpha1, fR);
    alpha = alpha * _BaseColor.a;*/
    //暂时没图，先这么着吧
    //float alpha = lerp(_Alpha0, _Alpha1, fR * _BaseColor.a);
    //FR现在是光泽度了，头身目前分开的，所以不能共用
    //后面等真正除了通透图再弄上去
    //float alpha = lerp(_Alpha0, _Alpha1, 1*_BaseColor.a);
    float alpha = lerp(_Alpha0, _Alpha1, 1 );
    
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
    half occ = SAMPLE_TEXTURE2D(_OccGlossMap, sampler_OccGlossMap, uv).g;
    half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
#ifndef _SIMPLIFY_OFF
    float3 lookup = NdotL3 * 0.5 + 0.5;
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
    //float fTemp = wrappedNdL * (0.5+0.5*((attenuatedLightColor * saturate(NdotL)).r));
    //float fTemp = wrappedNdL + ((attenuatedLightColor * saturate(NdotL)).r);
    diffuseSSS = SAMPLE_TEXTURE2D(_SSSLUT, sampler_SSSLUT, float2(wrappedNdL, alpha)).xyz;
    //inputData.bakedGI* occ* _Occ + attenuatedLightColor * saturate(NdotL)
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
    //原版是0.1~5
    diffuseSSS = lerp(_LutRemap0, _LutRemap1, (diffuseSSS));
    //diffuseSSS = saturate(diffuseSSS);
    //diffuseSSS = diffuseSSS * step(diffuseSSS.r, _Roughness);// +diffuseSSS * step(diffuseSSS.r, 1);
    // 草稿
    //diffuseSSS = diffuseSSS * step(diffuseSSS.r, _Roughness) +diffuseSSS * step(_Roughness, diffuseSSS.r);
    /*diffuseSSS = diffuseSSS * step(diffuseSSS.r, 0.71) + diffuseSSS * step(0.71, diffuseSSS.r);*/
    //分界线
    //方案2
    //diffuseSSS = diffuseSSS * step(RGBConvertToHSV(diffuseSSS.rgb).z, 1.99) + diffuseSSS * step(1.92, RGBConvertToHSV(diffuseSSS.rgb).z);
    //diffuseSSS = diffuseSSS * step(diffuseSSS.r, 0.71) ;
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
    
    float3x3 t2wMatrix = float3x3(input.tangent.x, input.bitangent.x, input.normal.x
                                , input.tangent.y, input.bitangent.y, input.normal.y
                                , input.tangent.z, input.bitangent.z, input.normal.z);
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
    float4 alpha3 = SAMPLE_TEXTURE2D(_SpecularLUT, sampler_SpecularLUT, float2(ndh, fR ));

    //float4 alpha3 = _SpecularLUT.Sample(sampler_linear_repeat_SpecularLUT, float2(ndh, (1 - _Roughness) * diffuseAlpha.a));//builtin
    
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
    //兰伯特
    //float res = mainLight.color * shadow * saturate(NoL) * _Brightness * frSpec;
    //float res = _LightColor0.rgb * shadow * saturate(NoL) * _Brightness * frSpec; 
    //half3 specular3 = inputData.bakedGI + half3(res, res, res);
    //half3 specular3 = mainLight.color * shadow * saturate(NoL) * _Brightness * frSpec;
    half3 specular3 = mainLight.color ;
    
    /*half occ = SAMPLE_TEXTURE2D(_OccGlossMap, sampler_OccGlossMap, uv).g;*/
    /*LerpWhiteTo(occ, _Smoothness);*/

    
    /*half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);*/
    //half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * 1);// mainLight.shadowAttenuation);
    half3 lambertColor = LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS);
    half3 diffuseColor = inputData.bakedGI* occ* _Occ  + lambertColor;
    
    //specular3 = specular3 * shadow * saturate(NoL) * _Brightness * frSpec;
    //specular3 = SafeNormalize(specular3) * shadow * saturate(NoL) * _Brightness * frSpec;
    //specular3 = diffuseColor * saturate(NoL) * _Brightness * frSpec;
    specular3 = saturate(NoL) * _Brightness * frSpec;
    //specular3 = SafeNormalize(specular3) *  saturate(NoL) * _Brightness * frSpec;
    specular3 = lerp(_SpcRemap0, _SpcRemap1, specular3);
    //half3 specular3 = half3(res, res, res);

   
    //diffuseSSS = 0;
    //fixed3 diffuse = _LightColor0.rgb * albedo * diffuseSSS * shadow;
    //half3 diffuse3 =  mainLight.color * albedo * diffuseSSS ;
    //half3 diffuse3 = albedo *  diffuseColor* diffuseSSS;
    //half3 diffuse3 = albedo * diffuseColor;
    half3 diffuse3 = albedo * diffuseColor* _BaseColor.a;
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
    //finalColor = _SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor));
    //finalColor = _SpecColor.rgb * albedo + albedo * diffuseSSS;// *(1 - SafeNormalize(diffuseColor));
    finalColor = _SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor));
    //finalColor = _SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - diffuseColor*0.4);
    //finalColor = finalColor *(1+_Roughness * (diffuseColor));
    //finalColor = lerp(finalColor,diffuseColor, _Roughness);
    //finalColor = min(finalColor, saturate(diffuseColor)*_Roughness);
    //diffuseColor = (diffuseColor.r + diffuseColor.g + diffuseColor.b) / 3;
    
    //diffuseColor = min(min(diffuseColor.r, diffuseColor.g),diffuseColor.b) * _Roughness*2;
    
    //finalColor = min(finalColor, saturate(diffuseColor));
    
    //finalColor = _SpecColor.rgb * albedo;
    //finalColor = lerp(finalColor* saturate(diffuseColor), specular3, _Roughness);
    //finalColor = lerp(finalColor * SafeNormalize(diffuseColor), specular3 + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor)), _Roughness);
    //finalColor = lerp(finalColor * SafeNormalize(diffuseColor), min(specular3 , finalColor), _Roughness);
    //diffuseColor*0.4
    /*finalColor = max(specular3, finalColor );*/
    //不sa太爆
    //finalColor =  max(specular3 , finalColor* saturate(diffuseColor));
    //finalColor = saturate(diffuseColor);
    //finalColor = lerp(finalColor, specular3, _Roughness);
    //finalColor = lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor));
    
    //finalColor = lerp(finalColor, specular3, _Roughness) * diffuseSSS ;
    //finalColor =  lerp(finalColor, specular3, _Roughness) + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor));
    //finalColor = (1 - SafeNormalize(diffuseColor));
    //finalColor = (1 - saturate(diffuseColor));
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
    //finalColor = albedo ;
    //finalColor = finalColor* (1+specular3.r);
    //finalColor = diffuseColor* diffuseSSS;
    //finalColor = diffuseColor * _Roughness;
    //finalColor = diffuseColor;
    //finalColor = 1 - diffuseColor;
    //finalColor = (attenuatedLightColor * saturate(NdotL));
    //finalColor = (attenuatedLightColor * saturate(NdotL));
    //截断
    //float finalColor2 = saturate(wrappedNdL * 1.4);
    //区别不大
    //finalColor = (wrappedNdL * 1.4);
    //去掉不需要的
    //finalColor2 = finalColor2 * step(_Roughness, RGBConvertToHSV(finalColor.rgb).z);
    //finalColor = 1-finalColor2 * step(finalColor2, _Roughness);
    //finalColor = finalColor * step(_Roughness, finalColor);
    //finalColor = finalColor - finalColor * step(_Roughness, finalColor);
    //finalColor = diffuseSSS * (1 - SafeNormalize(diffuseColor));
    //finalColor = (diffuseSSS * (1 - diffuseColor)).r * _Roughness * 4 - saturate(1 - diffuseSSS.b);
    /////////////////////////////////////////////////////////////
    //强行截断会导致阴暗情况截断，不可取
    //finalColor = (diffuseSSS * (1 - diffuseColor)).r* 0.4- saturate(1-diffuseSSS.b);
    ////finalColor = finalColor.r *step(_Roughness, finalColor.r);
    //finalColor = finalColor.r * step(0.316, finalColor.r);
    //finalColor = saturate(half3(1, 1, 1) - finalColor);
    /////////////////////////////////////////////////////////////
    //finalColor = saturate(half3(1, 1, 1) - ((diffuseSSS * (1 - SafeNormalize(diffuseColor))).r * 0.4 - saturate(1 - diffuseSSS.b)));
    //finalColor = saturate(half3(1, 1, 1) - ((diffuseSSS * (1 - (diffuseColor))).r * 0.4 - saturate(1 - diffuseSSS.b)));
    //finalColor = ((diffuseSSS * (1 - diffuseColor)).r * _Roughness - saturate(1 - diffuseSSS.b));
    /////////////////////////////////////////////////////////////
    //finalColor = (_SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor)));
    ////////////////////////////////////////
    //方案1，暗部不够鲜艳
    /*float fStep3 = step(diffuseColor.r, _Roughness);
    finalColor = diffuseColor * fStep3+ 1*(1- fStep3);*/
    //截断法处理不了实时阴影
    //太暗
    //finalColor = (_SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor) * _Roughness)) *saturate(diffuseColor);
    //finalColor = (_SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor)* _Roughness))* lerp(0.4,1,saturate(diffuseColor));
    //finalColor = (_SpecColor.rgb * albedo + albedo * diffuseSSS * lerp(0, 2, (1 - SafeNormalize(diffuseColor) * _Roughness))) * lerp(0.4, 1, saturate(diffuseColor));
    //底色 暗红
    //finalColor = _SpecColor.rgb * albedo;
    //加值 棕黄次表面散射
    //finalColor = albedo* diffuseSSS* (1 - SafeNormalize(diffuseColor) * _Roughness * 2);
    //原版
    finalColor = (_SpecColor.rgb * albedo  + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor) * _Roughness*2));
    //暗部变暗了一点点
    //finalColor = (_SpecColor.rgb * albedo * saturate(diffuseColor) + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor) * _Roughness)) ;
    // //方案2
    // 还行  ,配合鲜色过渡，还需要降低一下亮部过曝
    //finalColor = (_SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor) * _Roughness) * saturate(diffuseColor)) ;
    ////////////////////////////////////////
    //finalColor = (_SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor)))* finalColor;
    //finalColor = _SpecColor.rgb * albedo* finalColor + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor)) ;
    //finalColor = _SpecColor.rgb * albedo + albedo * diffuseSSS * (1 - SafeNormalize(diffuseColor))* finalColor;
    //finalColor = 1- diffuseSSS.b;
    //finalColor = (1 - (mainLight.shadowAttenuation));
    //finalColor = (mainLight.shadowAttenuation);
    // 因为环境光的原因，偏黄
    //finalColor = (1 - diffuseColor);
    //finalColor = (1 - lambertColor);
    //finalColor = diffuseSSS;
    //finalColor = max(specular3, finalColor);
    //finalColor = finalColor * step(finalColor, _Roughness);
    //finalColor = finalColor2;
    //finalColor = finalColor + finalColor*finalColor2;
    //调试
    //finalColor = wrappedNdL * _Roughness * 2;
    //finalColor = finalColor - (1- diffuseColor) * _Roughness * 2;
    // 调试
    //finalColor = diffuseSSS* (1 - SafeNormalize(diffuseColor)* _Roughness*2);
    //81融合增加细节
    //finalColor = diffuseSSS * (1 - SafeNormalize(diffuseColor));
    //黄阴影
    //finalColor = diffuseSSS * (1 - SafeNormalize(diffuseColor) * 1.4);
    //finalColor = diffuse3 + ambient ;
    //finalColor = diffuse3;
    //finalColor = diffuseSSS;
    //finalColor = SafeNormalize(diffuseSSS);
    //finalColor = abs(diffuseColor.b - diffuseSSS.b);
    //finalColor = albedo;
    //finalColor = lerp(finalColor, inputData.bakedGI * occ, _Roughness);
    //finalColor = lerp(finalColor, occ, _Roughness);
    
    //finalColor = lerp(finalColor, specular3, _Roughness)* attenuatedLightColor;
    //finalColor = lerp(finalColor, specular3, _Roughness) * diffuseColor;
    finalColor = max(finalColor,specular3);
	//half3 finalColor = inputData.bakedGI + diffuse3 + ambient;
    //half3 finalColor = diffuseColor * diffuse2;
    //half3 finalColor = diffuseColor * diffuse2 + emission;

//#if defined(_SPECGLOSSMAP) || defined(_SPECULAR_COLOR)
//    finalColor += specularColor;
//#endif

    half4 color = half4(finalColor, alpha);
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

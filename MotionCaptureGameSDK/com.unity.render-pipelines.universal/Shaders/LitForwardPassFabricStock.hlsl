#ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED_FS
#define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED_FS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/BSDF.hlsl"

// GLES2 has limited amount of interpolators
#if defined(_PARALLAXMAP) && !defined(SHADER_API_GLES)
#define REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR
#endif

#if (defined(_NORMALMAP) || (defined(_PARALLAXMAP) && !defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR))) || defined(_DETAIL)
#define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

// keep this file in sync with LitGBufferPass.hlsl

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float2 lightmapUV   : TEXCOORD1;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv                       : TEXCOORD0;
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    float3 positionWS               : TEXCOORD2;
#endif

    float3 normalWS                 : TEXCOORD3;
//#if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    float4 tangentWS                : TEXCOORD4;    // xyz: tangent, w: sign
//#endif
    float3 viewDirWS                : TEXCOORD5;

    half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord              : TEXCOORD7;
#endif

#if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    float3 viewDirTS                : TEXCOORD8;
#endif

    float4 positionCS               : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData, float3 bitangent)
{
    inputData = (InputData)0;

#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    inputData.positionWS = input.positionWS;
#endif
    //inputData.tangentWS = input.tangentWS;
    half3 viewDirWS = SafeNormalize(input.viewDirWS);
#if defined(_NORMALMAP) || defined(_DETAIL)
    //float sgn = input.tangentWS.w;      // should be either +1 or -1
    //float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
    inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));
#else
    inputData.normalWS = input.normalWS;
#endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
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
    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
}

///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////

// Used in Standard (Physically Based) shader
Varyings LitPassVertex(Attributes input)
{
    Varyings output = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

    // normalWS and tangentWS already normalize.
    // this is required to avoid skewing the direction during interpolation
    // also required for per-vertex lighting and SH evaluation
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

    half3 viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
    half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);

    // already normalized from normal transform to WS.
    output.normalWS = normalInput.normalWS;
    output.viewDirWS = viewDirWS;
//#if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR) || defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    real sign = input.tangentOS.w * GetOddNegativeScale();
    half4 tangentWS = half4(normalInput.tangentWS.xyz, sign);
//#endif
//#if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    output.tangentWS = tangentWS;
//#endif

//#if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
//    half3 viewDirTS = GetViewDirectionTangentSpace(tangentWS, output.normalWS, viewDirWS);
//    output.viewDirTS = viewDirTS;
//#endif

    OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

    output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    output.positionWS = vertexInput.positionWS;
#endif

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    output.shadowCoord = GetShadowCoord(vertexInput);
#endif

    output.positionCS = vertexInput.positionCS;
    //float sgn2 = _Smoothness;
    //float sgn = _Anisotropy;
    
    return output;
}
//real PerceptualRoughnessToRoughness(real perceptualRoughness)
//{
//    return perceptualRoughness * perceptualRoughness;
//}

//void ConvertValueAnisotropyToValueTB(real value, real anisotropy, out real valueT, out real valueB)
//{
//    // Use the parametrization of Sony Imageworks.
//    // Ref: Revisiting Physically Based Shading at Imageworks, p. 15.
//    valueT = value * (1 + anisotropy);
//    valueB = value * (1 - anisotropy);
//}

//void ConvertAnisotropyToRoughness(real perceptualRoughness, real anisotropy, out real roughnessT, out real roughnessB)
//{
//    real roughness = PerceptualRoughnessToRoughness(perceptualRoughness);
//    ConvertValueAnisotropyToValueTB(roughness, anisotropy, roughnessT, roughnessB);
//}
//void GetBSDFAngle2(float3 V, float3 L, float NdotL, float NdotV,
//    out float LdotV, out float NdotH, out float LdotH, out float invLenLV)
//{
//    // Optimized math. Ref: PBR Diffuse Lighting for GGX + Smith Microsurfaces (slide 114).
//    LdotV = dot(L, V);
//    invLenLV = rsqrt(max(2.0 * LdotV + 2.0, FLT_EPS));    // invLenLV = rcp(length(L + V)), clamp to avoid rsqrt(0) = inf, inf * 0 = NaN
//    NdotH = saturate((NdotL + NdotV) * invLenLV);
//    LdotH = saturate(invLenLV * LdotV + invLenLV);
//}
//float F_Schlick2(float f0, float f90, float u)
//{
//    float x = 1.0 - u;
//    float x2 = x * x;
//    float x5 = x * x2 * x2;
//    return (f90 - f0) * x5 + f0;                // sub mul mul mul sub mad
//}
//
//float F_Schlick2(float f0, float u)
//{
//    return F_Schlick(f0, 1.0, u);               // sub mul mul mul sub mad
//}
//
//float3 F_Schlick2(float3 f0, float f90, float u)
//{
//    float x = 1.0 - u;
//    float x2 = x * x;
//    float x5 = x * x2 * x2;
//    return f0 * (1.0 - x5) + (f90 * x5);        // sub mul mul mul sub mul mad*3
//}
//
//float3 F_Schlick2(float3 f0, float u)
//{
//    return F_Schlick(f0, 1.0, u);               // sub mul mul mul sub mad*3
//}
float DV_SmithJointGGXAniso2(float TdotH, float BdotH, float NdotH, float NdotV,
    float TdotL, float BdotL, float NdotL,
    float roughnessT, float roughnessB, float partLambdaV)
{
    float a2 = roughnessT * roughnessB;
    float3 v = float3(roughnessB * TdotH, roughnessT * BdotH, a2 * NdotH);
    float  s = dot(v, v);

    float lambdaV = NdotL * partLambdaV;
    float lambdaL = NdotV * length(float3(roughnessT * TdotL, roughnessB * BdotL, NdotL));

    float2 D = float2(a2 * a2 * a2, s * s);  // Fraction without the multiplier (1/Pi)
    float2 G = float2(1, lambdaV + lambdaL); // Fraction without the multiplier (1/2)

    // This function is only used for direct lighting.
    // If roughness is 0, the probability of hitting a punctual or directional light is also 0.
    // Therefore, we return 0. The most efficient way to do it is with a max().
    //return (INV_PI * 0.5) * (D.x * G.x) / max(D.y * G.y, REAL_MIN);
    return (INV_PI * 0.5) * (D.x * G.x) / max(D.y * G.y, FLT_MIN);
}

float DV_SmithJointGGXAniso2(float TdotH, float BdotH, float NdotH,
    float TdotV, float BdotV, float NdotV,
    float TdotL, float BdotL, float NdotL,
    float roughnessT, float roughnessB)
{
    float partLambdaV = GetSmithJointGGXAnisoPartLambdaV(TdotV, BdotV, NdotV, roughnessT, roughnessB);
    return DV_SmithJointGGXAniso(TdotH, BdotH, NdotH, NdotV,
        TdotL, BdotL, NdotL,
        roughnessT, roughnessB, partLambdaV);
}
half3 DirectBRDFSpecularAnisotropy(BRDFData brdfData, float3 N, float3 L, float3 V, float3 tangentWS, float3 bitangentWS)
{
    float NdotL = dot(N, L);
    float NdotV = dot(N, V);
    float clampedNdotV = ClampNdotV(NdotV);
    float clampedNdotL = saturate(NdotL);
    float LdotV = 0, NdotH = 0, LdotH = 0, invLenLV = 0;
    GetBSDFAngle(V, L, NdotL, NdotV, LdotV, NdotH, LdotH, invLenLV);
    float3 F = F_Schlick(_SpecColor.rgb, LdotH);

    float DV;
    float3 H = (L + V) * invLenLV;
    float TdotL = dot(tangentWS, L);
    float TdotH = dot(tangentWS, H);
    float BdotH = dot(bitangentWS, H);
    float BdotL = dot(bitangentWS, L);
    float roughnessT = 0, roughnessB = 0;
    ConvertAnisotropyToRoughness(brdfData.perceptualRoughness, _Anisotropy, roughnessT, roughnessB);
    //ConvertAnisotropyToRoughness(brdfData.perceptualRoughness, 0, roughnessT, roughnessB);
    DV = DV_SmithJointGGXAniso2(TdotH, BdotH, NdotH, clampedNdotV, TdotL, BdotL, abs(NdotL),
        roughnessT, roughnessB, 1);

    //float diffTerm = DisneyDiffuse(clampedNdotV, abs(NdotL), LdotV, brdfData.perceptualRoughness) * clampedNdotL;

    float3 specTerm = F * DV;
    //float3 specTerm = F * lerp(_Anisotropy0, _Anisotropy1, DV);
    //float3 specTerm = F * lerp(_Anisotropy0, _Anisotropy1, saturate(DV));
    
    //specTerm *= clampedNdotL;
    specTerm *= clampedNdotL;
    //specTerm = saturate(specTerm);
    //specTerm = lerp(_Anisotropy0, _Anisotropy1, specTerm);
    //specTerm = _Anisotropy1*specTerm;
    specTerm = sqrt(specTerm / _Anisotropy0) + _Anisotropy1;
    //specTerm = min(2,sqrt(specTerm/ _Anisotropy0)+ _Anisotropy1);
    //return 1;
    //return 0;
    //_BaseColor
    return specTerm;
    //return min(specTerm,3);
}
//
//half DirectBRDFSpecularFabric(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
//{
//    float3 halfDir = SafeNormalize(float3(lightDirectionWS)+float3(viewDirectionWS));
//
//    float NoH = saturate(dot(normalWS, halfDir));
//    half LoH = saturate(dot(lightDirectionWS, halfDir));
//
//    // GGX Distribution multiplied by combined approximation of Visibility and Fresnel
//    // BRDFspec = (D * V * F) / 4.0
//    // D = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2
//    // V * F = 1.0 / ( LoH^2 * (roughness + 0.5) )
//    // See "Optimizing PBR for Mobile" from Siggraph 2015 moving mobile graphics course
//    // https://community.arm.com/events/1155
//
//    // Final BRDFspec = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2 * (LoH^2 * (roughness + 0.5) * 4.0)
//    // We further optimize a few light invariant terms
//    // brdfData.normalizationTerm = (roughness + 0.5) * 4.0 rewritten as roughness * 4.0 + 2.0 to a fit a MAD.
//    float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;
//
//    half LoH2 = LoH * LoH;
//    half specularTerm = brdfData.roughness2 / ((d * d) * max(0.1h, LoH2) * brdfData.normalizationTerm);
//
//    // On platforms where half actually means something, the denominator has a risk of overflow
//    // clamp below was added specifically to "fix" that, but dx compiler (we convert bytecode to metal/gles)
//    // sees that specularTerm have only non-negative terms, so it skips max(0,..) in clamp (leaving only min(100,...))
//#if defined (SHADER_API_MOBILE) || defined (SHADER_API_SWITCH)
//    specularTerm = specularTerm - HALF_MIN;
//    specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
//#endif
//
//    return specularTerm;
//}
//对比度饱和度调节
half3 colorAdjust(half3 Color, half _Saturation, half _Contrast)
{

    half3 finalColor = Color;
    half gray = 0.2125 * Color.r + 0.7154 * Color.g + 0.0721 * Color.b;
    half3 grayColor = half3(gray, gray, gray);
    finalColor = lerp(grayColor, finalColor, _Saturation);
    half3 avgColor = half3(0.5, 0.5, 0.5);
    finalColor = lerp(avgColor, finalColor, _Contrast);

    return finalColor;
}
half3 LightingPhysicallyBasedFabricStock(BRDFData brdfData, BRDFData brdfDataClearCoat,
    half3 lightColor, half3 lightDirectionWS, half lightAttenuation,
    half3 normalWS, half3 viewDirectionWS,
    half clearCoatMask, bool specularHighlightsOff,half3 tangentWS, half3 bitangentWS)
{
    half NdotL = saturate(dot(normalWS, lightDirectionWS));
    NdotL = lerp(_Base0, _Base1, lightAttenuation * NdotL);
    half3 radiance = lightColor * NdotL;
    //lightAttenuation = lerp(_Base0, _Base1, lightAttenuation);
    //half3 radiance = lightColor * (lightAttenuation * NdotL);
    
    //half _Anisotropy0;
    //half _Anisotropy1;
    //half _Base0;
    //half _Base1;

    //half3 brdf = lerp(_Base0, _Base1,brdfData.diffuse);
    half3 brdf = brdfData.diffuse;
    //half3 brdf = _BaseColor;
    //half3 brdf = 0;
#ifndef _SPECULARHIGHLIGHTS_OFF
    [branch] if (!specularHighlightsOff)
    {
        /*brdf += brdfData.specular * max(DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS)
            ,DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS+ _AnisotropyVec3, viewDirectionWS+ _AnisotropyVec3, tangentWS, bitangentWS));*/
        //奇怪的类黄金材质
        /*brdf += brdfData.specular * max(DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS)
            , DirectBRDFSpecularAnisotropy(brdfData, normalWS, half3(_AnisotropyVec3.r * -lightDirectionWS.x, lightDirectionWS.y, _AnisotropyVec3.r * -lightDirectionWS.z), viewDirectionWS + _AnisotropyVec3, tangentWS, bitangentWS));*/
        //取max在阴暗面还是会有边界
        /*brdf += brdfData.specular * max(DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS)
            , DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS - _AnisotropyVec3, viewDirectionWS + _AnisotropyVec3, tangentWS, bitangentWS));*/
        //叠加污渍
        /*brdf += brdfData.specular * (DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS)
            +DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS - _AnisotropyVec3, viewDirectionWS + _AnisotropyVec3, tangentWS, bitangentWS));*/
        /*half3 h3S = max(DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS)
            , DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS - _AnisotropyVec3, viewDirectionWS + _AnisotropyVec3, tangentWS, bitangentWS));
        brdf +=  brdfData.specular * max(h3S
            , DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS + _AnisotropyVec3, viewDirectionWS + _AnisotropyVec3, tangentWS, bitangentWS));*/
        /*half bS = brdfData.specular * DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS);
        brdf += lerp(_Anisotropy0, _Anisotropy1, bS);*/
        //原始
        //brdf += brdfData.specular * DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS);
        // /////////////////////////////////////////////
        //相反光
        //brdf += brdfData.specular * (DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS)+ _AnisotropyVec3.g*DirectBRDFSpecularAnisotropy(brdfData,  normalWS, -_AnisotropyVec3.r* lightDirectionWS,  viewDirectionWS,  tangentWS, bitangentWS));
        /////////////////////////////////////////////
        //half a = 45.0* PI / 180.0;
        //45度光
        float a = _AnisotropyVec3.b *2* PI ;
        float3x3 tangent_roate = float3x3(cos(a), sin(a), 0,
            sin(a), cos(a), 0,
            0, 0, 1
            );
        float3 tangentWS2 = mul(tangent_roate, tangentWS);
        float3 bitangentWS2 = cross(normalWS, tangentWS);
        //brdf += brdfData.specular * (DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS) );
        //brdf += SafeNormalize(brdfData.specular * (DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS)));
        brdf += brdfData.specular * (DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS) + _AnisotropyVec3.g * DirectBRDFSpecularAnisotropy(brdfData, normalWS,  lightDirectionWS, viewDirectionWS, tangentWS2, bitangentWS2));
        /////////////////////////////////////////////
        //lightDirectionWS viewDirectionWS
        //half NoV = saturate(dot(normalWS, viewDirectionWS));
        //half fresnelTerm = Pow4(1.0 - NoV);
        //brdf += fresnelTerm * 0.1;
        //侧面补光,不能转，得在外面算出位置，然后求lightDirectionWS2
        /*half3 h3LD = -_AnisotropyVec3.r * lightDirectionWS;
        h3LD = cross(h3LD, lightDirectionWS);
        brdf += brdfData.specular * (DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS) + _AnisotropyVec3.g * DirectBRDFSpecularAnisotropy(brdfData, normalWS, h3LD, viewDirectionWS, tangentWS, bitangentWS));*/

        /*half3 h3Temp = DirectBRDFSpecularAnisotropy(brdfData, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS);
        brdf = brdf * (1-h3Temp.r) + brdfData.specular * h3Temp.r;*/
        //return 0;
#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
        // Clear coat evaluates the specular a second timw and has some common terms with the base specular.
        // We rely on the compiler to merge these and compute them only once.
        half brdfCoat = kDielectricSpec.r * DirectBRDFSpecularAnisotropy(brdfDataClearCoat, normalWS, lightDirectionWS, viewDirectionWS, tangentWS, bitangentWS);

        // Mix clear coat and base layer using khronos glTF recommended formula
        // https://github.com/KhronosGroup/glTF/blob/master/extensions/2.0/Khronos/KHR_materials_clearcoat/README.md
        // Use NoV for direct too instead of LoH as an optimization (NoV is light invariant).
        half NoV = saturate(dot(normalWS, viewDirectionWS));
        // Use slightly simpler fresnelTerm (Pow4 vs Pow5) as a small optimization.
        // It is matching fresnel used in the GI/Env, so should produce a consistent clear coat blend (env vs. direct)
        half coatFresnel = kDielectricSpec.x + kDielectricSpec.a * Pow4(1.0 - NoV);

        brdf = brdf * (1.0 - clearCoatMask * coatFresnel) + brdfCoat * clearCoatMask;
#endif // _CLEARCOAT
    }
#endif // _SPECULARHIGHLIGHTS_OFF
    
    return brdf * radiance;
    //return brdf ;
}
half3 LightingPhysicallyBasedFabricStock(BRDFData brdfData, BRDFData brdfDataClearCoat, Light light, half3 normalWS, half3 viewDirectionWS, half clearCoatMask, bool specularHighlightsOff, half3 tangentWS, half3 bitangentWS)
{
    return LightingPhysicallyBasedFabricStock(brdfData, brdfDataClearCoat, light.color, light.direction, light.distanceAttenuation * light.shadowAttenuation, normalWS, viewDirectionWS, clearCoatMask, specularHighlightsOff, tangentWS, bitangentWS);
}
half3 GlobalIlluminationFabricStock(BRDFData brdfData, BRDFData brdfDataClearCoat, float clearCoatMask,
    half3 bakedGI, half occlusion,
    half3 normalWS, half3 viewDirectionWS)
{
    half3 reflectVector = reflect(-viewDirectionWS, normalWS);
    half NoV = saturate(dot(normalWS, viewDirectionWS));
    half fresnelTerm = Pow4(1.0 - NoV);

    half3 indirectDiffuse = bakedGI * occlusion;
    half3 indirectSpecular = GlossyEnvironmentReflection(reflectVector, brdfData.perceptualRoughness, occlusion);

    half3 color = _Cutoff *EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);

#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
    half3 coatIndirectSpecular = GlossyEnvironmentReflection(reflectVector, brdfDataClearCoat.perceptualRoughness, occlusion);
    // TODO: "grazing term" causes problems on full roughness
    half3 coatColor = EnvironmentBRDFClearCoat(brdfDataClearCoat, clearCoatMask, coatIndirectSpecular, fresnelTerm);

    // Blend with base layer using khronos glTF recommended way using NoV
    // Smooth surface & "ambiguous" lighting
    // NOTE: fresnelTerm (above) is pow4 instead of pow5, but should be ok as blend weight.
    half coatFresnel = kDielectricSpec.x + kDielectricSpec.a * fresnelTerm;
    return color * (1.0 - coatFresnel * clearCoatMask) + coatColor;
#else
    return color;
#endif
}
//half MainLightRealtimeShadowFabricStock(float4 shadowCoord)
//{
//#if !defined(MAIN_LIGHT_CALCULATE_SHADOWS)
//    return 1.0h;
//#endif
//
//    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
//    half4 shadowParams = GetMainLightShadowParams();
//    //shadowParams = 0;
//    return SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData, shadowParams, false);
//}
//half MainLightShadowFabricStock(float4 shadowCoord, float3 positionWS, half4 shadowMask, half4 occlusionProbeChannels)
//{
//    half realtimeShadow = MainLightRealtimeShadowFabricStock(shadowCoord);
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
//Light GetMainLightFabricStock(float4 shadowCoord, float3 positionWS, half4 shadowMask)
//{
//    Light light = GetMainLight();
//    //light.shadowAttenuation = 0.5*MainLightShadowFabricStock(shadowCoord, positionWS, shadowMask, _MainLightOcclusionProbes);
//    light.shadowAttenuation = MainLightShadowFabricStock(shadowCoord, positionWS, shadowMask, _MainLightOcclusionProbes);
//    return light;
//}
half4 UniversalFragmentFabricStockPBR(InputData inputData, SurfaceData surfaceData, half3 tangentWS, half3 bitangentWS)
{
#ifdef _SPECULARHIGHLIGHTS_OFF
    bool specularHighlightsOff = true;
#else
    bool specularHighlightsOff = false;
#endif

    BRDFData brdfData;

    // NOTE: can modify alpha
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

    BRDFData brdfDataClearCoat = (BRDFData)0;
#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
    // base brdfData is modified here, rely on the compiler to eliminate dead computation by InitializeBRDFData()
    InitializeBRDFDataClearCoat(surfaceData.clearCoatMask, surfaceData.clearCoatSmoothness, brdfData, brdfDataClearCoat);
#endif

    // To ensure backward compatibility we have to avoid using shadowMask input, as it is not present in older shaders
#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
    half4 shadowMask = inputData.shadowMask;
#elif !defined (LIGHTMAP_ON)
    half4 shadowMask = unity_ProbesOcclusion;
#else
    half4 shadowMask = half4(1, 1, 1, 1);
#endif

    Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, shadowMask);
    mainLight.shadowAttenuation *= _EnvLight;
#if defined(_SCREEN_SPACE_OCCLUSION)
    AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(inputData.normalizedScreenSpaceUV);
    mainLight.color *= aoFactor.directAmbientOcclusion;
    surfaceData.occlusion = min(surfaceData.occlusion, aoFactor.indirectAmbientOcclusion);
#endif

    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI);
    half3 color = GlobalIlluminationFabricStock(brdfData, brdfDataClearCoat, surfaceData.clearCoatMask,
        inputData.bakedGI, surfaceData.occlusion,
        inputData.normalWS, inputData.viewDirectionWS);
    color += LightingPhysicallyBasedFabricStock(brdfData, brdfDataClearCoat,
        mainLight,
        inputData.normalWS, inputData.viewDirectionWS,
        surfaceData.clearCoatMask, specularHighlightsOff, tangentWS, bitangentWS);

#ifdef _ADDITIONAL_LIGHTS
    uint pixelLightCount = GetAdditionalLightsCount();
    for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
    {
        Light light = GetAdditionalLight(lightIndex, inputData.positionWS, shadowMask);
#if defined(_SCREEN_SPACE_OCCLUSION)
        light.color *= aoFactor.directAmbientOcclusion;
#endif
        color += LightingPhysicallyBasedFabricStock(brdfData, brdfDataClearCoat,
            light,
            inputData.normalWS, inputData.viewDirectionWS,
            surfaceData.clearCoatMask, specularHighlightsOff, tangentWS, bitangentWS);
    }
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX
    color += inputData.vertexLighting * brdfData.diffuse;
#endif

    color += surfaceData.emission;
    //color += 0.1;

    return half4(color, surfaceData.alpha);
}
// Used in Standard (Physically Based) shader
half4 LitPassFragment(Varyings input) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

#if defined(_PARALLAXMAP)
#if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    half3 viewDirTS = input.viewDirTS;
#else
    half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, input.viewDirWS);
#endif
    ApplyPerPixelDisplacement(viewDirTS, input.uv);
#endif

    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

    InputData inputData;
    float sgn = input.tangentWS.w;      // should be either +1 or -1
    float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
    /*sgn = _Anisotropy;*/
    //sgn = GetAniso();
    
    InitializeInputData(input, surfaceData.normalTS, inputData, bitangent);

    half4 color = UniversalFragmentFabricStockPBR(inputData, surfaceData, input.tangentWS.rgb,bitangent);

    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    color.a = OutputAlpha(color.a, _Surface);

    return color;
}

#endif

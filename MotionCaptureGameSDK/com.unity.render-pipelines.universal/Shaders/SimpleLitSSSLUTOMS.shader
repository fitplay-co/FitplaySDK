// Shader targeted for low end devices. Single Pass Forward Rendering.
Shader "Universal Render Pipeline/Simple Lit SSSLUTOMS"
{
    // Keep properties of StandardSpecular shader for upgrade reasons.
    Properties
    {

        //_Exposure("Exposure", Range(0.0, 2.0)) = 1.0
        //[Toggle] _Tonemap("Use Tone Mapping", Float) = 1
        //_BumpScale("BumpScale", Range(0, 2)) = 1
        //_NormalBlurredBias("NormalBlurredBias", Range(0, 10)) = 3
        //// Curvature LUT
        //[NoScaleOffset] _CurvatureTex_LUT("Curvature LUT", 2D) = "black" {}
        ////_CurvatureBias("CurvatureBias", Vector) = (1, 0, 0, 0)
        //// Shadow LUT
        //[NoScaleOffset] _ShadowTex_LUT("Shadow LUT", 2D) = "black" {}
        ////_ShadowBias("ShadowBias", Vector) = (1, 0, 0, 0)

        [MainTexture] _BaseMap("Base Map (RGB) Smoothness / Alpha (A)", 2D) = "white" {}
        [MainColor]   _BaseColor("Base Color", Color) = (1, 1, 1, 1)
            
        _Cutoff("Alpha Clipping", Range(0.0, 1.0)) = 0.5

        _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
        _SpecGlossMap("Specular Map", 2D) = "white" {}
        _OccGlossMap("Occ Map", 2D) = "white" {}
        
        [Enum(Specular Alpha,0,Albedo Alpha,1)] _SmoothnessSource("Smoothness Source", Float) = 0.0
        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0

        _BumpScale("Bump Scale", Float) = 1.0
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        /*[HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
        [NoScaleOffset]_EmissionMap("Emission Map", 2D) = "white" {}*/
        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0

        [ToggleOff] _ReceiveShadows("Receive Shadows", Float) = 1.0

        // Editmode props
        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0
        [HideInInspector] _Smoothness("Smoothness", Float) = 0.5

        // ObsoleteProperties
        //[HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
        //[HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
        //[HideInInspector] _Shininess("Smoothness", Float) = 0.0
        //[HideInInspector] _GlossinessSource("GlossinessSource", Float) = 0.0
        //[HideInInspector] _SpecSource("SpecularHighlights", Float) = 0.0

        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}


        /////////////////////////////////////////////
            _CurveTex("CurveMap", 2D) = "white" {}
            /*_BumpTex("Normal Map", 2D) = "bump" {}
            _BumpScale("Bump Scale", Float) = 1.0*/
            //_Color("Color Tint", Color) = (1, 1, 1, 1)
            //_Specular("Specular", Color) = (1, 1, 1, 1)
            //_tuneNormalBlur("tuneNormalBlur", Color) = (1, 1, 1, 1)
            _SSSLUT("SSSLUT", 2D) = "white" {}
            _SpecularLUT("BeckmannLUT", 2D) = "white" {}
            _Brightness("Brightness", Range(0, 1)) = 1
            _Roughness("Roughness", Range(0, 0.999)) = 0.999
            _CurveFactor("CurveRate",Range(1,4)) = 1
            _Occ("Occ", Range(0.0, 16.0)) = 1.0
            _LutRemap0("LutRemap0", Range(0, 16)) = 0
            _LutRemap1("LutRemap1", Range(0, 16)) = 1
            _SpcRemap0("SpcRemap0", Range(0, 1)) = 0
            _SpcRemap1("SpcRemap1", Range(0, 1)) = 1
            _Alpha0("Alpha0", Range(0, 1)) = 0
            _Alpha1("Alpha1", Range(0, 1)) = 1
                /////////////////////////////////////////////
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "SimpleLit" "IgnoreProjector" = "False" "ShaderModel"="4.5"}
        LOD 300
        //Pass
        //{

        //    HLSLPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag

        //    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        //    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float2 uv : TEXCOORD0;
        //        float3 normal : NORMAL;

        //    };

        //    struct v2f
        //    {
        //        float2 uv : TEXCOORD0;
        //        float4 vertex : SV_POSITION;


        //    };


        //    v2f vert(appdata v)
        //    {
        //        v2f o;
        //        //v.vertex.xyz += v.normal * 0.002;
        //        o.vertex = TransformObjectToHClip(v.vertex);


        //        return o;
        //    }

        //    half4 frag(v2f i) : SV_Target
        //    {
        //        return half4(1,0,0,1) * _MainLightColor;
        //    }
        //    ENDHLSL
        //}
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            // Use same blending / depth states as Standard shader
            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
            #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
            #pragma shader_feature_local _NORMALMAP
            //#pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _SIMPLIFY_OFF
            #pragma shader_feature_local _DIFFUSE_OFF
            #pragma shader_feature_local _DIFFUSEA_RVT
            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma vertex LitPassVertexSimple
            #pragma fragment LitPassFragmentSimpleSSSLUT
            #define BUMP_SCALE_NOT_SUPPORTED 1

            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitForwardPassSSSLUTOMS.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

    //    Pass
    //    {
    //        Name "GBuffer"
    //        Tags{"LightMode" = "UniversalGBuffer"}

    //        ZWrite[_ZWrite]
    //        ZTest LEqual
    //        Cull[_Cull]

    //        HLSLPROGRAM
    //        #pragma exclude_renderers gles gles3 glcore
    //        #pragma target 4.5

    //        // -------------------------------------
    //        // Material Keywords
    //        #pragma shader_feature_local_fragment _ALPHATEST_ON
    //        //#pragma shader_feature _ALPHAPREMULTIPLY_ON
    //        #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
    //        #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
    //        #pragma shader_feature_local _NORMALMAP
    //        //#pragma shader_feature_local_fragment _EMISSION
    //        #pragma shader_feature_local _RECEIVE_SHADOWS_OFF

    //        // -------------------------------------
    //        // Universal Pipeline keywords
    //        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
    //        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
    //        //#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
    //        //#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
    //        #pragma multi_compile _ _SHADOWS_SOFT
    //        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

    //        // -------------------------------------
    //        // Unity defined keywords
    //        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
    //        #pragma multi_compile _ LIGHTMAP_ON
    //        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

    //        //--------------------------------------
    //        // GPU Instancing
    //        #pragma multi_compile_instancing
    //        #pragma multi_compile _ DOTS_INSTANCING_ON

    //        #pragma vertex LitPassVertexSimple
    //        #pragma fragment LitPassFragmentSimple
    //        #define BUMP_SCALE_NOT_SUPPORTED 1

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitGBufferPass.hlsl"
    //        ENDHLSL
    //    }

    //    Pass
    //    {
    //        Name "DepthOnly"
    //        Tags{"LightMode" = "DepthOnly"}

    //        ZWrite On
    //        ColorMask 0
    //        Cull[_Cull]

    //        HLSLPROGRAM
    //        #pragma exclude_renderers gles gles3 glcore
    //        #pragma target 4.5

    //        #pragma vertex DepthOnlyVertex
    //        #pragma fragment DepthOnlyFragment

    //        // -------------------------------------
    //        // Material Keywords
    //        #pragma shader_feature_local_fragment _ALPHATEST_ON
    //        #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    //        //--------------------------------------
    //        // GPU Instancing
    //        #pragma multi_compile_instancing
    //        #pragma multi_compile _ DOTS_INSTANCING_ON

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
    //        ENDHLSL
    //    }

    //    // This pass is used when drawing to a _CameraNormalsTexture texture
    //    Pass
    //    {
    //        Name "DepthNormals"
    //        Tags{"LightMode" = "DepthNormals"}

    //        ZWrite On
    //        Cull[_Cull]

    //        HLSLPROGRAM
    //        #pragma exclude_renderers gles gles3 glcore
    //        #pragma target 4.5

    //        #pragma vertex DepthNormalsVertex
    //        #pragma fragment DepthNormalsFragment

    //        // -------------------------------------
    //        // Material Keywords
    //        #pragma shader_feature_local _NORMALMAP
    //        #pragma shader_feature_local_fragment _ALPHATEST_ON
    //        #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    //        //--------------------------------------
    //        // GPU Instancing
    //        #pragma multi_compile_instancing
    //        #pragma multi_compile _ DOTS_INSTANCING_ON

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthNormalsPass.hlsl"
    //        ENDHLSL
    //    }

    //    // This pass it not used during regular rendering, only for lightmap baking.
    //    Pass
    //    {
    //        Name "Meta"
    //        Tags{ "LightMode" = "Meta" }

    //        Cull Off

    //        HLSLPROGRAM
    //        #pragma exclude_renderers gles gles3 glcore
    //        #pragma target 4.5

    //        #pragma vertex UniversalVertexMeta
    //        #pragma fragment UniversalFragmentMetaSimple

    //        //#pragma shader_feature_local_fragment _EMISSION
    //        #pragma shader_feature_local_fragment _SPECGLOSSMAP

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitMetaPassSSSLUT.hlsl"

    //        ENDHLSL
    //    }
    //    Pass
    //    {
    //        Name "Universal2D"
    //        Tags{ "LightMode" = "Universal2D" }
    //        Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

    //        HLSLPROGRAM
    //        #pragma exclude_renderers gles gles3 glcore
    //        #pragma target 4.5

    //        #pragma vertex vert
    //        #pragma fragment frag
    //        #pragma shader_feature_local_fragment _ALPHATEST_ON
    //        #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
    //        ENDHLSL
    //    }
    //}

    //SubShader
    //{
    //    Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "SimpleLit" "IgnoreProjector" = "True" "ShaderModel"="2.0"}
    //    LOD 300

    //    Pass
    //    {
    //        Name "ForwardLit"
    //        Tags { "LightMode" = "UniversalForward" }

    //        // Use same blending / depth states as Standard shader
    //        Blend[_SrcBlend][_DstBlend]
    //        ZWrite[_ZWrite]
    //        Cull[_Cull]

    //        HLSLPROGRAM
    //        #pragma only_renderers gles gles3 glcore d3d11
    //        #pragma target 2.0

    //        // -------------------------------------
    //        // Material Keywords
    //        #pragma shader_feature_local_fragment _ALPHATEST_ON
    //        #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
    //        #pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
    //        #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
    //        #pragma shader_feature_local _NORMALMAP
    //        //#pragma shader_feature_local_fragment _EMISSION
    //        #pragma shader_feature_local _RECEIVE_SHADOWS_OFF

    //        // -------------------------------------
    //        // Universal Pipeline keywords
    //        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
    //        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
    //        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
    //        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
    //        #pragma multi_compile_fragment _ _SHADOWS_SOFT
    //        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
    //        #pragma multi_compile _ SHADOWS_SHADOWMASK
    //        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION


    //        // -------------------------------------
    //        // Unity defined keywords
    //        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
    //        #pragma multi_compile _ LIGHTMAP_ON
    //        #pragma multi_compile_fog

    //        #pragma vertex LitPassVertexSimple
    //        #pragma fragment LitPassFragmentSimpleSSSLUT
    //        #define BUMP_SCALE_NOT_SUPPORTED 1

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitForwardPassSSSLUTOMS.hlsl"
    //        ENDHLSL
    //    }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma only_renderers gles gles3 glcore d3d11
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

    //    Pass
    //    {
    //        Name "DepthOnly"
    //        Tags{"LightMode" = "DepthOnly"}

    //        ZWrite On
    //        ColorMask 0
    //        Cull[_Cull]

    //        HLSLPROGRAM
    //        #pragma only_renderers gles gles3 glcore d3d11
    //        #pragma target 2.0

    //        #pragma vertex DepthOnlyVertex
    //        #pragma fragment DepthOnlyFragment

    //        // Material Keywords
    //        #pragma shader_feature_local_fragment _ALPHATEST_ON
    //        #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
    //        ENDHLSL
    //    }

    //    // This pass is used when drawing to a _CameraNormalsTexture texture
    //    Pass
    //    {
    //        Name "DepthNormals"
    //        Tags{"LightMode" = "DepthNormals"}

    //        ZWrite On
    //        Cull[_Cull]

    //        HLSLPROGRAM
    //        #pragma only_renderers gles gles3 glcore d3d11
    //        #pragma target 2.0

    //        #pragma vertex DepthNormalsVertex
    //        #pragma fragment DepthNormalsFragment

    //        // -------------------------------------
    //        // Material Keywords
    //        #pragma shader_feature_local _NORMALMAP
    //        #pragma shader_feature_local_fragment _ALPHATEST_ON
    //        #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA

    //        //--------------------------------------
    //        // GPU Instancing
    //        #pragma multi_compile_instancing

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthNormalsPass.hlsl"
    //        ENDHLSL
    //    }

    //    // This pass it not used during regular rendering, only for lightmap baking.
    //    Pass
    //    {
    //        Name "Meta"
    //        Tags{ "LightMode" = "Meta" }

    //        Cull Off

    //        HLSLPROGRAM
    //        #pragma only_renderers gles gles3 glcore d3d11
    //        #pragma target 2.0

    //        #pragma vertex UniversalVertexMeta
    //        #pragma fragment UniversalFragmentMetaSimple

    //        //#pragma shader_feature_local_fragment _EMISSION
    //        #pragma shader_feature_local_fragment _SPECGLOSSMAP

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitMetaPassSSSLUT.hlsl"

    //        ENDHLSL
    //    }
    //    Pass
    //    {
    //        Name "Universal2D"
    //        Tags{ "LightMode" = "Universal2D" }
    //        Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

    //        HLSLPROGRAM
    //        #pragma only_renderers gles gles3 glcore d3d11
    //        #pragma target 2.0

    //        #pragma vertex vert
    //        #pragma fragment frag
    //        #pragma shader_feature_local_fragment _ALPHATEST_ON
    //        #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON

    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInputSSSLUT.hlsl"
    //        #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
    //        ENDHLSL
    //    }
    }
    Fallback "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.SimpleLitShaderSSSLUT"
}
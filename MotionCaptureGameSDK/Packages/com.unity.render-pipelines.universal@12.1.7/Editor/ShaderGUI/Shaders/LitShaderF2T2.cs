using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class LitShaderF2T2 : BaseShaderGUI
    {
        static readonly string[] workflowModeNames = Enum.GetNames(typeof(LitGUI.WorkflowMode));
        private LitGUI.LitProperties litProperties;
        private LitDetailGUI.LitProperties litDetailProperties;
        protected MaterialProperty baseMapProp2 { get; set; }
        protected MaterialProperty BaseMap3Prop2 { get; set; }
        protected MaterialProperty BaseMap4Prop2 { get; set; }
        protected MaterialProperty MatcapMapProp { get; set; }

        protected MaterialProperty ditherHandle { get; set; }
        protected MaterialProperty ditherHandle2 { get; set; }
        protected MaterialProperty AnisoScale0{ get; set; }
        protected MaterialProperty AnisoScale1 { get; set; }

        private bool enableRvtSpc = false;

        Material targetMat = null;
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            materialEditor.Repaint();
            targetMat = materialEditor.target as Material;
            EditorGUI.BeginChangeCheck();
            base.OnGUI(materialEditor, props);
            enableRvtSpc = EditorGUILayout.Toggle(new GUIContent("enableRvtSpc", null, "enableRvtSpc"), enableRvtSpc);
            if (EditorGUI.EndChangeCheck())
            {
                UnityEngine.Rendering.CoreUtils.SetKeyword(targetMat, "_SPC_RVT", enableRvtSpc);
                EditorUtility.SetDirty(targetMat);
            }
        }

        public override void FillAdditionalFoldouts(MaterialHeaderScopeList materialScopesList)
        {
            materialScopesList.RegisterHeaderScope(LitDetailGUI.Styles.detailInputs, Expandable.Details, _ => LitDetailGUI.DoDetailArea(litDetailProperties, materialEditor));
        }

        // collect properties from the material properties
        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            litProperties = new LitGUI.LitProperties(properties);
            litDetailProperties = new LitDetailGUI.LitProperties(properties);
            baseMapProp2 = FindProperty("_MainTex2", properties, false);
            BaseMap3Prop2 = FindProperty("_BaseMap3", properties, false);
            BaseMap4Prop2 = FindProperty("_BaseMap4", properties, false);
            MatcapMapProp = FindProperty("_MatcapMap", properties, false);
            
            ditherHandle = FindProperty("_DitherHandle", properties);
            ditherHandle2 = FindProperty("_DitherHandle2", properties);
            AnisoScale0 = FindProperty("_AnisoScale0", properties);
            AnisoScale1 = FindProperty("_AnisoScale1", properties);
            if(targetMat != null)
                enableRvtSpc = targetMat.IsKeywordEnabled("_SPC_RVT");
        }

        // material changed check
        public override void ValidateMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            SetMaterialKeywords2(material, LitGUI.SetMaterialKeywords, LitDetailGUI.SetMaterialKeywords);
        }
        public void SetMaterialKeywords2(Material material, Action<Material> shadingModelFunc = null, Action<Material> shaderFunc = null)
        {
            // Clear all keywords for fresh start
            material.shaderKeywords = null;
            if (targetMat == null)
                targetMat = material;
            // Setup blending - consistent across all Universal RP shaders
            SetupMaterialBlendMode2(material);

            // Receive Shadows
            if (material.HasProperty("_ReceiveShadows"))
                CoreUtils.SetKeyword(material, "_RECEIVE_SHADOWS_OFF", material.GetFloat("_ReceiveShadows") == 0.0f);
            UnityEngine.Rendering.CoreUtils.SetKeyword(targetMat, "_SPC_RVT", enableRvtSpc);
            // Emission
            if (material.HasProperty("_EmissionColor"))
                MaterialEditor.FixupEmissiveFlag(material);
            bool shouldEmissionBeEnabled =
                (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
            if (material.HasProperty("_EmissionEnabled") && !shouldEmissionBeEnabled)
                shouldEmissionBeEnabled = material.GetFloat("_EmissionEnabled") >= 0.5f;
            CoreUtils.SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);

            // Normal Map
            if (material.HasProperty("_BumpMap"))
                CoreUtils.SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap"));

            // Shader specific keyword functions
            shadingModelFunc?.Invoke(material);
            shaderFunc?.Invoke(material);
        }
        // material main surface options
        public override void DrawSurfaceOptions(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            // Detect any changes to the material
            //EditorGUI.BeginChangeCheck();
            if (litProperties.workflowMode != null)
            {
                DoPopup(LitGUI.Styles.workflowModeText, litProperties.workflowMode, workflowModeNames);
            }
            //if (EditorGUI.EndChangeCheck())
            //{
                //foreach (var obj in blendModeProp.targets)
                    //MaterialChanged((Material)obj);
            //}
            base.DrawSurfaceOptions(material);
        }
        public static readonly GUIContent guicDM = new GUIContent("Dither Map",
                "Specifies the base Material and/or Color of the surface. If you¡¯ve selected Transparent or Alpha Clipping under Surface Options, your Material uses the Texture¡¯s alpha channel or color.");
        public static readonly GUIContent guicDHDM = new GUIContent("DitherHandle","");
        public static readonly GUIContent guicDHDM2 = new GUIContent("DitherHandle2", "");
        public static readonly GUIContent guicDM3 = new GUIContent("Color3Map", "");
        public static readonly GUIContent guicDM4 = new GUIContent("Temp4Map", "");
        public static readonly GUIContent guicMC = new GUIContent("MatcapMap", "");
        public static readonly GUIContent guic_AnisoScale0 = new GUIContent("AnisoScale0", "");
        public static readonly GUIContent guic_AnisoScale1 = new GUIContent("AnisoScale1", "");
        
        void DrawBaseProperties2(Material material)
        {
            if (baseMapProp2 != null )
            {

                //materialEditor.TexturePropertySingleLine(Styles.baseMap, baseMapProp, baseColorProp);
                materialEditor.TexturePropertySingleLine(guicDM, baseMapProp2);
                // TODO Temporary fix for lightmapping, to be replaced with attribute tag.
                if (material.HasProperty("_MainTex2"))
                {
                    DrawTileOffset(materialEditor, baseMapProp2);
                    material.SetTexture("_MainTex2", baseMapProp2.textureValue);
                    var baseMapTiling = baseMapProp2.textureScaleAndOffset;
                    material.SetTextureScale("_MainTex2", new Vector2(baseMapTiling.x, baseMapTiling.y));
                    material.SetTextureOffset("_MainTex2", new Vector2(baseMapTiling.z, baseMapTiling.w));
                    materialEditor.ShaderProperty(ditherHandle, guicDHDM, 1);
                    materialEditor.ShaderProperty(ditherHandle2, guicDHDM2, 1);
                    materialEditor.ShaderProperty(AnisoScale0, guic_AnisoScale0, 1);
                    materialEditor.ShaderProperty(AnisoScale1, guic_AnisoScale1, 1);
                }
                materialEditor.TexturePropertySingleLine(guicDM3, BaseMap3Prop2);
                if (material.HasProperty("_BaseMap3"))
                {
                    DrawTileOffset(materialEditor, BaseMap3Prop2);
                    material.SetTexture("_BaseMap3", BaseMap3Prop2.textureValue);
                    var baseMapTiling = BaseMap3Prop2.textureScaleAndOffset;
                    material.SetTextureScale("_BaseMap3", new Vector2(baseMapTiling.x, baseMapTiling.y));
                    material.SetTextureOffset("_BaseMap3", new Vector2(baseMapTiling.z, baseMapTiling.w));
                }
                materialEditor.TexturePropertySingleLine(guicDM4, BaseMap4Prop2);
                if (material.HasProperty("_BaseMap4"))
                {
                    material.SetTexture("_BaseMap4", BaseMap4Prop2.textureValue);
                }
                materialEditor.TexturePropertySingleLine(guicMC, MatcapMapProp);
                if (material.HasProperty("_MatcapMap"))
                {
                    material.SetTexture("_MatcapMap", MatcapMapProp.textureValue);
                }
                
            }
        }
        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            DrawBaseProperties2(material);
            base.DrawSurfaceInputs(material);
            LitGUI.Inputs(litProperties, materialEditor, material);
            DrawEmissionProperties(material, true);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        // material main advanced options
        public override void DrawAdvancedOptions(Material material)
        {
            if (litProperties.reflections != null && litProperties.highlights != null)
            {
                materialEditor.ShaderProperty(litProperties.highlights, LitGUI.Styles.highlightsText);
                materialEditor.ShaderProperty(litProperties.reflections, LitGUI.Styles.reflectionsText);
            }

            base.DrawAdvancedOptions(material);
        }
        public  void SetupMaterialBlendMode2(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            bool alphaClip = false;
            if (material.HasProperty("_AlphaClip"))
                alphaClip = material.GetFloat("_AlphaClip") >= 0.5;

            if (alphaClip)
            {
                material.EnableKeyword("_ALPHATEST_ON");
            }
            else
            {
                material.DisableKeyword("_ALPHATEST_ON");
            }

            if (material.HasProperty("_Surface"))
            {
                SurfaceType surfaceType = (SurfaceType)material.GetFloat("_Surface");
                if (surfaceType == SurfaceType.Opaque)
                {
                    if (alphaClip)
                    {
                        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                        material.SetOverrideTag("RenderType", "TransparentCutout");
                    }
                    else
                    {
                        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                        material.SetOverrideTag("RenderType", "Opaque");
                    }

                    material.renderQueue += material.HasProperty("_QueueOffset") ? (int)material.GetFloat("_QueueOffset") : 0;
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.SetShaderPassEnabled("ShadowCaster", true);
                }
                else
                {
                    BlendMode blendMode = (BlendMode)material.GetFloat("_Blend");

                    // Specific Transparent Mode Settings
                    switch (blendMode)
                    {
                        case BlendMode.Alpha:
                            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            break;
                        case BlendMode.Premultiply:
                            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                            break;
                        case BlendMode.Additive:
                            //material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                            //material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);

                            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
                            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            break;
                        case BlendMode.Multiply:
                            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.DstColor);
                            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            material.EnableKeyword("_ALPHAMODULATE_ON");
                            break;
                    }

                    // General Transparent Material Settings
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_ZWrite", 0);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.renderQueue += material.HasProperty("_QueueOffset") ? (int)material.GetFloat("_QueueOffset") : 0;
                    material.SetShaderPassEnabled("ShadowCaster", false);
                }
            }
        }
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialBlendMode2(material);
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat("_AlphaClip", 1);
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            //material.SetFloat("_Surface", (float)surfaceType);
            material.SetFloat("_Blend", (float)blendMode);

            material.SetFloat("_Surface", (float)surfaceType);
            if (surfaceType == SurfaceType.Opaque)
            {
                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            else
            {
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }

            if (oldShader.name.Equals("Standard (Specular setup)"))
            {
                material.SetFloat("_WorkflowMode", (float)LitGUI.WorkflowMode.Specular);
                Texture texture = material.GetTexture("_SpecGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
            else
            {
                material.SetFloat("_WorkflowMode", (float)LitGUI.WorkflowMode.Metallic);
                Texture texture = material.GetTexture("_MetallicGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }

            //MaterialChanged(material);
        }
    }
}

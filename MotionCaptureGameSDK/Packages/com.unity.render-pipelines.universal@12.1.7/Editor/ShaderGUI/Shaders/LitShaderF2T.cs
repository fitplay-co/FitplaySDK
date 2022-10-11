using System;
using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class LitShaderF2T : BaseShaderGUI
    {
        static readonly string[] workflowModeNames = Enum.GetNames(typeof(LitGUI.WorkflowMode));
        private LitGUI.LitProperties litProperties;
        private LitDetailGUI.LitProperties litDetailProperties;
        //private SavedBool m_DetailInputsFoldout;
        protected MaterialProperty baseMapProp2 { get; set; }
        protected MaterialProperty ditherHandle { get; set; }
        protected MaterialProperty ditherHandle2 { get; set; }
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
            ditherHandle = FindProperty("_DitherHandle", properties);
            ditherHandle2 = FindProperty("_DitherHandle2", properties);
        }

        // material changed check
        public override void ValidateMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords, LitDetailGUI.SetMaterialKeywords);
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
                SetupMaterialBlendMode(material);
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

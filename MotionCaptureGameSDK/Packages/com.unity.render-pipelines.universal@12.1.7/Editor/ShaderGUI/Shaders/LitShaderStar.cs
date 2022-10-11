using System;
using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class LitShaderStar : BaseShaderGUI
    {
        static readonly string[] workflowModeNames = Enum.GetNames(typeof(LitGUI.WorkflowMode));
        private LitGUI.LitProperties litProperties;
        private LitDetailGUI.LitProperties litDetailProperties;

        //private MaterialProperty _StarTex;
        protected MaterialProperty baseMapProp2 { get; set; }
        protected MaterialProperty ReSampleScale { get; set; }
        protected MaterialProperty ViewDirScale { get; set; }
        //protected MaterialProperty VdsMax { get; set; }
        protected MaterialProperty VdsAdd { get; set; }
        protected MaterialProperty NorMul { get; set; }
        protected MaterialProperty Re0 { get; set; }
        protected MaterialProperty Re1 { get; set; }
        protected MaterialProperty StarColorProp { get; set; }
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
            //_StarTex = BaseShaderGUI.FindProperty("_StarTex", properties, false);
            baseMapProp2 = FindProperty("_StarTex", properties, false);
            StarColorProp = FindProperty("_StarColor", properties, false);
            ReSampleScale = FindProperty("_ReSampleScale", properties);
            ViewDirScale = FindProperty("_ViewDirScale", properties);
            //VdsMax = FindProperty("_VdsMax", properties);
            VdsAdd = FindProperty("_VdsAdd", properties);
            NorMul = FindProperty("_NorMul", properties);
            Re0 = FindProperty("_Re0", properties);
            Re1 = FindProperty("_Re1", properties);
            
        }
  
        // material changed check
        public override void ValidateMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords, LitDetailGUI.SetMaterialKeywords);
            UnityEngine.Rendering.CoreUtils.SetKeyword(material, "_STARADD_OFF", material.GetFloat("_VdsAdd") == 0.0f);
            UnityEngine.Rendering.CoreUtils.SetKeyword(material, "_NORMAL_COLOR_OFF", material.GetFloat("_NorMul") == 0.0f);
        }
        public static readonly GUIContent tempText4 =
                new GUIContent("StarTex", "");
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
            //materialEditor.TexturePropertySingleLine(tempText4, _StarTex);
        }
        public static readonly GUIContent guicDM = new GUIContent("闪烁帧动画纹理 Star Map",
                "Specifies the base Material and/or Color of the surface. If you’ve selected Transparent or Alpha Clipping under Surface Options, your Material uses the Texture’s alpha channel or color.");
        public static readonly GUIContent guicDHDM = new GUIContent("频率 ReSampleScale", "");
        public static readonly GUIContent guicDHDMV = new GUIContent("距离增强 ViewDirAddScale ", "");
        //public static readonly GUIContent guicDHDMVM = new GUIContent("VdsMax", "");
        public static readonly GUIContent guicDHDM0 = new GUIContent("强度下限 Re0", "");
        public static readonly GUIContent guicDHDM1 = new GUIContent("强度上限 Re1", "");
        public static readonly GUIContent vdsaddText = new GUIContent("远距离增强开关 VdsAdd", "");
        public static readonly GUIContent normulText = new GUIContent("法线叠色开关 NorMul", "");
        void DrawBaseProperties2(Material material)
        {
            if (baseMapProp2 != null)
            {

                //materialEditor.TexturePropertySingleLine(Styles.baseMap, baseMapProp, baseColorProp);
                materialEditor.TexturePropertySingleLine(guicDM, baseMapProp2, StarColorProp);
                // TODO Temporary fix for lightmapping, to be replaced with attribute tag.
                if (material.HasProperty("_StarTex"))
                {
                    DrawTileOffset(materialEditor, baseMapProp2);
                    material.SetTexture("_StarTex", baseMapProp2.textureValue);
                    var baseMapTiling = baseMapProp2.textureScaleAndOffset;
                    material.SetTextureScale("_StarTex", new Vector2(baseMapTiling.x, baseMapTiling.y));
                    material.SetTextureOffset("_StarTex", new Vector2(baseMapTiling.z, baseMapTiling.w));
                    materialEditor.ShaderProperty(ReSampleScale, guicDHDM, 1);
                    //materialEditor.ShaderProperty(VdsMax, guicDHDMVM, 1);
                    materialEditor.ShaderProperty(Re0, guicDHDM0, 1);
                    materialEditor.ShaderProperty(Re1, guicDHDM1, 1);
                    
                }
                

            }
            if (VdsAdd != null)
            {
                EditorGUI.BeginChangeCheck();
                var receiveShadows =
                    EditorGUILayout.Toggle(vdsaddText, VdsAdd.floatValue == 1.0f);
                if (EditorGUI.EndChangeCheck())
                    VdsAdd.floatValue = receiveShadows ? 1.0f : 0.0f;
                if(receiveShadows)
                    materialEditor.ShaderProperty(ViewDirScale, guicDHDMV, 1);
            }
            EditorGUI.BeginChangeCheck();
            var receiveShadows2 =
                EditorGUILayout.Toggle(normulText, NorMul.floatValue == 1.0f);
            if (EditorGUI.EndChangeCheck())
                NorMul.floatValue = receiveShadows2 ? 1.0f : 0.0f;
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

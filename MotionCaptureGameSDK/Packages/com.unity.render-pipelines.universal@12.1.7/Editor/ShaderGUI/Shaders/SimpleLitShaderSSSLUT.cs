using System;
using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class SimpleLitShaderSSSLUT : BaseShaderGUI
    {
        Material targetMat = null;
        // Properties
        private SimpleLitGUI.SimpleLitProperties shadingModelProperties;
        private MaterialProperty SSSLUT;
        private MaterialProperty SpecularLUT;
        private MaterialProperty _OccGlossMap;
        private MaterialProperty CurveTex;
        private bool disableSimplify = false;
        private bool enableDif0 = false;
        private bool enableRvtDA = false;
        //private float Brightness = 1.0f;
        //private float Roughness = 1.0f;
        //private float CurveRate = 1.0f;

        private MaterialProperty Brightness;
        private MaterialProperty Roughness;
        private MaterialProperty CurveRate;
        private MaterialProperty LutRemap0;
        private MaterialProperty LutRemap1;
        private MaterialProperty Occ;
        private MaterialProperty SpcRemap0;
        private MaterialProperty SpcRemap1;
        private MaterialProperty Alpha0;
        private MaterialProperty Alpha1;
        private MaterialProperty Shadow0;
        private MaterialProperty Shadow1;
        
        // collect properties from the material properties
        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            shadingModelProperties = new SimpleLitGUI.SimpleLitProperties(properties);
            SSSLUT = BaseShaderGUI.FindProperty("_SSSLUT", properties, false);
            SpecularLUT = BaseShaderGUI.FindProperty("_SpecularLUT", properties, false);
            _OccGlossMap = BaseShaderGUI.FindProperty("_OccGlossMap", properties, false);
            
            CurveTex = BaseShaderGUI.FindProperty("_CurveTex", properties, false);
            
            //foreach (var obj in blendModeProp.targets)
            //{
                Brightness = FindProperty("_Brightness", properties);
                Roughness = FindProperty("_Roughness", properties);
                CurveRate = FindProperty("_CurveFactor", properties);
            LutRemap0 = FindProperty("_LutRemap0", properties);
            LutRemap1 = FindProperty("_LutRemap1", properties);
            Occ = FindProperty("_Occ", properties);
            
            SpcRemap0 = FindProperty("_SpcRemap0", properties);
            SpcRemap1 = FindProperty("_SpcRemap1", properties);
            
            Alpha0 = FindProperty("_Alpha0", properties);
            Alpha1 = FindProperty("_Alpha1", properties);

            Shadow0 = FindProperty("_Shadow0", properties);
            Shadow1 = FindProperty("_Shadow1", properties);
            

            disableSimplify = !targetMat.IsKeywordEnabled("_SIMPLIFY_OFF");
            enableDif0 = targetMat.IsKeywordEnabled("_DIFFUSE_OFF");
            enableRvtDA = targetMat.IsKeywordEnabled("_DIFFUSEA_RVT");
            //}

            //if (enableSimplify)
            //    Debug.Log("bTrue");
        }

        // material changed check
        public override void MaterialChanged(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            SetMaterialKeywords(material, SimpleLitGUI.SetMaterialKeywords);
        }
        
        // material main surface options
        public override void DrawSurfaceOptions(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            // Detect any changes to the material
            EditorGUI.BeginChangeCheck();
            {
                base.DrawSurfaceOptions(material);
            }
            if (EditorGUI.EndChangeCheck())
            {
                //foreach (var obj in blendModeProp.targets)
                    MaterialChanged(material);
            }
        }
        public static readonly GUIContent tempText1 =
                new GUIContent("SSSLUT", "");
        public static readonly GUIContent tempText2 =
                new GUIContent("SpecularLUT", "");
        public static readonly GUIContent tempText4 =
                new GUIContent("OccGlossMap", "");
        public static readonly GUIContent tempText3 =
                new GUIContent("CurveTex", "");
        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            SimpleLitGUI.Inputs(shadingModelProperties, materialEditor, material);


            //SpecularSource specSource = (SpecularSource)shadingModelProperties.specHighlights.floatValue;
            //EditorGUI.BeginDisabledGroup(specSource == SpecularSource.NoSpecular);
            //BaseShaderGUI.TextureColorProps(materialEditor, Styles.specularMapText, shadingModelProperties.specGlossMap, properties.specColor, true);
            //DoSmoothness(shadingModelProperties, material);
            //EditorGUI.EndDisabledGroup();
            //BaseShaderGUI.DrawNormalArea(materialEditor, shadingModelProperties.bumpMapProp);
            //DrawEmissionProperties(material, true);

            //materialEditor.TexturePropertySingleLine(tempText1, SSSLUT);
            //materialEditor.TexturePropertySingleLine(tempText2, SpecularLUT);

            //这就没A，还是用着用HDR吧
            //SpecularSource specSource = (SpecularSource)shadingModelProperties.specHighlights.floatValue;
            //EditorGUI.BeginDisabledGroup(specSource == SpecularSource.NoSpecular);
            //BaseShaderGUI.TextureColorProps(materialEditor, SimpleLitGUI.Styles.specularMapText, shadingModelProperties.specGlossMap, shadingModelProperties.specColor, false);
            //SimpleLitGUI.DoSmoothness(shadingModelProperties, material);
            //EditorGUI.EndDisabledGroup();
            //BaseShaderGUI.DrawNormalArea(materialEditor, shadingModelProperties.bumpMapProp);

            materialEditor.TexturePropertySingleLine(tempText1, SSSLUT);
            materialEditor.TexturePropertySingleLine(tempText2, SpecularLUT);
            materialEditor.TexturePropertySingleLine(tempText4, _OccGlossMap);
            materialEditor.TexturePropertySingleLine(tempText3, CurveTex);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        public override void DrawAdvancedOptions(Material material)
        {
            SimpleLitGUI.Advanced(shadingModelProperties);
            base.DrawAdvancedOptions(material);
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            //if (material.HasProperty("_Emission"))
            //{
            //    material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            //}

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
            material.SetFloat("_Surface", (float)surfaceType);
            material.SetFloat("_Blend", (float)blendMode);

            MaterialChanged(material);
        }
        private static float m_DrawFloatTicker(float value, string label = null, string tooltip = null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent(label, null, tooltip));
            if (GUILayout.Button("«", EditorStyles.miniButtonLeft, GUILayout.MaxWidth(20)))
            {
                value -= 0.1f;
            }
            if (GUILayout.Button("‹", EditorStyles.miniButtonMid, GUILayout.MaxWidth(17)))
            {
                value -= 0.01f;
            }

            value = EditorGUILayout.FloatField(value, GUILayout.MaxWidth(45));

            if (GUILayout.Button("›", EditorStyles.miniButtonMid, GUILayout.MaxWidth(17)))
            {
                value += 0.01f;
            }
            if (GUILayout.Button("»", EditorStyles.miniButtonRight, GUILayout.MaxWidth(20)))
            {
                value += 0.1f;
            }
            EditorGUILayout.EndHorizontal();

            return value;
        }
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            materialEditor.Repaint();
            targetMat = materialEditor.target as Material;
            bool bChecked = m_FirstTimeApply;
            EditorGUI.BeginChangeCheck();
            base.OnGUI(materialEditor, props);
            Occ.floatValue = m_DrawFloatTicker(Occ.floatValue, "Occlusion");
            Occ.floatValue = EditorGUILayout.Slider(Occ.floatValue, 0, 16);

            Shadow0.floatValue = m_DrawFloatTicker(Shadow0.floatValue, "Shadow0");
            Shadow0.floatValue = EditorGUILayout.Slider(Shadow0.floatValue, -2, 2);
            Shadow1.floatValue = m_DrawFloatTicker(Shadow1.floatValue, "Shadow1");
            Shadow1.floatValue = EditorGUILayout.Slider(Shadow1.floatValue, -2, 2);

            disableSimplify = EditorGUILayout.Toggle(new GUIContent("disableSimplify", null, "DisableSimplify Simplify"), disableSimplify);
            enableDif0 = EditorGUILayout.Toggle(new GUIContent("enableDif0", null, "enableDif0"), enableDif0);
            enableRvtDA = EditorGUILayout.Toggle(new GUIContent("enableRvtDA", null, "enableRvtDA"), enableRvtDA);
            
            //Brightness.floatValue = m_DrawFloatTicker(Brightness.floatValue, "Brightness");
            Roughness.floatValue = m_DrawFloatTicker(Roughness.floatValue, "Roughness");
            //Roughness.floatValue = EditorGUILayout.Slider(Roughness.floatValue, 0, 0.999f);//builtin
            Roughness.floatValue = EditorGUILayout.Slider(Roughness.floatValue, 0f, 1f);//builtin
            //CurveRate.floatValue = m_DrawFloatTicker(CurveRate.floatValue, "CurveRate");
            EditorGUILayout.Space();
            LutRemap0.floatValue = m_DrawFloatTicker(LutRemap0.floatValue, "LutRemap0");
            LutRemap0.floatValue = EditorGUILayout.Slider(LutRemap0.floatValue, 0, 16);
            LutRemap1.floatValue = m_DrawFloatTicker(LutRemap1.floatValue, "LutRemap1");
            LutRemap1.floatValue = EditorGUILayout.Slider(LutRemap1.floatValue, 0, 16);
            EditorGUILayout.Space();

            SpcRemap0.floatValue = m_DrawFloatTicker(SpcRemap0.floatValue, "SpcRemap0");
            SpcRemap0.floatValue = EditorGUILayout.Slider(SpcRemap0.floatValue, -1, 8);
            SpcRemap1.floatValue = m_DrawFloatTicker(SpcRemap1.floatValue, "SpcRemap1");
            SpcRemap1.floatValue = EditorGUILayout.Slider(SpcRemap1.floatValue, -1, 8);
            EditorGUILayout.Space();
            //Alpha0.floatValue = m_DrawFloatTicker(Alpha0.floatValue, "AlphaLut0");
            //Alpha1.floatValue = m_DrawFloatTicker(Alpha1.floatValue, "AlphaLut1");
            Alpha0.floatValue = m_DrawFloatTicker(Alpha0.floatValue, "AlphaLut0");
            Alpha0.floatValue = EditorGUILayout.Slider(Alpha0.floatValue, 0, 1);
            Alpha1.floatValue = m_DrawFloatTicker(Alpha1.floatValue, "AlphaLut1");
            Alpha1.floatValue = EditorGUILayout.Slider(Alpha1.floatValue, 0, 1);

            
            
            //if (enableSimplify)
            //    Debug.Log("bTrue");
            if (bChecked || EditorGUI.EndChangeCheck())
            {
                //foreach (var obj in blendModeProp.targets)
                //{
                    UnityEngine.Rendering.CoreUtils.SetKeyword(targetMat, "_SIMPLIFY_OFF", !disableSimplify);
                    UnityEngine.Rendering.CoreUtils.SetKeyword(targetMat, "_DIFFUSE_OFF", enableDif0);
                    UnityEngine.Rendering.CoreUtils.SetKeyword(targetMat, "_DIFFUSEA_RVT", enableRvtDA);
                    EditorUtility.SetDirty(targetMat);
                //}
                    
            }
        }
    }
}

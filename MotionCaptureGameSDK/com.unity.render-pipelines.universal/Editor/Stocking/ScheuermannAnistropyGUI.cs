using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.LWRP.ShaderGUI
{
    public static class ScheuermannAnistropyGUI
    {
        public enum SpecularSource
        {
            SpecularTextureAndColor,
            NoSpecular
        }
        
        public enum SmoothnessMapChannel
        {
            SpecularAlpha,
            AlbedoAlpha,
        }

        public static class Styles
        {
            public static GUIContent specularMapText =
                new GUIContent("Specular Map", "Sets and configures a Specular map and color for your Material.");

            public static GUIContent smoothnessText = new GUIContent("Smoothness Value",
                "Controls the spread of highlights and reflections on the surface.");
            public static GUIContent anistropyScale = new GUIContent("Anistropy anistropyScale",
                "");
            public static GUIContent anistropyShiftText = new GUIContent("Anistropy Shift",
                "Offsets the highlights and reflections position on the surface.");
            public static GUIContent LambertRemap0Text = new GUIContent("LambertRemap0",
                "");
            public static GUIContent LambertRemap1Text = new GUIContent("LambertRemap1",
                "");
            public static GUIContent HueText = new GUIContent("Hue",
                "");
            public static GUIContent SaturationText = new GUIContent("Saturation",
                "");
            public static GUIContent ValueText = new GUIContent("Value",
                "");
            public static GUIContent highlightsText = new GUIContent("Specular Highlights",
                "When enabled, the Material reflects the shine from direct lighting.");

            
            public static GUIContent denierText = new GUIContent("denier", "");
            public static GUIContent rimPowerText = new GUIContent("rimPower","");
            public static GUIContent fresnelScaleText = new GUIContent("fresnelScale", "");
            public static GUIContent fresnel0Text = new GUIContent("fresnel0", "");
            public static GUIContent fresnel1Text = new GUIContent("fresnel1", "");
            public static GUIContent FactorMLText = new GUIContent("FactorML", "");
            public static GUIContent FactorGIText = new GUIContent("FactorGI", "");
            public static GUIContent stockingTex = new GUIContent("stockingTex", "");
        }

        public struct SimpleLitProperties
        {
            // Surface Input Props
            public MaterialProperty specColor;
            public MaterialProperty specGlossMap;
            public MaterialProperty specHighlights;
            public MaterialProperty smoothness;
            public MaterialProperty anistropyScale;
            public MaterialProperty anistropyShift;
            public MaterialProperty LambertRemap0;
            public MaterialProperty LambertRemap1;
            //public MaterialProperty Hue;
            //public MaterialProperty Saturation;
            //public MaterialProperty Value;
            public MaterialProperty bumpMapProp;

            
            public MaterialProperty denier;
            public MaterialProperty rimPower;
            public MaterialProperty fresnelScale;
            public MaterialProperty fresnel0;
            public MaterialProperty fresnel1;
            public MaterialProperty FactorML;
            public MaterialProperty FactorGI;
            //public MaterialProperty skinTintColor;
            //public MaterialProperty skinTex;
            public MaterialProperty stockingTintColor;
            public MaterialProperty stockingTex;
            public SimpleLitProperties(MaterialProperty[] properties)
            {
                // Surface Input Props
                specColor = BaseShaderGUI.FindProperty("_SpecColor", properties);
                specGlossMap = BaseShaderGUI.FindProperty("_SpecGlossMap", properties, false);
                specHighlights = BaseShaderGUI.FindProperty("_SpecularHighlights", properties, false);
                smoothness = BaseShaderGUI.FindProperty("_Smoothness", properties, false);
                
                anistropyScale = BaseShaderGUI.FindProperty("_AnistropyScale", properties, false);
                anistropyShift = BaseShaderGUI.FindProperty("_AnistropyShift", properties, false);
                LambertRemap0 = BaseShaderGUI.FindProperty("_LambertRemap0", properties, false);
                LambertRemap1 = BaseShaderGUI.FindProperty("_LambertRemap1", properties, false);

                //Hue = BaseShaderGUI.FindProperty("_Hue", properties, false);
                //Saturation = BaseShaderGUI.FindProperty("_Saturation", properties, false);
                //Value = BaseShaderGUI.FindProperty("_Value", properties, false);

                bumpMapProp = BaseShaderGUI.FindProperty("_BumpMap", properties, false);

                denier = BaseShaderGUI.FindProperty("_Denier", properties, false);
                rimPower = BaseShaderGUI.FindProperty("_RimPower", properties, false);
                fresnelScale = BaseShaderGUI.FindProperty("_FresnelScale", properties, false);
                fresnel0 = BaseShaderGUI.FindProperty("_Fresnel0", properties, false);
                fresnel1 = BaseShaderGUI.FindProperty("_Fresnel1", properties, false);
                FactorML = BaseShaderGUI.FindProperty("_FactorML", properties, false);
                FactorGI = BaseShaderGUI.FindProperty("_FactorGI", properties, false);
                //skinTintColor = BaseShaderGUI.FindProperty("_SpecColor", properties, false);
                //skinTex = BaseShaderGUI.FindProperty("_SpecGlossMap", properties, false);
                stockingTintColor = BaseShaderGUI.FindProperty("_StockingTint", properties, false);
                stockingTex = BaseShaderGUI.FindProperty("_StockingTex", properties, false);
            }
        }

        public static void Inputs(SimpleLitProperties properties, MaterialEditor materialEditor, Material material)
        {
            DoSpecularArea(properties, materialEditor, material);
            BaseShaderGUI.DrawNormalArea(materialEditor, properties.bumpMapProp);
        }
        
        public static void Advanced(SimpleLitProperties properties)
        {
            SpecularSource specularSource = (SpecularSource)properties.specHighlights.floatValue;
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.specHighlights.hasMixedValue;
            bool enabled = EditorGUILayout.Toggle(Styles.highlightsText, specularSource == SpecularSource.SpecularTextureAndColor);
            if (EditorGUI.EndChangeCheck())
                properties.specHighlights.floatValue = enabled ? (float)SpecularSource.SpecularTextureAndColor : (float)SpecularSource.NoSpecular;
            EditorGUI.showMixedValue = false;
        }

        public static void DoSpecularArea(SimpleLitProperties properties, MaterialEditor materialEditor, Material material)
        {
            SpecularSource specSource = (SpecularSource)properties.specHighlights.floatValue;
            EditorGUI.BeginDisabledGroup(specSource == SpecularSource.NoSpecular);
            BaseShaderGUI.TextureColorProps(materialEditor, Styles.specularMapText, properties.specGlossMap,properties.specColor, true);
            DoSmoothness(properties, material);
            EditorGUI.EndDisabledGroup();
        }

        public static void DoSmoothness(SimpleLitProperties properties, Material material)
        {
            var opaque = ((BaseShaderGUI.SurfaceType) material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
            EditorGUI.indentLevel += 2;
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = properties.smoothness.hasMixedValue;

            var smoothness = properties.smoothness.floatValue;
            smoothness = EditorGUILayout.Slider(Styles.smoothnessText, smoothness, 0f, 1f);
            
            var anistropyScale = properties.anistropyScale.floatValue;
            anistropyScale = EditorGUILayout.Slider(Styles.anistropyScale, anistropyScale, 0f, 16f);
            var anistropyShift = properties.anistropyShift.floatValue;
            anistropyShift = EditorGUILayout.Slider(Styles.anistropyShiftText, anistropyShift, -4f, 4f);

            var LambertRemap0 = properties.LambertRemap0.floatValue;
            LambertRemap0 = EditorGUILayout.Slider(Styles.LambertRemap0Text, LambertRemap0, -1f, 2f);
            var LambertRemap1 = properties.LambertRemap1.floatValue;
            LambertRemap1 = EditorGUILayout.Slider(Styles.LambertRemap1Text, LambertRemap1, -1f, 2f);
            string sClr = "_SpecColor";
            var spcColor = material.GetColor(sClr);
            //var hsvColor = HSVUtil.ConvertRgbToHsv(spcColor);

            //var vH = (int)EditorGUILayout.Slider(Styles.HueText, (int)(hsvColor.normalizedH * 255f), 0f, 255f);
            //var vS = (int)EditorGUILayout.Slider(Styles.SaturationText, (int)(hsvColor.S * 255f), 0f, 255f);
            //var vV = (int)EditorGUILayout.Slider(Styles.ValueText, (int)(hsvColor.V * 255f), 0f, 255f);
            float vH = 0;
            float vS = 0;
            float vV = 0;
            Color.RGBToHSV(spcColor, out vH, out vS,out vV);

            float vH2 = (EditorGUILayout.Slider(Styles.HueText, (int)(vH * 255f), 0f, 255f)/255f);
            float vS2 = (EditorGUILayout.Slider(Styles.SaturationText, (int)(vS * 255f), 0f, 255f) / 255f);
            float vV2 = (EditorGUILayout.Slider(Styles.ValueText, (int)(vV * 255f), 0f, 255f) / 255f);

            if (EditorGUI.EndChangeCheck())
            {
                properties.smoothness.floatValue = smoothness;
                properties.anistropyScale.floatValue = anistropyScale;
                properties.anistropyShift.floatValue = anistropyShift;
                properties.LambertRemap0.floatValue = LambertRemap0;
                properties.LambertRemap1.floatValue = LambertRemap1;
                //var rgbColor = HSVUtil.ConvertHsvToRgb((float)vH *360f/255f, (float)vS / 255f, (float)vV / 255f, spcColor.a);
                Color cTmp = Color.HSVToRGB(vH2, vS2, vV2,true);
                cTmp.a = spcColor.a;
                material.SetColor(sClr, cTmp);
            }
            EditorGUI.showMixedValue = false;

            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(!opaque);

            EditorGUI.showMixedValue = false;
            EditorGUI.indentLevel -= 3;
            EditorGUI.EndDisabledGroup();
        }

        public static void SetMaterialKeywords(Material material)
        {
            UpdateMaterialSpecularSource(material);
        }
        
        private static void UpdateMaterialSpecularSource(Material material)
        {
            var opaque = ((BaseShaderGUI.SurfaceType) material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
            SpecularSource specSource = (SpecularSource)material.GetFloat("_SpecularHighlights");
            if (specSource == SpecularSource.NoSpecular)
            {
                CoreUtils.SetKeyword(material, "_SPECGLOSSMAP", false);
                CoreUtils.SetKeyword(material, "_SPECULAR_COLOR", false);
                CoreUtils.SetKeyword(material, "_GLOSSINESS_FROM_BASE_ALPHA", false);
            }
            else
            {
                
                bool hasMap = material.GetTexture("_SpecGlossMap");
                CoreUtils.SetKeyword(material, "_SPECGLOSSMAP", hasMap);
                CoreUtils.SetKeyword(material, "_SPECULAR_COLOR", !hasMap);


                string color = "_BaseColor";

                var col = material.GetColor(color);
                material.SetColor(color, col);
            }
        }
    }
}

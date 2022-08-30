using System;
using UnityEngine;

namespace UnityEditor.Rendering.LWRP.ShaderGUI
{
    internal class As220629Shader : BaseShaderGUI
    {
        // Properties
        private ScheuermannAnistropyGUI.SimpleLitProperties shadingModelProperties;
        
        // collect properties from the material properties
        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            shadingModelProperties = new ScheuermannAnistropyGUI.SimpleLitProperties(properties);
        }

        // material changed check
        public override void MaterialChanged(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            SetMaterialKeywords(material, ScheuermannAnistropyGUI.SetMaterialKeywords);
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
                foreach (var obj in blendModeProp.targets)
                    MaterialChanged((Material)obj);
            }
        }
        void DrawSelfInputs(Material material)
        {
            EditorGUI.BeginChangeCheck();

            
            var denier = shadingModelProperties.denier.floatValue;
            denier = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.denierText, denier, 5f, 120f);

            var rimPower = shadingModelProperties.rimPower.floatValue;
            rimPower = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.rimPowerText, rimPower, 0f, 64f);

            var fresnelScale = shadingModelProperties.fresnelScale.floatValue;
            fresnelScale = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.fresnelScaleText, fresnelScale, 0f, 10f);

            var fresnel0 = shadingModelProperties.fresnel0.floatValue;
            fresnel0 = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.fresnel0Text, fresnel0, 0f, 1f);

            var fresnel1 = shadingModelProperties.fresnel1.floatValue;
            fresnel1 = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.fresnel1Text, fresnel1, 0f, 1f);


            var FactorML = shadingModelProperties.FactorML.floatValue;
            FactorML = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.FactorMLText, FactorML, 0f, 1f);

            var FactorGI = shadingModelProperties.FactorGI.floatValue;
            FactorGI = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.FactorGIText, FactorGI, 0f, 1f);

            BaseShaderGUI.TextureColorProps(materialEditor, ScheuermannAnistropyGUI.Styles.stockingTex, shadingModelProperties.stockingTex, shadingModelProperties.stockingTintColor, true);

        //                public MaterialProperty skinTintColor;
        //public MaterialProperty skinTex;
        //public MaterialProperty stockingTintColor;
        //public MaterialProperty stockingTex;


            if (EditorGUI.EndChangeCheck())
            {
                shadingModelProperties.denier.floatValue = denier;
                shadingModelProperties.rimPower.floatValue = rimPower;
                shadingModelProperties.fresnelScale.floatValue = fresnelScale;
                shadingModelProperties.fresnel0.floatValue = fresnel0;
                shadingModelProperties.fresnel1.floatValue = fresnel1;
                shadingModelProperties.FactorML.floatValue = FactorML;
                shadingModelProperties.FactorGI.floatValue = FactorGI;
            }

            //var opaque = ((BaseShaderGUI.SurfaceType)material.GetFloat("_Surface") ==
            //              BaseShaderGUI.SurfaceType.Opaque);
            //EditorGUI.indentLevel += 2;

            //EditorGUI.BeginChangeCheck();
            //EditorGUI.showMixedValue = shadingModelProperties.smoothness.hasMixedValue;

            //var smoothness = shadingModelProperties.smoothness.floatValue;
            //smoothness = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.smoothnessText, smoothness, 0f, 1f);
            //var anistropyShift = shadingModelProperties.anistropyShift.floatValue;
            //anistropyShift = EditorGUILayout.Slider(ScheuermannAnistropyGUI.Styles.anistropyShiftText, anistropyShift, -1f, 1f);

            //if (EditorGUI.EndChangeCheck())
            //{
            //    shadingModelProperties.smoothness.floatValue = smoothness;
            //    shadingModelProperties.anistropyShift.floatValue = anistropyShift;
            //}
            //EditorGUI.showMixedValue = false;

            //EditorGUI.indentLevel++;
            //EditorGUI.BeginDisabledGroup(!opaque);

            //EditorGUI.showMixedValue = false;
            //EditorGUI.indentLevel -= 3;
            //EditorGUI.EndDisabledGroup();
        }
        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            DrawSelfInputs(material);
            ScheuermannAnistropyGUI.Inputs(shadingModelProperties, materialEditor, material);
            DrawEmissionProperties(material, true);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        public override void DrawAdvancedOptions(Material material)
        {
            ScheuermannAnistropyGUI.Advanced(shadingModelProperties);
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
            material.SetFloat("_Surface", (float)surfaceType);
            material.SetFloat("_Blend", (float)blendMode);

            MaterialChanged(material);
        }
    }
}

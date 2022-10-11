using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class LitShaderNormalIn1 : BaseShaderGUI
    {
static readonly string[] workflowModeNames = Enum.GetNames(typeof(LitGUI.WorkflowMode));
        Material targetMat = null;
        private LitGUI.LitProperties litProperties;
        private LitDetailGUI.LitProperties litDetailProperties;
        private bool enableIn1 = false;
        private bool bConvert2MMOnce = false;


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
            enableIn1 = targetMat.IsKeywordEnabled("_NORMAL_IN1");
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

        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
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
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            EditorGUILayout.TextField("标准lit的轻微优化版本，目前适用于需要粗糙度但是没有金属度和缕空的情况。");
            materialEditor.Repaint();
            targetMat = materialEditor.target as Material;
            EditorGUI.BeginChangeCheck();
            base.OnGUI(materialEditor, props);
            EditorGUILayout.TextField("开启采样BaseMap的透明度通道作为粗糙度");
            enableIn1 = EditorGUILayout.Toggle(new GUIContent("EnableIn1", null, "EnableIn1"), enableIn1);
            EditorGUILayout.TextField("点击把当前MetallicMap的透明度通道复制到BaseMap去,大概需要几秒钟");
            bConvert2MMOnce = EditorGUILayout.Toggle(new GUIContent("Convert2MMOnce ", null, "Convert2MMOnce"), bConvert2MMOnce);
            if (bConvert2MMOnce)
            {
                bConvert2MMOnce = false;
                //do
                var tNormal = targetMat.GetTexture("_MetallicGlossMap");
                var tGloss = targetMat.GetTexture("_BaseMap");
                if (tGloss == null || tNormal == null)
                {
                    Debug.LogError("NoTextureNeeded");
                    return;
                }
                    
                var t2dNormal = tNormal as Texture2D;
                string sTmp3Normal = UnityEditor.AssetDatabase.GetAssetPath(t2dNormal);
                TextureImporter Importer0 = AssetImporter.GetAtPath(sTmp3Normal) as TextureImporter;
                Importer0.isReadable = true;
                //Importer0.textureType = TextureImporterType.Default;
                //设可读，后续需要设置回来
                UnityEditor.AssetDatabase.ImportAsset(sTmp3Normal);


                
                var t2dGloss = tGloss as Texture2D;

                string sTmp3Gloss = UnityEditor.AssetDatabase.GetAssetPath(t2dGloss);
                TextureImporter Importer = AssetImporter.GetAtPath(sTmp3Gloss) as TextureImporter;
                var ticTmp = Importer.textureCompression;
                var qltTmp = Importer.compressionQuality;
                Importer.textureCompression = TextureImporterCompression.Uncompressed;
                Importer.isReadable = true;
                //设可读，后续需要设置回来
                UnityEditor.AssetDatabase.ImportAsset(sTmp3Gloss);


                AssetDatabase.Refresh();


                List<Color> colorList = new List<Color>(t2dGloss.width * t2dGloss.height);
                //int iTemp1 = 0;
                //int jTemp1 = 0;
                int nWidth = t2dGloss.width;//不考虑非2次幂
                int nWidthM2 = nWidth/2;
                for (int i = 0; i < nWidth; i++)
                {
                    for (int j = 0; j < nWidth; j++)
                    {
                        var colorTmp = t2dGloss.GetPixel(i, j);
                        //iTemp1 = j;
                        //jTemp1 = i;
                        //var colorTmp2 = t2dNormal.GetPixel(iTemp1, jTemp1);
                        //var colorTmp2 = t2dNormal.GetPixel(i, j);
                        var colorTmp2 = t2dNormal.GetPixel(j, i);
                        //////////////////////////////////////
                        ////r似乎是1
                        ////colorTmp.r = colorTmp2.b;b和g一样
                        //colorTmp.g = colorTmp2.a;
                        ////colorTmp.g = colorTmp2.a* colorTmp2.r;
                        //colorTmp.b = colorTmp2.g;
                        //////////////////////////////////////
                        colorTmp.a = colorTmp2.a;
                        colorList.Add(colorTmp);
                    }
                }
                //Texture2D pT2D = new Texture2D(t2dGloss.width, t2dGloss.height, TextureFormat.ARGB32, false);
                t2dGloss.SetPixels(0, 0, t2dGloss.width, t2dGloss.height, colorList.ToArray());
                t2dGloss.Apply(true);
                //SaveTextureToFile(t2dGloss, assetName);
                //用DeCompress不知道为毛会变暗
                //byte[] byt = DeCompress(t2dGloss).EncodeToPNG();
                //已经解压过了
                byte[] byt = t2dGloss.EncodeToPNG();
                // 该函数会直接覆盖已存在的同名文件
                File.WriteAllBytes(sTmp3Gloss, byt);

                AssetDatabase.Refresh();

                //Importer0.textureType = TextureImporterType.NormalMap;
                Importer0.isReadable = false;
                //设可读，后续需要设置回来
                UnityEditor.AssetDatabase.ImportAsset(sTmp3Normal);
                Importer.textureCompression = ticTmp;
                Importer.compressionQuality = qltTmp;
                Importer.isReadable = false;
                //设可读，后续需要设置回来
                UnityEditor.AssetDatabase.ImportAsset(sTmp3Gloss);
            }
            if ( EditorGUI.EndChangeCheck())
            {
                UnityEngine.Rendering.CoreUtils.SetKeyword(targetMat, "_NORMAL_IN1", enableIn1);
                EditorUtility.SetDirty(targetMat);
            }
        }
        public Texture2D DeCompress(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
        public void SaveTextureToFile(Texture2D texture, string fileName)
        {
            //sTmp3 = "Assets/TownV2/Textures/T_Decoration2_MT.TGA"
            //var bytes = texture.EncodeToPNG();
            ////string sNameTmp = sceneSplitterSettings.scenesPath + "FricTexAssets/" + fileName + ".png";
            //string sNameTmp = 
            ////var file = File.Open(Application.dataPath + "/"+ sNameTmp, FileMode.OpenOrCreate);
            //var file = File.Open(Application.dataPath + sNameTmp, FileMode.OpenOrCreate);
            //var binary = new BinaryWriter(file);
            //binary.Write(bytes);
            //binary.Flush();
            //file.Close();
            ////test1
            //AssetDatabase.Refresh();
            ////SaveTexture2DSetting("Assets/" + sNameTmp);
        }
        void SaveTexture2DSetting(string pathname)
        {
            TextureImporter Importer = AssetImporter.GetAtPath(pathname) as TextureImporter;
            //Importer.textureType = TextureImporterType.Default;
            //TextureImporterPlatformSettings setting = Importer.GetDefaultPlatformTextureSettings();
            //setting.format = TextureImporterFormat.RGBA32;
            //setting.textureCompression = TextureImporterCompression.Uncompressed;
            //Importer.SetPlatformTextureSettings(setting);
            //Importer.mipmapEnabled = true;
            Importer.isReadable = false;
            //Importer.textureCompression = TextureImporterCompression.CompressedLQ;
            //Importer.textureFormat = TextureImporterFormat.R8;

            AssetDatabase.ImportAsset(pathname);
            //AssetDatabase.Refresh();

            Debug.Log("SaveTextureToPNG " + pathname);
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

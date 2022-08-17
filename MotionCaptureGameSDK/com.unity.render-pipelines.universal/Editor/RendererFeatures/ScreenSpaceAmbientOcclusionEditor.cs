using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UnityEditor.Rendering.Universal
{
    [CustomEditor(typeof(ScreenSpaceAmbientOcclusion))]
    internal class ScreenSpaceAmbientOcclusionEditor : Editor
    {
        #region Serialized Properties
        private SerializedProperty m_Downsample;
        private SerializedProperty m_Source;
        private SerializedProperty m_NormalQuality;
        private SerializedProperty m_Intensity;
        private SerializedProperty m_DirectLightingStrength;
        private SerializedProperty m_Radius;
        private SerializedProperty m_SampleCount;

		#endregion

        private bool m_IsInitialized = false;
        public int nResolution = 512;
        public string sPicName = "SsaoTemplate";
        // Structs
        private struct Styles
        {
            public static GUIContent Downsample = EditorGUIUtility.TrTextContent("Downsample", "With this option enabled, Unity downsamples the SSAO effect texture to improve performance. Each dimension of the texture is reduced by a factor of 2.");
            public static GUIContent Source = EditorGUIUtility.TrTextContent("Source", "This option determines whether the ambient occlusion reconstructs the normal from depth or is given it from a DepthNormal/Deferred Gbuffer texture.");
            public static GUIContent NormalQuality = new GUIContent("Normal Quality", "The options in this field define the number of depth texture samples that Unity takes when computing the normals. Low: 1 sample, Medium: 5 samples, High: 9 samples.");
            public static GUIContent Intensity = EditorGUIUtility.TrTextContent("Intensity", "The degree of darkness that Ambient Occlusion adds.");
            public static GUIContent DirectLightingStrength = EditorGUIUtility.TrTextContent("Direct Lighting Strength", "Controls how much the ambient occlusion affects direct lighting.");
            public static GUIContent Radius = EditorGUIUtility.TrTextContent("Radius", "The radius around a given point, where Unity calculates and applies the effect.");
            public static GUIContent SampleCount = EditorGUIUtility.TrTextContent("Sample Count", "The number of samples that Unity takes when calculating the obscurance value. Higher values have high performance impact.");
        }

        private void Init()
        {
            SerializedProperty settings = serializedObject.FindProperty("m_Settings");
            m_Source = settings.FindPropertyRelative("Source");
            m_Downsample = settings.FindPropertyRelative("Downsample");
            m_NormalQuality = settings.FindPropertyRelative("NormalSamples");
            m_Intensity = settings.FindPropertyRelative("Intensity");
            m_DirectLightingStrength = settings.FindPropertyRelative("DirectLightingStrength");
            m_Radius = settings.FindPropertyRelative("Radius");
            m_SampleCount = settings.FindPropertyRelative("SampleCount");
            m_IsInitialized = true;
        }

        public override void OnInspectorGUI()
        {
            if (!m_IsInitialized)
            {
                Init();
            }
            DrawPropertiesExcluding(serializedObject, "m_Script");
            EditorGUILayout.PropertyField(m_Downsample, Styles.Downsample);
            EditorGUILayout.PropertyField(m_Source, Styles.Source);

            // We only enable this field when depth source is selected
            GUI.enabled = m_Source.enumValueIndex == (int) ScreenSpaceAmbientOcclusionSettings.DepthSource.Depth;
            EditorGUILayout.PropertyField(m_NormalQuality, Styles.NormalQuality);
            GUI.enabled = true;

            m_Intensity.floatValue = EditorGUILayout.Slider(Styles.Intensity,m_Intensity.floatValue, 0f, 20f);
            m_DirectLightingStrength.floatValue = EditorGUILayout.Slider(Styles.DirectLightingStrength,m_DirectLightingStrength.floatValue, 0f, 1f);
            EditorGUILayout.PropertyField(m_Radius, Styles.Radius);
            m_Radius.floatValue = Mathf.Clamp(m_Radius.floatValue, 0f, m_Radius.floatValue);
            m_SampleCount.intValue = EditorGUILayout.IntSlider(Styles.SampleCount,m_SampleCount.intValue, 4, 20);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            if (GUILayout.Button("Bake Curve"))
            {
                this.Bake();
            }

            EditorGUILayout.Space();
            GUILayout.Label("Infos unsaved!!! Remember them yourselfs if necessary!  --by KLH");
            EditorGUILayout.Space();
            //v3Direction = EditorGUILayout.Vector3Field("v3Direction", v3Direction);
            //EditorGUILayout.Space();
            //fVarCurve = EditorGUILayout.FloatField("fVarCurve", fVarCurve);
            //EditorGUILayout.Space();
            nResolution = EditorGUILayout.IntField("nResolution", nResolution);
            EditorGUILayout.Space();
            sPicName = EditorGUILayout.TextField("sPicName", sPicName);
            //EditorGUILayout.Space();
            var refTar = (ScreenSpaceAmbientOcclusion)target;
            //refTar.curveHRPHWS = EditorGUILayout.CurveField(refTar.curveHRPHWS);
            refTar.SetCurve(EditorGUILayout.CurveField(refTar.GetCurve()));
            EditorGUILayout.Space();
            if (GUILayout.Button("Apply Curve"))
            {
                refTar.SetCurve(refTar.curveHRPHWS);
            }
            //if (GUILayout.Button("Temp Copy Curve"))
            //{
            //    refTar.SetCurve(refTar.curveHRPHWS);
            //}
            EditorGUILayout.EndVertical();
        }
        public void Bake()
        {

            List<Color> colorList = new List<Color>(nResolution);
            float fT = 0;
            //Color colorTmp3;
            var refTar = (ScreenSpaceAmbientOcclusion)target;
            for (int i = 0; i < nResolution; ++i)
            {
                fT = (float)i / nResolution;
                //colorTmp3 = colorList[i];
                //colorTmp3.r = curveHRPHWS.Evaluate(fT);
                //colorList[i] = colorTmp3;

                colorList.Add(new Color(refTar.curveHRPHWS.Evaluate(fT), 0, 0, 1));
            }
            var _texture = new Texture2D(nResolution, 1, TextureFormat.R8, false, true);

            _texture.wrapMode = TextureWrapMode.Mirror;
            _texture.filterMode = FilterMode.Bilinear;
            _texture.SetPixels(0, 0, nResolution, 1, colorList.ToArray());
            _texture.Apply(false);
            SaveTextureToFile(_texture, sPicName);
        }
        public void SaveTextureToFile(Texture2D texture, string fileName)
        {
            var bytes = texture.EncodeToPNG();
            //string sNameTmp = sceneSplitterSettings.scenesPath + "FricTexAssets/" + fileName + ".png";
            string sNameTmp = "/ThirdParty/com.unity.render-pipelines.universal/Textures/" + fileName + ".png";
            var file = File.Open(Application.dataPath + sNameTmp, FileMode.OpenOrCreate);
            var binary = new BinaryWriter(file);
            binary.Write(bytes);
            binary.Flush();
            file.Close();
            //test1
            AssetDatabase.Refresh();
            SaveTexture2DSetting("Assets/" + sNameTmp);
        }
        void SaveTexture2DSetting(string pathname)
        {
            TextureImporter Importer = AssetImporter.GetAtPath(pathname) as TextureImporter;
            Importer.textureType = TextureImporterType.Default;
            TextureImporterPlatformSettings setting = Importer.GetDefaultPlatformTextureSettings();
            Importer.mipmapEnabled = false;
            Importer.textureCompression = TextureImporterCompression.CompressedLQ;
            AssetDatabase.ImportAsset(pathname);
            //AssetDatabase.Refresh();

            Debug.Log("SaveTextureToPNG " + pathname);
        }
    }
}

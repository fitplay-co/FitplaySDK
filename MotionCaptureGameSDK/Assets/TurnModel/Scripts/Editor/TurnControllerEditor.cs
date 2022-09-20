using UniRx;
using UnityEngine;
using UnityEditor;

namespace TurnModel.Scripts.Editor
{
    [CustomEditor(typeof(TurnController))]
    public class TurnControllerEditor : UnityEditor.Editor
    {
        private TurnController script;
        private GUIStyle tempFontStyle;
        private GUIStyle normalFontStyle;
        private bool isChanged = true;
        private int lastTimeId = 0;
        private void OnEnable()
        {
            script = (TurnController) target;
            tempFontStyle = new GUIStyle();
            tempFontStyle.normal.textColor = Color.yellow;
            tempFontStyle.alignment = TextAnchor.MiddleCenter;
            tempFontStyle.fontSize = 16;

            normalFontStyle = new GUIStyle();
            normalFontStyle.normal.textColor = Color.yellow;
            normalFontStyle.fontSize = 12;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            Color bc = GUI.backgroundColor;
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical("box");
            if (!serializedObject.isEditingMultipleObjects)
            {
                GUI.backgroundColor = Color.blue;
                if (script.HowToTurn == TurnController.TurnMode.UseLeftAndRight)
                {
                    GUILayout.Button($"“左右倾斜”转向模式", GUILayout.Height(40));
                    //EditorGUILayout.LabelField($"启用“左右倾斜”转向模式", tempFontStyle);
                }
                else if (script.HowToTurn == TurnController.TurnMode.UseTorsoRotation)
                {
                    GUILayout.Button($"“躯干转身”转向模式", GUILayout.Height(40));
                    normalFontStyle.normal.textColor = Color.red;
                    normalFontStyle.alignment = TextAnchor.MiddleCenter;
                    EditorGUILayout.LabelField($"---- 请正对摄像头站立 ----", normalFontStyle);
                    //EditorGUILayout.LabelField($"启用“躯干转身”转向模式", tempFontStyle);
                }

                GUI.backgroundColor = bc;
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                script.HowToTurn =
                    (TurnController.TurnMode) EditorGUILayout.EnumPopup("检测转身模式：", script.HowToTurn,
                        EditorStyles.toolbarDropDown);
                EditorGUILayout.Space();

                if (script.HowToTurn == TurnController.TurnMode.UseTorsoRotation)
                {
                    script.TurnData =
                        (TurnController.TurnDataType) EditorGUILayout.EnumPopup("转身数据来源：", script.TurnData,
                            EditorStyles.toolbarDropDown);
                }

                GUILayout.Label(
                    "____________________________________________________________________________________________________________________________________________________________________________________________");
                normalFontStyle.alignment = TextAnchor.MiddleLeft;
                normalFontStyle.normal.textColor = Color.yellow;
                if (script.HowToTurn == TurnController.TurnMode.UseTorsoRotation && script.TurnData == TurnController.TurnDataType.AndXShoulder)
                {  
                    EditorGUILayout.LabelField($"B < |X轴肩宽| < A时，Avatar的转向速率从1开始衰减。", normalFontStyle);
                    EditorGUILayout.LabelField($"肩宽越小转动速度越快! (AB为肩宽值，必须B < A)", normalFontStyle);
                    EditorGUILayout.Space();
                    script.XShoulderModeZValue = EditorGUILayout.Slider("启动肩宽计算的Z轴差值:", script.XShoulderModeZValue, 0, 1);
                    EditorGUILayout.Space();
                    script.A = EditorGUILayout.Slider("最小检测值A(m):", script.A, 0, 2);
                    EditorGUILayout.Space();
                    script.B = EditorGUILayout.Slider("最大检测值B(m):", script.B, 0, 1);
                    if(lastTimeId != 1)
                        script.SetValuesByMode();
                    lastTimeId = 1;
                }
                else
                {
                    EditorGUILayout.LabelField($"A < |angle| < B时，Avatar的转向速率从0开始增加", normalFontStyle);
                    EditorGUILayout.Space();
                    script.A = EditorGUILayout.Slider("最小检测值A(°):", script.A, 0, 30);
                    EditorGUILayout.Space();
                    script.B = EditorGUILayout.Slider("最大检测值A(°):", script.B, 0, 90);
                   
                    if(lastTimeId == 1)
                        script.SetValuesByMode();
                    lastTimeId = script.HowToTurn == TurnController.TurnMode.UseLeftAndRight ? 2 : 3;
                }
                
                
                EditorGUILayout.Space();
                script.Wmax = EditorGUILayout.Slider("最大转动速度:", script.Wmax, 0, 200);
                EditorGUILayout.Space();
                script.ReturnWmax = EditorGUILayout.Slider("回正角速度阈值:", script.ReturnWmax, 0, 30);
                EditorGUILayout.Space();
                script.SpeedCurve = EditorGUILayout.CurveField("速度曲线设定：", script.SpeedCurve);
                EditorGUILayout.Space();
                if(GUILayout.Button($"保存数据", GUILayout.Height(40)))
                {
                    if (script.HowToTurn == TurnController.TurnMode.UseLeftAndRight)
                    {
                        script.A_LR = script.A;
                        script.B_LR = script.B; 
                    }
                    else
                    {
                        if (script.TurnData == TurnController.TurnDataType.AndXShoulder)
                        {
                            script.A_SX = script.A;
                            script.B_SX = script.B;
                        }
                        else
                        {
                            script.A_FB = script.A;
                            script.B_FB = script.B;    
                        }
                    }
                    
                    script.SetValuesByMode();
                    //EditorUtility.SetDirty(script);
                    //AssetDatabase.SaveAssets();
                    if (PrefabUtility.GetPrefabParent(script) != null)
                    {
                        PrefabUtility.ReplacePrefab(script.gameObject, PrefabUtility.GetPrefabParent(script), ReplacePrefabOptions.ConnectToPrefab);
                    }
                }
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();
        }
    }
}
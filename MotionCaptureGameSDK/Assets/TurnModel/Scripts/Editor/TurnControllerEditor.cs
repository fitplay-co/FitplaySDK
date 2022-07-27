using StandTravelModel.Scripts.Runtime;
using UnityEngine;
using UnityEditor;

namespace TurnModel.Scripts.Editor
{
    [CustomEditor(typeof(TurnController)), DisallowMultipleComponent, CanEditMultipleObjects]
    public class TurnControllerEditor : UnityEditor.Editor
    {
        private TurnController script;
        private GUIStyle tempFontStyle;
        private GUIStyle normalFontStyle;

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
                script.standTravelModelManager = GameObject.FindObjectOfType<StandTravelModelManager>();
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
                EditorGUILayout.LabelField($"A < angle < B时，avatar的转向速率从0开始增加", normalFontStyle);
                EditorGUILayout.Space();
                if (script.HowToTurn == TurnController.TurnMode.UseLeftAndRight)
                {
                    script.A = script.A_LR;
                    script.A = EditorGUILayout.Slider("最小检测角度:", script.A, 0, 100);
                    script.A_LR = script.A;
                    EditorGUILayout.Space();
                    script.B = script.B_LR;
                    script.B = EditorGUILayout.Slider("最大检测角度:", script.B, 0, 100);
                    script.B_LR = script.B;
                }
                else if (script.HowToTurn == TurnController.TurnMode.UseTorsoRotation)
                {
                    script.A = script.A_FB;
                    script.A = EditorGUILayout.Slider("最小检测角度:", script.A, 0, 100);
                    script.A_FB = script.A;
                    EditorGUILayout.Space();
                    script.B = script.B_FB;
                    script.B = EditorGUILayout.Slider("最大检测角度:", script.B, 0, 100);
                    script.B_FB = script.B;
                }

                EditorGUILayout.Space();
                script.Wmax = EditorGUILayout.Slider("最大转动速度:", script.Wmax, 0, 200);
                EditorGUILayout.Space();
                script.SpeedCurve = EditorGUILayout.CurveField("速度曲线设定：", script.SpeedCurve);
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();
        }
    }
}
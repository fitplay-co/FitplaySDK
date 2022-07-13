#if UNITY_EDITOR
using StandTravelModel.Scripts.Runtime.FK.Scripts;
using UnityEditor;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Editor
{
    [CustomEditor(typeof(FKStandardPoseAnglesPrinter))]
    public class FKStandardPoseAnglesPrinterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("PrintAngles"))
            {
                var printer = target as FKStandardPoseAnglesPrinter;
                printer.PrintPoseAngles();
            }

            if(GUILayout.Button("PrintForward"))
            {
                var printer = target as FKStandardPoseAnglesPrinter;
                printer.PrintPosForwards();
            }
        }
    }
}
#endif
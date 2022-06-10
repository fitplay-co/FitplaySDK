#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace FK
{
    [CustomEditor(typeof(FKStandardPoseAnglesPrinter))]
    public class FKStandardPoseAnglesPrinterEditor : Editor
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
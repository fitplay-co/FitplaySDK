#if UNITY_EDITOR
using StandTravelModel.Scripts.Runtime.FK.Scripts;
using UnityEditor;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Editor
{
    [CustomEditor(typeof(FKTest))]
    public class FKTestEditor : UnityEditor.Editor {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Start"))
            {
                var test = target as FKTest;
                test.StartInit();
            }
        }
    }
}
#endif
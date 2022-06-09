#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace FK
{
    [CustomEditor(typeof(FKTest))]
    public class FKTestEditor : Editor {
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
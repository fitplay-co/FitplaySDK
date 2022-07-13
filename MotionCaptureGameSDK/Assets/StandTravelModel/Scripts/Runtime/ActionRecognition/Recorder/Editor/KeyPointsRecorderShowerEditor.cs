#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Recorder
{
    [CustomEditor(typeof(KeyPointsRecorderShower))]
    public class KeyPointsRecorderShowerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("WalkThrough"))
            {
                var shower = target as KeyPointsRecorderShower;
                shower.WalkThroughRecorder();
            }

            if(GUILayout.Button("Simulate"))
            {
                var shower = target as KeyPointsRecorderShower;
                shower.Simulate();
            }
        }
    }
}
#endif
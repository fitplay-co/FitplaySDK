#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Recorder.Editor
{
    [CustomEditor(typeof(KeyPointsRecorderShower))]
    public class KeyPointsRecorderShowerEditor : UnityEditor.Editor
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
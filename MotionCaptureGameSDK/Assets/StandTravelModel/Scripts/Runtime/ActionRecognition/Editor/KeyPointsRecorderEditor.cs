#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Recorder
{
    [CustomEditor(typeof(KeyPointsRecorder))]
    public class KeyPointsRecorderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Clear"))
            {
                var recorder = target as KeyPointsRecorder;
                recorder.ClearKeyPointsList();
            }

            if(GUILayout.Button("Output"))
            {
                var recorder = target as KeyPointsRecorder;
                recorder.OutputKeyPointsList();
            }

            if(GUILayout.Button("StartRecord"))
            {
                var recorder = target as KeyPointsRecorder;
                recorder.EnableRecording();
            }

            if(GUILayout.Button("StopRecord"))
            {
                var recorder = target as KeyPointsRecorder;
                recorder.DisableRecording();
            }

            if(GUILayout.Button("PrintFilePath"))
            {
                var recorder = target as KeyPointsRecorder;
                recorder.PrintFilePath();
            }
        }
    }
}
#endif
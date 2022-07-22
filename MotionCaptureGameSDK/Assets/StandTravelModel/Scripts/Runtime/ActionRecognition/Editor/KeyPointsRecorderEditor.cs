#if UNITY_EDITOR
using StandTravelModel.Scripts.Runtime.ActionRecognition.Recorder;
using UnityEditor;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.ActionRecognition.Editor
{
    [CustomEditor(typeof(KeyPointsRecorder))]
    public class KeyPointsRecorderEditor : UnityEditor.Editor
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
                recorder.OutputDatas();
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
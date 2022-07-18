#if UNITY_EDITOR
using StandTravelModel.Scripts.Runtime.FK.Scripts;
using UnityEditor;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Editor
{
    [CustomEditor(typeof(FKPoseModel))]
    public class FKPoseModelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(Application.isPlaying)
            {
                if(GUILayout.Button("ShowSkeleton"))
                {
                    var poseModel = target as FKPoseModel;
                    poseModel.ShowSkeleton();
                }
            }
        }
    }
}
#endif
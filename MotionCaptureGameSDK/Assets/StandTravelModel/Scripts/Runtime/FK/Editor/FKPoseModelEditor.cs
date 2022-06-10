#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace FK
{
    [CustomEditor(typeof(FKPoseModel))]
    public class FKPoseModelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(Application.isPlaying && GUILayout.Button("ShowSkeleton"))
            {
                var poseModel = target as FKPoseModel;
                poseModel.ShowSkeleton();
            }    
        }
    }
}
#endif
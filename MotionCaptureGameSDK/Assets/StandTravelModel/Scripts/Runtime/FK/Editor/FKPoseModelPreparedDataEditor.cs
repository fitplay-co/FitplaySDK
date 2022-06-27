#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace FK
{
    [CustomEditor(typeof(FKPoseModelPreparedData))]
    public class FKPoseModelPreparedDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("BakeData"))
            {
                var preparedData = target as FKPoseModelPreparedData;
                preparedData.BakeData();
            }
        }
    }
}
#endif
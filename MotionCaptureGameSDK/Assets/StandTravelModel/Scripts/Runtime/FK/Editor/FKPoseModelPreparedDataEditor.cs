#if UNITY_EDITOR
using StandTravelModel.Scripts.Runtime.FK.Scripts;
using UnityEditor;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Editor
{
    [CustomEditor(typeof(FKPoseModelPreparedData))]
    public class FKPoseModelPreparedDataEditor : UnityEditor.Editor
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
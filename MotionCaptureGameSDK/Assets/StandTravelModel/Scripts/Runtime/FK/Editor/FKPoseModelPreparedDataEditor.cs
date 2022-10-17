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

            if(!Application.isPlaying && GUILayout.Button("BakeData"))
            {
                var preparedData = target as FKPoseModelPreparedData;
                preparedData.BakeData();
                TryApplyPrefab(preparedData);
            }
        }

        private void TryApplyPrefab(FKPoseModelPreparedData preparedData)
        {
            var prefabObj = PrefabUtility.GetCorrespondingObjectFromSource(preparedData.gameObject);
            var prefabScr = prefabObj.GetComponent<FKPoseModelPreparedData>();
            if(prefabScr != null && EditorUtility.DisplayDialog("Apply Baked Data", "Do you want to apply the matrix to the prefab?", "Yes", "No"))
            {
                prefabScr.SetRotationCorrects(preparedData.GetRotationCorrects());
                EditorUtility.SetDirty(prefabScr);
                PrefabUtility.SavePrefabAsset(prefabObj);
            }
        }
    }
}
#endif
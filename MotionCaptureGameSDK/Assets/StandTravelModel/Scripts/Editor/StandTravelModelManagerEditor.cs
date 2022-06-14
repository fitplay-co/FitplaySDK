#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using StandTravelModel;

[CustomEditor(typeof(StandTravelModelManager))]
public class StandTravelModelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(Application.isPlaying)
        {
            var mgr = target as StandTravelModelManager;
            if(!mgr.IsFKEnabled())
            {
                if(GUILayout.Button("FK Enable"))
                {
                    mgr.EnableFK();
                }
            }
            else
            {
                if(GUILayout.Button("FK Disable"))
                {
                    mgr.DisableFK();
                }
            }
        }
    }
}
#endif
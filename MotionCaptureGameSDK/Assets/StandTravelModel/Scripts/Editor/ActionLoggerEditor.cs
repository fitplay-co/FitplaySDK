#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionLogger))]
public class ActionLoggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("OutputDatas"))
        {
            var logger = target as ActionLogger;
            logger.OutputDatas();
        }    
    }
}
#endif
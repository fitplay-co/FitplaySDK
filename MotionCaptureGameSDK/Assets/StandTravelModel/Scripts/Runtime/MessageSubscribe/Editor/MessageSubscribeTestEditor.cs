#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MessageSubscribeTest))]
public class MessageSubscribeTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("SubscribeMessages"))
        {
            var test = target as MessageSubscribeTest;
            test.MessagesSubscribe();
        }

        if(GUILayout.Button("ReleaseMessages"))
        {
            var test = target as MessageSubscribeTest;
            test.MessagesRelease();
        }
    }
}
#endif
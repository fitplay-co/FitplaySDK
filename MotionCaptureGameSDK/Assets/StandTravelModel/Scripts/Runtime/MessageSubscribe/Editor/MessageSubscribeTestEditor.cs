#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.MessageSubscribe.Editor
{
    [CustomEditor(typeof(MessageSubscribeTest))]
    public class MessageSubscribeTestEditor : UnityEditor.Editor
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
}
#endif
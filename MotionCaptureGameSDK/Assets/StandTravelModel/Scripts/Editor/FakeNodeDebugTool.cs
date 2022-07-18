using StandTravelModel;
using StandTravelModel.Scripts.Runtime;
using UnityEditor;
using UnityEngine;

namespace MotionCapture.StandTravelModel.Editor
{
    public class FakeNodeDebugTool
    {
        [MenuItem("Tools/Hide Fake Node")]
        static void HideFakeNode()
        {
            NodeReference[] fakeNodeList = Object.FindObjectsOfType<NodeReference>();
            if (fakeNodeList != null && fakeNodeList.Length != 0)
            {
                foreach (var fakeNode in fakeNodeList)
                {
                    fakeNode.NodeMeshRenderer.enabled = false;
                }
            }
        }
        
        [MenuItem("Tools/Show Fake Node")]
        static void ShowFakeNode()
        {
            NodeReference[] fakeNodeList = Object.FindObjectsOfType<NodeReference>();
            if (fakeNodeList != null && fakeNodeList.Length != 0)
            {
                foreach (var fakeNode in fakeNodeList)
                {
                    fakeNode.NodeMeshRenderer.enabled = true;
                }
            }
        }
    }
}
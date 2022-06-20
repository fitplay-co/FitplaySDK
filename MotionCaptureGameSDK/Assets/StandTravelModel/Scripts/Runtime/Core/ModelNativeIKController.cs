using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using StandTravelModel.Core.Interface;
using UnityEngine;

namespace StandTravelModel.Core
{
    public enum NativeIKNodeType
    {
        Nose = 0,
        LeftElbow,
        RightElbow,
        LeftHand,
        RightHand,
        LeftIndex,
        RightIndex,
        LeftKnee,
        RightKnee,
        LeftFoot,
        RightFoot,
        LeftFootIndex,
        RightFootIndex
    }

    public class ModelNativeIKController : IModelIKController
    {
        private GameObject fakeNodePrefab;
        private List<GameObject> fakeNodeList;
        private UnityIKMecanimScript unityIKMecanimScript;
        private const int CountOfNativeIKReference = 13;
        public ModelNativeIKController(GameObject fakeNodePrefab, UnityIKMecanimScript unityIKMecanimScript)
        {
            this.fakeNodePrefab = fakeNodePrefab;
            this.unityIKMecanimScript = unityIKMecanimScript;
            fakeNodeList = new List<GameObject>();
        }

        public void ClearFakeNodes()
        {
            foreach (var fakeNode in fakeNodeList)
            {
                UnityEngine.Object.Destroy(fakeNode);
            }
        }

        public void InitializeIKTargets(Transform ikPointsRoot)
        {
            for (int i = 0; i < CountOfNativeIKReference; i++)
            {
                var currentNode =
                    UnityEngine.Object.Instantiate(fakeNodePrefab, Vector3.zero, Quaternion.identity, ikPointsRoot);
                var nodeReference = currentNode.GetComponent<NodeReference>();
                if (nodeReference != null)
                {
                    nodeReference.NodeLabel.text = i.ToString();
                }
                fakeNodeList.Add(currentNode);
            }

            //TODO: 头朝向控制IK需要找到更合适的点
            //unityIKMecanimScript.lookPoint = fakeNodeList[(int)NativeIKNodeType.Nose].transform;
            unityIKMecanimScript.IK_leftElbowTarget = fakeNodeList[(int)NativeIKNodeType.LeftElbow].transform;
            unityIKMecanimScript.IK_rightElbowTarget = fakeNodeList[(int)NativeIKNodeType.RightElbow].transform;
            unityIKMecanimScript.IK_leftHandTarget = fakeNodeList[(int)NativeIKNodeType.LeftHand].transform;
            unityIKMecanimScript.IK_rightHandTarget = fakeNodeList[(int)NativeIKNodeType.RightHand].transform;
            unityIKMecanimScript.IK_leftKneeTarget = fakeNodeList[(int)NativeIKNodeType.LeftKnee].transform;
            unityIKMecanimScript.IK_rightKneeTarget = fakeNodeList[(int)NativeIKNodeType.RightKnee].transform;
            unityIKMecanimScript.IK_leftFootTarget = fakeNodeList[(int)NativeIKNodeType.LeftFoot].transform;
            unityIKMecanimScript.IK_rightFootTarget = fakeNodeList[(int)NativeIKNodeType.RightFoot].transform;
        }

        public void UpdateIKTargetsData(List<Vector3> keyPointsList)
        {
            fakeNodeList[(int)NativeIKNodeType.Nose].transform.localPosition = keyPointsList[(int)GameKeyPointsType.Nose];
            fakeNodeList[(int)NativeIKNodeType.LeftElbow].transform.localPosition = keyPointsList[(int)GameKeyPointsType.LeftElbow];
            fakeNodeList[(int)NativeIKNodeType.RightElbow].transform.localPosition = keyPointsList[(int)GameKeyPointsType.RightElbow];
            fakeNodeList[(int)NativeIKNodeType.LeftHand].transform.localPosition = keyPointsList[(int)GameKeyPointsType.LeftHand];
            fakeNodeList[(int)NativeIKNodeType.RightHand].transform.localPosition = keyPointsList[(int)GameKeyPointsType.RightHand];
            fakeNodeList[(int)NativeIKNodeType.LeftIndex].transform.localPosition = keyPointsList[(int)GameKeyPointsType.LeftIndex];
            fakeNodeList[(int)NativeIKNodeType.RightIndex].transform.localPosition = keyPointsList[(int)GameKeyPointsType.RightIndex];
            fakeNodeList[(int)NativeIKNodeType.LeftKnee].transform.localPosition = keyPointsList[(int)GameKeyPointsType.LeftKnee];
            fakeNodeList[(int)NativeIKNodeType.RightKnee].transform.localPosition = keyPointsList[(int)GameKeyPointsType.RightKnee];
            fakeNodeList[(int)NativeIKNodeType.LeftFoot].transform.localPosition = keyPointsList[(int)GameKeyPointsType.LeftFoot];
            fakeNodeList[(int)NativeIKNodeType.RightFoot].transform.localPosition = keyPointsList[(int)GameKeyPointsType.RightFoot];
            fakeNodeList[(int)NativeIKNodeType.LeftFootIndex].transform.localPosition = keyPointsList[(int)GameKeyPointsType.LeftFootIndex];
            fakeNodeList[(int)NativeIKNodeType.RightFootIndex].transform.localPosition = keyPointsList[(int)GameKeyPointsType.RightFootIndex];
            
            //计算手脚朝向
            CalculateNodeForward(fakeNodeList[(int) NativeIKNodeType.LeftHand],
                fakeNodeList[(int) NativeIKNodeType.LeftIndex]);
            CalculateNodeForward(fakeNodeList[(int) NativeIKNodeType.RightHand],
                fakeNodeList[(int) NativeIKNodeType.RightIndex]);
            CalculateNodeForward(fakeNodeList[(int) NativeIKNodeType.LeftFoot],
                fakeNodeList[(int) NativeIKNodeType.LeftFootIndex]);
            CalculateNodeForward(fakeNodeList[(int) NativeIKNodeType.RightFoot],
                fakeNodeList[(int) NativeIKNodeType.RightFootIndex]);
        }

        private void CalculateNodeForward(GameObject targetNode, GameObject forwardToPoint)
        {
            var towardVector = forwardToPoint.transform.position - targetNode.transform.position;
            targetNode.transform.forward = Vector3.Normalize(towardVector);
        }

        public void ChangeLowerBodyIKWeight(float weight)
        {
            unityIKMecanimScript.IK_leftFootPositionWeight = weight;
            unityIKMecanimScript.IK_leftFootRotationWeight = weight;
            unityIKMecanimScript.IK_rightFootPositionWeight = weight;
            unityIKMecanimScript.IK_rightFootRotationWeight = weight;
            unityIKMecanimScript.IK_leftKneePositionWeight = weight;
            unityIKMecanimScript.IK_rightKneePositionWeight = weight;
        }

        public void ChangeUpperBodyIKWeight(float weight)
        {
            unityIKMecanimScript.IK_leftHandPositionWeight = weight;
            unityIKMecanimScript.IK_leftHandRotationWeight = weight;
            unityIKMecanimScript.IK_rightHandPositionWeight = weight;
            unityIKMecanimScript.IK_rightHandRotationWeight = weight;
            unityIKMecanimScript.IK_leftElbowPositionWeight = weight;
            unityIKMecanimScript.IK_rightElbowPositionWeight = weight;
        }

        public void Enable()
        {
            
        }
    }
}
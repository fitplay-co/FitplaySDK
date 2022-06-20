using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using RootMotion.FinalIK;
using StandTravelModel.Core.Interface;
using UnityEngine;

namespace StandTravelModel.Core
{
    public class ModelFinalIKController : IModelIKController
    {
        private GameObject fakeNodePrefab;
        private List<GameObject> fakeNodeList;
        private FullBodyBipedIK fbbIK;
        private LookAtIK lookAtIK;
        private const int CountOfFinalIKReference = 17;
        
        private float _skewCorrection;

        public float skewCorrection
        {
            set => _skewCorrection = value;
            get => _skewCorrection;
        }
        
        public ModelFinalIKController(GameObject fakeNodePrefab, FullBodyBipedIK fbbIK, LookAtIK lookAtIK)
        {
            this.fakeNodePrefab = fakeNodePrefab;
            this.fbbIK = fbbIK;
            this.lookAtIK = lookAtIK;
            fakeNodeList = new List<GameObject>();
            InitializeWeight();
        }
        
        private void InitializeWeight()
        {
            fbbIK.solver.leftShoulderEffector.positionWeight = 1;
            fbbIK.solver.rightShoulderEffector.positionWeight = 1;
            fbbIK.solver.leftArmChain.bendConstraint.weight = 1;
            fbbIK.solver.rightArmChain.bendConstraint.weight = 1;
            fbbIK.solver.leftHandEffector.positionWeight = 1;
            fbbIK.solver.rightHandEffector.positionWeight = 1;
            fbbIK.solver.leftThighEffector.positionWeight = 1;
            fbbIK.solver.rightThighEffector.positionWeight = 1;
            fbbIK.solver.leftLegChain.bendConstraint.weight = 1;
            fbbIK.solver.rightLegChain.bendConstraint.weight = 1;
            fbbIK.solver.leftFootEffector.positionWeight = 1;
            fbbIK.solver.rightFootEffector.positionWeight = 1;
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
            for (int i = 0; i < CountOfFinalIKReference; i++)
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
            //lookAtIK.solver.target = fakeNodeList[(int) GameKeyPointsType.Nose].transform;
            fbbIK.solver.leftShoulderEffector.target = fakeNodeList[(int) GameKeyPointsType.LeftShoulder].transform;
            fbbIK.solver.rightShoulderEffector.target = fakeNodeList[(int) GameKeyPointsType.RightShoulder].transform;
            fbbIK.solver.leftArmChain.bendConstraint.bendGoal = fakeNodeList[(int) GameKeyPointsType.LeftElbow].transform;
            fbbIK.solver.rightArmChain.bendConstraint.bendGoal = fakeNodeList[(int) GameKeyPointsType.RightElbow].transform;
            fbbIK.solver.leftHandEffector.target = fakeNodeList[(int) GameKeyPointsType.LeftHand].transform;
            fbbIK.solver.rightHandEffector.target = fakeNodeList[(int) GameKeyPointsType.RightHand].transform;
            fbbIK.solver.leftThighEffector.target = fakeNodeList[(int) GameKeyPointsType.LeftHip].transform;
            fbbIK.solver.rightThighEffector.target = fakeNodeList[(int) GameKeyPointsType.RightHip].transform;
            fbbIK.solver.leftLegChain.bendConstraint.bendGoal = fakeNodeList[(int) GameKeyPointsType.LeftKnee].transform;
            fbbIK.solver.rightLegChain.bendConstraint.bendGoal = fakeNodeList[(int) GameKeyPointsType.RightKnee].transform;
            fbbIK.solver.leftFootEffector.target = fakeNodeList[(int) GameKeyPointsType.LeftFoot].transform;
            fbbIK.solver.rightFootEffector.target = fakeNodeList[(int) GameKeyPointsType.RightFoot].transform;
        }

        public void UpdateIKTargetsData(List<Vector3> keyPointsList)
        {
            for (int i = 0; i < CountOfFinalIKReference; i++)
            {
                var keyPoint = keyPointsList[i];
                keyPoint.z += keyPoint.y * _skewCorrection;
                fakeNodeList[i].transform.localPosition = keyPointsList[i];
            }
        }

        public void ChangeLowerBodyIKWeight(float weight)
        {
            /*fbbIK.solver.leftArmChain.bendConstraint.weight = weight;
            fbbIK.solver.rightArmChain.bendConstraint.weight = weight;
            fbbIK.solver.leftShoulderEffector.positionWeight = weight;
            fbbIK.solver.rightShoulderEffector.positionWeight = weight;*/
            fbbIK.solver.leftThighEffector.positionWeight = weight;
            fbbIK.solver.rightThighEffector.positionWeight = weight;
            fbbIK.solver.leftFootEffector.positionWeight = weight;
            fbbIK.solver.rightFootEffector.positionWeight = weight;
            fbbIK.solver.leftLegChain.bendConstraint.weight = weight;
            fbbIK.solver.rightLegChain.bendConstraint.weight = weight;
        }
    }
}
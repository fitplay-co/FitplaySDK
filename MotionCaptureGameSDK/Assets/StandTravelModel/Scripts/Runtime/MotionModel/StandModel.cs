using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using StandTravelModel.Core;
using UnityEngine;

namespace StandTravelModel.MotionModel
{
    public class StandModel : MotionModelBase
    {
        private Quaternion predictBodyRotation;

        public StandModel(
            Transform selfTransform,
            Transform characterHipNode,
            Transform characterHeadNode,
            Transform keyPointsParent,
            TuningParameterGroup tuningParameters,
            IMotionDataModel motionDataModel,
            AnchorController anchorController)
            : base(
                selfTransform,
                characterHipNode,
                characterHeadNode,
                keyPointsParent,
                tuningParameters,
                motionDataModel,
                anchorController)
        {
        }

        public override void OnLateUpdate()
        {
            anchorController.TravelFollowPoint.transform.position =
                anchorController.StandFollowPoint.transform.position + localShift;
            /*selfTransform.rotation = anchorController.TravelFollowPoint.transform.rotation *
                                        predictBodyRotation;*/
            base.OnLateUpdate();
        }

        public override void OnUpdate(List<Vector3> keyPoints)
        {
            base.OnUpdate(keyPoints);
            //CalculateBodyRotation(keyPoints);
        }

        private void CalculateBodyRotation(List<Vector3> keyPoints)
        {
            Vector3 bodyForwardOriginal =
                Vector3.Cross(
                    keyPoints[(int) GameKeyPointsType.LeftShoulder] - keyPoints[(int) GameKeyPointsType.RightHip],
                    keyPoints[(int) GameKeyPointsType.LeftHip] - keyPoints[(int) GameKeyPointsType.RightShoulder]);
            bodyForwardOriginal = Vector3.Normalize(bodyForwardOriginal);

            predictBodyRotation = Quaternion.FromToRotation(Vector3.forward, bodyForwardOriginal);
        }
    }
}
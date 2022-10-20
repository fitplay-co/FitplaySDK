using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime.Core;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.MotionModel
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
            AnchorController anchorController,
            MotionModelInteractData interactData)
            : base(
                selfTransform,
                characterHipNode,
                characterHeadNode,
                keyPointsParent,
                tuningParameters,
                motionDataModel,
                anchorController,
                interactData)
        {
        }

        public override void OnLateUpdate()
        {
            anchorController.TravelFollowPoint.transform.position =
                anchorController.StandFollowPoint.transform.position + interactData.localShift;
            /*selfTransform.rotation = anchorController.TravelFollowPoint.transform.rotation *
                                        predictBodyRotation;*/
            var parent = selfTransform.parent;
            if (parent == null)
            {
                selfTransform.rotation = anchorController.TravelFollowPoint.transform.rotation;
            }
            else
            {
                parent.rotation = anchorController.TravelFollowPoint.transform.rotation;
            }
            
            CheckGroundHeight();
            
            base.OnLateUpdate();
        }

        public void ResetLocomotion()
        {
            locomotionComp.ResetLocomotion();
        }

        public void AdjustTransformHeight() 
        {
            CheckGroundHeight();
            var currentPos = selfTransform.position;
            currentPos.y = interactData.groundHeight;
            
            var parent = selfTransform.parent;
            if (parent == null)
            {
                selfTransform.position = currentPos;
            }
            else
            {
                parent.position = currentPos;
            }
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
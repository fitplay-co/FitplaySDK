using System.Collections.Generic;
using MotionCaptureBasic.Interface;
using UnityEngine;

namespace StandTravelModel.Core
{
    public class TravelModel : MotionModelBase
    {
        public int currentLeg => animatorController.currentLeg;
        public float currentFrequency => animatorController.currentFrequency;

        private TravelModelAnimatorController animatorController;

        public TravelModel(
            Transform selfTransform,
            Transform characterHipNode,
            Transform keyPointsParent,
            TuningParameterGroup tuningParameters,
            IMotionDataModel motionDataModel,
            AnchorController anchorController,
            AnimatorSettingGroup animatorSettingGroup
            ) : base(
                selfTransform,
                characterHipNode,
                keyPointsParent,
                tuningParameters,
                motionDataModel,
                anchorController
            )
        {
            animatorController = new TravelModelAnimatorController(selfTransform.GetComponent<Animator>(), motionDataModel, animatorSettingGroup);
        }

        public override void OnLateUpdate()
        {
            anchorController.StandFollowPoint.transform.position =
                        anchorController.TravelFollowPoint.transform.position - localShift;
                    selfTransform.rotation = anchorController.TravelFollowPoint.transform.rotation;

            base.OnLateUpdate();
        }

        public override void OnUpdate(List<Vector3> keyPoints)
        {
            base.OnUpdate(keyPoints);
            animatorController.UpdateTravelAnimator();
        }

        public void StopPrevAnimation(string currentState)
        {
            animatorController.StopPrevAnimation(currentState);
        }

        public override void Clear()
        {
            base.Clear();
            animatorController.Clear();
            animatorController = null;
        }
    }
}
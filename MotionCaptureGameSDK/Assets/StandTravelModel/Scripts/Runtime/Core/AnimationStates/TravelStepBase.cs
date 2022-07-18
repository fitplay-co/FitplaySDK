using MotionCaptureBasic.Interface;
using StandTravelModel.Scripts.Runtime.ActionRecognition.ActionReconComponents;
using StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components;
using StandTravelModel.Scripts.Runtime.MotionModel;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates
{
    public abstract class TravelStepBase : TravelBaseState
    {
        private ReconCompAngleGetterWithDirect angleGetterWithDirectLeft;
        private ReconCompAngleGetterWithDirect angleGetterWithDirectRight;
        private StepStateAnimatorParametersSetter parametersSetter;

        protected TravelStepBase(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter) : base(owner)
        {
            this.parametersSetter = parametersSetter;
            this.angleGetterWithDirectLeft = new ReconCompAngleGetterWithDirect(GameKeyPointsType.LeftKnee, GameKeyPointsType.LeftHip, GameKeyPointsType.Nose, Vector3.up);
            this.angleGetterWithDirectRight = new ReconCompAngleGetterWithDirect(GameKeyPointsType.RightKnee, GameKeyPointsType.RightKnee, GameKeyPointsType.Nose, Vector3.up);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            parametersSetter.TrySetStepParameters();
        }

        private float GetHipAngle(int leftLeg, int rightLeg)
        {
            if(leftLeg != 0)
            {
                return angleGetterWithDirectLeft.GetAngle(travelOwner.GetKeyPoints());
            }
            else if(rightLeg != 0)
            {
                return angleGetterWithDirectRight.GetAngle(travelOwner.GetKeyPoints());
            }

            return -1;
        }
    }
}
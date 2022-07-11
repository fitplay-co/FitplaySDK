using StandTravelModel.MotionModel;

namespace StandTravelModel.Core.AnimationStates
{
    public class TravelIdleState : AnimationStateBase
    {
        private StepStateAnimatorParametersSetter parametersSetter;

        public TravelIdleState(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter) : base(owner)
        {
            InitFields(AnimationList.Idle);
            this.parametersSetter = parametersSetter;
        }

        public override void Enter()
        {
            //Debug.Log("TravelIdleState:Enter");
            travelOwner.currentLeg = 0;
            travelOwner.currentFrequency = 0;
            travelOwner.UpdateAnimatorCadence();
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            var actionDetectionData = travelOwner.selfMotionDataModel.GetActionDetectionData();
            /*if (actionDetectionData.jump != null)
            {
                Debug.LogError($"Jump: {actionDetectionData.jump.up}, Strength: {actionDetectionData.jump.strength}");
                if (actionDetectionData.jump.up == 1)
                {
                    travelOwner.ChangeState(AnimationList.Jump);
                    return;
                }
            }*/

            //TODO: 等os完善数据
            /*if (actionDetectionData.squat != null)
            {
                travelOwner.ChangeState(AnimationList.Squat);
                return;
            }*/

            if (actionDetectionData != null && actionDetectionData.walk != null)
            {
                parametersSetter.TrySetParametersLegs();
                parametersSetter.TrySetParammeterFootHeightDiff();

                travelOwner.EnqueueStep(actionDetectionData.walk.legUp);
                travelOwner.currentLeg = actionDetectionData.walk.legUp;
                travelOwner.currentFrequency = actionDetectionData.walk.frequency / 60f;
                travelOwner.UpdateAnimatorCadence();
                //Debug.LogError($"Leg: {actionDetectionData.walk.legUp}, Frequency: {actionDetectionData.walk.frequency}, Strength: {actionDetectionData.walk.strength}");

                var isRunReady = travelOwner.IsEnterRunReady();
                isRunReady = false;         //for debuging

                if (actionDetectionData.walk.legUp != 0 && isRunReady)
                {
                    travelOwner.ChangeState(AnimationList.Run);
                    return;
                }

                if (actionDetectionData.walk.leftLeg != 0)
                {
                    travelOwner.ChangeState(AnimationList.LeftStep);
                    return;
                }
                
                if (actionDetectionData.walk.rightLeg != 0)
                {
                    travelOwner.ChangeState(AnimationList.RightStep);
                    return;
                }
            }
        }

        public override void Exit()
        {
            //Debug.Log("TravelIdleState:Exit");
            base.Exit();
        }
    }
}
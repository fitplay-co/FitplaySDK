using StandTravelModel.Scripts.Runtime.MotionModel;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates
{
    public class TravelRunState : AnimationStateBase
    {
        private Vector3 velocity;
        private float speedMultipler = 0.6f;
        
        public TravelRunState(MotionModelBase owner) : base(owner)
        {
            InitFields(AnimationList.Run);
        }

        public override void Enter()
        {
            //Debug.Log("TravelRunState:Enter");
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

            if (actionDetectionData.walk != null)
            {
                //Debug.LogError($"Leg: {actionDetectionData.walk.legUp}, Frequency: {actionDetectionData.walk.frequency}, Strength: {actionDetectionData.walk.strength}");
                travelOwner.EnqueueStep(actionDetectionData.walk.legUp);
                travelOwner.currentLeg = actionDetectionData.walk.legUp;
                travelOwner.currentFrequency = actionDetectionData.walk.frequency / 60f;
                travelOwner.UpdateAnimatorCadence();
                
                if (actionDetectionData.walk.legUp == 0)
                {
                    travelOwner.ChangeState(AnimationList.Idle);
                    return;
                }
                
                var isRunReady = travelOwner.IsEnterRunReady();
                if (!isRunReady)
                {
                    if (actionDetectionData.walk.legUp == -1)
                    {
                        travelOwner.ChangeState(AnimationList.LeftStep);
                        return;
                    }
                
                    if (actionDetectionData.walk.legUp == 1)
                    {
                        travelOwner.ChangeState(AnimationList.RightStep);
                        return;
                    }
                }
                
                velocity = travelOwner.GetAnchorController().TravelFollowPoint.transform.rotation * Vector3.forward * 
                           travelOwner.currentFrequency * 2 * speedMultipler;
                
                travelOwner.UpdateVelocity(velocity);
            }
        }

        public override void Exit()
        {
            //Debug.Log("TravelRunState:Exit");
            base.Exit();
        }
    }
}
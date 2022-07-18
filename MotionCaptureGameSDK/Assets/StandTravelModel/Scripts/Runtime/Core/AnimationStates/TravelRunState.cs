using StandTravelModel.Scripts.Runtime.MotionModel;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.Core.AnimationStates.Components
{
    public class TravelRunState : AnimationStateBase
    {
        private int animIdIsRun;
        private int animIdRunFreq;
        private StateFaderRun stateFaderRunIn;
        private StateFaderRun stateFaderRunOut;

        private StepStateAnimatorParametersSetter parametersSetter;

        public TravelRunState(MotionModelBase owner, StepStateAnimatorParametersSetter parametersSetter) : base(owner)
        {
            this.animIdIsRun = Animator.StringToHash("isRun");
            this.animIdRunFreq = Animator.StringToHash("runFrequency");
            this.stateFaderRunIn = new StateFaderRun(owner.GetAnimator(), 2, "runTransition");
            this.stateFaderRunOut = new StateFaderRun(owner.GetAnimator(), 4, "runTransition");
            this.stateFaderRunOut.SetPause(true);
            this.parametersSetter = parametersSetter;
            InitFields(AnimationList.Run);
        }

        public override void Enter()
        {
            base.Enter();
            travelOwner.selfAnimator.SetBool(animIdIsRun, true);
            stateFaderRunIn.Reset();
            stateFaderRunOut.Reset(true);
            stateFaderRunOut.SetPause(true);
        }

        public override void Tick(float deltaTime)
        {
            var actionDetectionData = travelOwner.selfMotionDataModel.GetActionDetectionData();
            if (actionDetectionData.walk != null)
            {
                if(!stateFaderRunOut.IsPaused() && !stateFaderRunOut.IsComplete())
                {
                    stateFaderRunOut.OnUdpate();
                }
                else
                {
                    stateFaderRunIn.OnUdpate();
                    travelOwner.EnqueueStep(actionDetectionData.walk.legUp);
                    travelOwner.currentLeg = actionDetectionData.walk.legUp;
                    travelOwner.currentFrequency = actionDetectionData.walk.leftFrequency;
                    travelOwner.UpdateAnimatorCadence();
                    
                    
                    
                    var isRunReady = runConditioner.IsEnterRunReady(actionDetectionData.walk);
                    if (!isRunReady)
                    {
                        if (actionDetectionData.walk.leftLeg != 0)
                        {
                            stateFaderRunOut.SetPause(false);
                            stateFaderRunOut.SetCompleteEvent(() => OnTransitionToIdleEnd(AnimationList.LeftStep));
                            return;
                        }
                    
                        if (actionDetectionData.walk.rightLeg != 0)
                        {
                            stateFaderRunOut.SetPause(false);
                            stateFaderRunOut.SetCompleteEvent(() => OnTransitionToIdleEnd(AnimationList.RightStep));
                            return;
                        }

                        if (actionDetectionData.walk.leftLeg == 0 && actionDetectionData.walk.rightLeg == 0)
                        {
                            stateFaderRunOut.SetPause(false);
                            stateFaderRunOut.SetCompleteEvent(() => OnTransitionToIdleEnd(AnimationList.Idle));
                            return;
                        }
                    }
                }

                UpdateRunSpeed(actionDetectionData.walk.leftFrequency);
                parametersSetter.TrySetStepParameters();
            }
        }

        public override void Exit()
        {
            base.Exit();
            travelOwner.selfAnimator.SetBool(animIdIsRun, false);
            travelOwner.selfAnimator.SetFloat("runTransition", 0);
            travelOwner.selfAnimator.SetTrigger("runFade");
        }

        private void UpdateRunSpeed(float stepFrequency)
        {
            travelOwner.selfAnimator.SetFloat(animIdRunFreq, stepFrequency * 0.12f);
        }

        private void OnTransitionToIdleEnd(AnimationList nextState)
        {
            travelOwner.ChangeState(nextState);
        }
    }
}
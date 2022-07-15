using MotionCaptureBasic.FSM;
using StandTravelModel.MotionModel;
using UnityEngine;

namespace StandTravelModel.Core.AnimationStates
{
    public class AnimationStateBase : State<MotionModelBase>
    {
        protected TravelModel travelOwner;
        protected RunConditioner runConditioner;

        private int stateTransParamHash;

        private AnimationList _stateAnimName;

        public AnimationList stateAnimName => _stateAnimName;

        protected AnimationStateBase(MotionModelBase owner) : base(owner)
        {
            travelOwner = owner as TravelModel;
            runConditioner = new RunConditioner();
        }

        protected void InitFields(AnimationList animName)
        {
            _stateAnimName = animName;
            if (travelOwner != null)
            {
                if (travelOwner.animSettingMap.TryGetValue(stateAnimName, out var animParam))
                {
                    stateTransParamHash = Animator.StringToHash(animParam.stateTransition);
                }
                else
                {
                    stateTransParamHash = 0;
                }
            }
        }

        public override void Enter()
        {
            if (stateTransParamHash != 0)
            {
                travelOwner.selfAnimator.SetBool(stateTransParamHash, true);
            }
        }

        public override void Exit()
        {
            if (stateTransParamHash != 0)
            {
                travelOwner.selfAnimator.SetBool(stateTransParamHash, false);
            }
        }
    }
}
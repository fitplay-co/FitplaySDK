using System.Collections.Generic;
using RootMotion.FinalIK.FitPlayProcedural;

namespace VRIK.State
{
    public class LocomotionFsmComponent : StateComponent
    {
        public LocomotionStateMachine<LocomotionFsmComponent> StateMachine { private set; get; }
        public Dictionary<WalkStateEnum, LocomotionState<LocomotionFsmComponent>> States { private set; get; }

        
        public override void OnInit()
        {
            States = new Dictionary<WalkStateEnum, LocomotionState<LocomotionFsmComponent>>
            {
                {WalkStateEnum.WalkStart, new ProceduralStartWalkingState(this)},
                {WalkStateEnum.WalkLoop, new ProceduralLoopWalkingState(this)},
                {WalkStateEnum.WalkCatchup, new ProceduralWalkCatchUpState(this)}
            };

            LocomotionState<LocomotionFsmComponent> initState = States[WalkStateEnum.WalkStart];
            StateMachine = new LocomotionStateMachine<LocomotionFsmComponent>(initState);

            base.OnInit();
        }

        private void OnFixedTick(float dt)
        {
            StateMachine.OnUpdate(dt);
        }
    }
}
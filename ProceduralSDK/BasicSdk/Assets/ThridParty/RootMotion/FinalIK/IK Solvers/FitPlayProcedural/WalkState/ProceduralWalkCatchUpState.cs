using UnityEngine;
using VRIK.State;

namespace RootMotion.FinalIK.FitPlayProcedural

{
    public class ProceduralWalkCatchUpState : ProceduralState
    {
        private IKProceduralFootstep activeFootStep;
        private IKProceduralFootstep catchupFootstep;

        public ProceduralWalkCatchUpState(LocomotionFsmComponent owner) : base(owner)
        {
        }


        public override void Enter()
        {
            /*for (int i = 0; i < footsteps.Length; i++)
            {
                if (footsteps[i].isStepping)
                {
                    activeFootStep = footsteps[1-i];
                    catchupFootstep =footsteps[i]; 
                    break;
                }
            }*/
            activeFootStep = sharedInfo.footsteps[(int)sharedInfo.LastStepedFoot];
            catchupFootstep = sharedInfo.footsteps[1 - (int)sharedInfo.LastStepedFoot];
            base.Enter();
        }

        public override LocomotionState<LocomotionFsmComponent> Event()
        {
            if (calcModule.dataInput.currentFoot != CurrentFoot.same)
            {
                return Owner.States[WalkStateEnum.WalkStart];
            }

            return base.Event();
        }

        public override void Exit()
        {
            for (int i = 0; i < sharedInfo.footsteps.Length; i++)
            {
                sharedInfo.footsteps[i].isSupportLeg = supportLegIndex == i;

                if (sharedInfo.footsteps[i].isStepping)
                {
                    sharedInfo.footsteps[i].UpdateStepping(calcModule.forwardRotation, 1);
                    lastStepOffset = sharedInfo.footsteps[i].stepTo;
                }
                else
                {
                    sharedInfo.footsteps[i].UpdateStanding(calcModule.forwardRotation, setup.relaxLegTwistMinAngle, 1);
                }

                sharedInfo.footsteps[i].Update(stepInterpolation, sharedInfo.footsteps[i].onFootstep);
            }

            base.Exit();
        }

        public override void Update(float deltaTime)
        {
            

            /*if (activeFootStep.isStepping)
            {
                activeFootStep.UpdateStepping(calcModule.forwardRotation, 1);
                catchupFootstep.UpdateStanding(calcModule.forwardRotation, setup.relaxLegTwistMinAngle, 1);
                for (int i = 0; i < footsteps.Length; i++)
                {
                    footsteps[i].Update(stepInterpolation, footsteps[i].onFootstep, deltaTime);
                }
            }
            else
            {
                Vector3 stepTo = calcModule.MirrorStepToPosition(activeFootStep, catchupFootstep, calcModule.forwardRotation);
                //随机？footsteps[currentFoot].stepSpeed = 3f;
                catchupFootstep.StepTo(stepTo, calcModule.forwardRotation, setup.stepThreshold * avatarInfo.scale);
            }*/
            
            Vector3 stepTo = calcModule.MirrorStepToPosition(activeFootStep, catchupFootstep, calcModule.forwardRotation);
            //随机？footsteps[currentFoot].stepSpeed = 3f;
            catchupFootstep.StepTo(stepTo, calcModule.forwardRotation, setup.stepThreshold * avatarInfo.scale);

            for (int i = 0; i < sharedInfo.footsteps.Length; i++)
            {
                if (!sharedInfo.footsteps[i].isStepping)
                    continue;
                
                sharedInfo.footsteps[i].isSupportLeg = supportLegIndex == i;

                //平滑更新
                sharedInfo.footsteps[i].SimulateUpdate(stepInterpolation, sharedInfo.footsteps[i].onFootstep, deltaTime);
                
            }
            

            RefreshData(deltaTime);
        }
    }
}
using UnityEngine;
using VRIK.State;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    public class ProceduralLoopWalkingState : ProceduralState
    {
        public ProceduralLoopWalkingState(LocomotionFsmComponent owner) : base(owner)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private bool SwitchFoot(float deltaTime)
        {
            if (sharedInfo.LastStepedFoot == calcModule.dataInput.currentFoot)
                return false;
            calcModule.maxHeight = 0f;
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


            return true;
        }
        
        
        public override LocomotionState<LocomotionFsmComponent> Event()
        {
            if (calcModule.dataInput.currentFoot == CurrentFoot.same)
            {
                return Owner.States[WalkStateEnum.WalkCatchup];
            }

            return base.Event();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (SwitchFoot(deltaTime))
            {
                if (calcModule.dataInput.currentFoot != CurrentFoot.same)
                {
                    sharedInfo.LastStepedFoot = calcModule.dataInput.currentFoot;    
                }
                
                RefreshData(deltaTime);
                return;
            }

            if (calcModule.dataInput.currentFoot != CurrentFoot.same)
            {
                sharedInfo.LastStepedFoot = calcModule.dataInput.currentFoot;
                //如果有在移动的脚，将脚属性刷新，赋值
                int currentFoot = calcModule.dataInput.currentFoot.GetHashCode();

                Vector3 stepTo = calcModule.CalculateStepToPosition(sharedInfo.footsteps[currentFoot], avatarInfo.rootBone.solverRotation, lastStepOffset);


                //随机？footsteps[currentFoot].stepSpeed = 3f;
                sharedInfo.footsteps[currentFoot].StepTo(stepTo, calcModule.forwardRotation, setup.stepThreshold * avatarInfo.scale);
            }

            for (int i = 0; i < sharedInfo.footsteps.Length; i++)
            {
                sharedInfo.footsteps[i].isSupportLeg = supportLegIndex == i;

                if (sharedInfo.footsteps[i].isStepping)
                {
                    sharedInfo.footsteps[i].UpdateStepping(calcModule.forwardRotation, stateProgess);
                }
                else
                {
                    sharedInfo.footsteps[i].UpdateStanding(calcModule.forwardRotation, setup.relaxLegTwistMinAngle, stateProgess);
                }

                //平滑更新
                sharedInfo.footsteps[i].Update(stepInterpolation, sharedInfo.footsteps[i].onFootstep);
            }

            RefreshData(deltaTime);
        }
    }
}
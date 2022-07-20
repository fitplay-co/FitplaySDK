using UnityEngine;
using VRIK.State;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    //起步阶段
    public class ProceduralStartWalkingState : ProceduralState
    {
        public ProceduralStartWalkingState(LocomotionFsmComponent owner) : base(owner)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            //sharedInfo.LastStepedFoot = calcModule.dataInput.currentFoot;
        }

        public override LocomotionState<LocomotionFsmComponent> Event()
        {
            if (sharedInfo.LastStepedFoot != calcModule.dataInput.currentFoot && calcModule.dataInput.currentFoot != CurrentFoot.same)
                return Owner.States[WalkStateEnum.WalkLoop];
            return base.Event();
        }

        public override void Update(float deltaTime)
        {
            //Owner.StateMachine.ChangeState(Owner.States[WalkStateEnum.WalkLoop]);
            base.Update(deltaTime);
            if (calcModule.dataInput.currentFoot != CurrentFoot.same)
            {
                //如果有在移动的脚，将脚属性刷新，赋值
                int currentFoot = calcModule.dataInput.currentFoot.GetHashCode();
                
                Vector3 stepTo = calcModule.CalculateStepToPosition(sharedInfo.footsteps[currentFoot], avatarInfo.rootBone.solverRotation);
                
                lastStepOffset = stepTo;
                
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
                    sharedInfo.footsteps[i].UpdateStanding(calcModule.forwardRotation, setup.relaxLegTwistMinAngle,stateProgess);
                }
                
                //平滑更新
                sharedInfo.footsteps[i].Update(stepInterpolation, sharedInfo.footsteps[i].onFootstep);

            }
            
            RefreshData(deltaTime);
        }



    }
}
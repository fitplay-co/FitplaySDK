using System;

using UnityEngine;
using UnityEngine.Events;
using VRIK.State;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    public class ProceduralState : LocomotionState<LocomotionFsmComponent>
    {
        
        public ProceduralState(LocomotionFsmComponent owner) : base(owner)
        {
        }
        
        public LocomotionState<ProceduralState> parent;
        public LocomotionState<ProceduralState> nextState;

        public WalkStateEnum stateEnum;
        protected CalculatorModule calcModule;
        private Vector3 stepToPosition;
        protected float stateProgess;
        protected int supportLegIndex;//Solver传参进来
        protected Vector3 lastStepOffset;

        //-----TODO：数据流向重构
        protected BaseAvatarIKInfo avatarInfo;

        protected ProceduralSetup setup;
        //--------
        private Vector3 LastFramePosition;
        // public CurrentFoot sharedInfo.LastStepedFoot = CurrentFoot.same;

        
        public InterpolationMode stepInterpolation = InterpolationMode.InOutSine;
        

        private Vector3 rootPosition;
        protected StateSharedInfo sharedInfo;


        //这个方法实际上是状态
        public virtual void Initialize()
        {
            //传一下人物参数
            calcModule.dataInput = calcModule.dataInput;
            
           
            //TODO：人物参数后续找地方放
           
            sharedInfo.LastStepedFoot = calcModule.dataInput.currentFoot;
        }

        // private void InitializeFootStep()
        // {
        //     
        //     footsteps[0].characterSpaceOffset = Vector3.left * (setup.footDistance * avatarInfo.scale);
        //     footsteps[1].characterSpaceOffset = Vector3.right * (setup.footDistance * avatarInfo.scale);
        //
        // }
        public override void Enter()
        {
            sharedInfo.LastStepedFoot = calcModule.dataInput.currentFoot;
        }

        public override void Exit()
        {
            base.Exit();
        }
        public void SetProceduralState(CalculatorModule calcModule, ProceduralSetup proceduralSetup, StateSharedInfo stateSharedInfo)
        {
            this.setup = proceduralSetup;
            this.calcModule = calcModule;
            this.sharedInfo = stateSharedInfo;
            this.Initialize();
        }

        public void SetDataGroup(BaseAvatarIKInfo baseAvatarIKInfo)
        {
            this.avatarInfo = baseAvatarIKInfo;
            calcModule.InitialWalkState(avatarInfo.rootBone,avatarInfo.spine,sharedInfo.footsteps);
        }
        //

        public override void Update(float deltaTime)
        {
            UnityEngine.Debug.Log("State!-------------------" +this.stateEnum);
            calcModule.Update();
            float newProgress = Mathf.Clamp01(calcModule.dataInput.currentDeltaHeight / calcModule.maxHeight);
            
 
             switch (calcModule.velocityDirection)
            {
                case CalculatorModule.VelocityDirection.upward:
                    stateProgess = 0.5f * newProgress;
                    break;
                case CalculatorModule.VelocityDirection.downward:
                    stateProgess = 0.5f + (1f -newProgress) * 0.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.Log("DeltaHeight---------------" + calcModule.dataInput.currentDeltaHeight +" |*******| progress---------------" + calcModule.maxHeight);
            Debug.Log("raw---------------" + newProgress +"progress---------------" + stateProgess + "$$$$$$$$$$$$"  + Time.frameCount);
            
            rootPosition = calcModule.SolveRootPosition(avatarInfo.rootBone, sharedInfo.footsteps, avatarInfo.spine);
            
            Debug.Log("StateFoot====================" + sharedInfo.LastStepedFoot +  "-----OSFoot==============" + calcModule.dataInput.currentFoot);

        
        }

        public void RefreshData(float deltaTime)
        {

            //更新属性
            avatarInfo.leftFootPosition = sharedInfo.footsteps[0].position;
            avatarInfo.rightFootPosition = sharedInfo.footsteps[1].position;

            avatarInfo.leftFootPosition = V3Tools.PointToPlane(avatarInfo.leftFootPosition, avatarInfo.leftLeg.lastBone.readPosition, calcModule.rootUp);
            avatarInfo.rightFootPosition = V3Tools.PointToPlane(avatarInfo.rightFootPosition, avatarInfo.rightLeg.lastBone.readPosition, calcModule.rootUp);

            avatarInfo.leftFootOffset = setup.stepHeight.Evaluate(sharedInfo.footsteps[0].stepProgress) * avatarInfo.scale;
            avatarInfo.rightFootOffset = setup.stepHeight.Evaluate(sharedInfo.footsteps[1].stepProgress) * avatarInfo.scale;

            avatarInfo.leftHeelOffset = setup.heelHeight.Evaluate(sharedInfo.footsteps[0].stepProgress) * avatarInfo.scale;
            avatarInfo.rightHeelOffset = setup.heelHeight.Evaluate(sharedInfo.footsteps[1].stepProgress) * avatarInfo.scale;

            avatarInfo.leftFootRotation = sharedInfo.footsteps[0].rotation;
            avatarInfo.rightFootRotation = sharedInfo.footsteps[1].rotation;

            //记录上帧数据

            avatarInfo.rootVelocity = (rootPosition - LastFramePosition) / deltaTime;
            LastFramePosition = rootPosition;
        }

        public virtual void Reset()
        {
            
        }

        public void SetEnum(WalkStateEnum stateEnum)
        {
            this.stateEnum = stateEnum;
        }
    }
}
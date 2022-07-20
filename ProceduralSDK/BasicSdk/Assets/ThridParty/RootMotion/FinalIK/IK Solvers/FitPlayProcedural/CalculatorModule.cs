using System;
using UnityEngine;


namespace RootMotion.FinalIK.FitPlayProcedural
{
    public class CalculatorModule
    {
        
        public OSDataInput dataInput;
        public float maxHeight = 0f;//OS返点到地面最高距离，用来记录预测的StepTo位置
        private float lastFrameDeltaHeight = 0f;
        public enum VelocityDirection
        {
            upward,
            downward
        }
        public VelocityDirection velocityDirection;
        public Quaternion forwardRotation;
        public Vector3 rootUp;
        public float reflectSpeed = 3f;
        
        //


        //后续删除
        public IKSolverVR.VirtualBone RootBone { get; set; }

        public IKSolverVR.Spine Spine { get; set; }
        
        public IKProceduralFootstep[] Footstep { get; set; }

        public CalculatorModule(OSDataInput inputData)
        {
            dataInput = inputData;
        }

        /*public float SolveRootLocomotion()
        {
            return 0f;
        }*/
        public Vector3 MirrorStepToPosition(IKProceduralFootstep activeFootStep, IKProceduralFootstep catchupFootstep, Quaternion rootRotation)
        {
            return activeFootStep.stepTo - rootRotation * activeFootStep.characterSpaceOffset +rootRotation * catchupFootstep.characterSpaceOffset ;
        }
        public Vector3 CalculateStepToPosition(IKProceduralFootstep footStep, Quaternion rootRotation)
        {
            var currentPridictedStep = new Vector3(0, 0, maxHeight * reflectSpeed);
            var stepTo = currentPridictedStep + rootRotation * footStep.characterSpaceOffset;
            return stepTo;
        }
        
        public Vector3 CalculateStepToPosition(IKProceduralFootstep footStep,Quaternion rootRotation,Vector3 lastStepToOffset)
        {
            var currentPridictedStep = new Vector3(0, 0, maxHeight * reflectSpeed) + lastStepToOffset;
            var stepTo = currentPridictedStep + rootRotation * footStep.characterSpaceOffset;
            return stepTo;
        }
        public Vector3 SolveRootPosition(IKSolverVR.VirtualBone rootBone, IKProceduralFootstep[] footsteps, IKSolverVR.Spine spine)
        {
            rootUp = rootBone.solverRotation * Vector3.up;
            //忽略COM，直接取物理特性重心和Root做配对（双脚中心）
            //取双脚位置预估物理特性重心做Center of Pressure
            Vector3 centerOfPressure = Vector3.Lerp(footsteps[0].position, footsteps[1].position, 0.5f);
            return V3Tools.PointToPlane(centerOfPressure, rootBone.solverPosition, rootUp);
        }
        
        private void UpdateCurrentDirection()
        {
            float dirThreshold = 0.01f;
            
            if (Mathf.Abs(lastFrameDeltaHeight - dataInput.currentDeltaHeight) > dirThreshold)
            {
                velocityDirection = lastFrameDeltaHeight > dataInput.currentDeltaHeight ? VelocityDirection.downward : VelocityDirection.upward;
            }
            
        }
        public void Update()
        {
            dataInput.OSUpdate();
            
            UpdateCurrentDirection();
            maxHeight = Math.Max(dataInput.currentDeltaHeight, maxHeight);

             //Rootbone和人物的信息，
            Vector3 rootUp = RootBone.solverRotation * Vector3.up;

  
            Vector3 forward = Spine.faceDirection;
            Vector3 forwardY = V3Tools.ExtractVertical(forward, rootUp, 1f);
            forward -= forwardY;
            forwardRotation = Quaternion.LookRotation(forward, rootUp);
            if (Spine.rootHeadingOffset != 0f) 
                forwardRotation = Quaternion.AngleAxis(Spine.rootHeadingOffset, rootUp) * forwardRotation;
            
            //---Refresh---

            lastFrameDeltaHeight = dataInput.currentDeltaHeight;
        }


        public void InitialWalkState(IKSolverVR.VirtualBone rootBone, IKSolverVR.Spine spine,
            IKProceduralFootstep[] ikProceduralFootsteps)
        {
            RootBone = rootBone;
            Spine = spine;
            Footstep = ikProceduralFootsteps;
        }


 
    }
}
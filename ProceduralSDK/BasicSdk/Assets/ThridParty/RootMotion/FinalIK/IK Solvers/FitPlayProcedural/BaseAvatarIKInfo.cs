using UnityEngine;

namespace RootMotion.FinalIK.FitPlayProcedural
{
    public class BaseAvatarIKInfo
    {
        public IKSolverVR.VirtualBone rootBone;
        public IKSolverVR.Spine spine;
        public IKSolverVR.Leg leftLeg;
        public IKSolverVR.Leg rightLeg;
        public IKSolverVR.Arm leftArm;
        public IKSolverVR.Arm rightArm;
        public int supportLegIndex;
        public Vector3 leftFootPosition;
        public Vector3 rightFootPosition;
        public Quaternion leftFootRotation;
        public Quaternion rightFootRotation;
        public float leftFootOffset;
        public float rightFootOffset;
        public float leftHeelOffset;
        public float rightHeelOffset;
        public float scale;
        public float deltaTime;

        public Vector3 rootVelocity;
        
        
        //DefaultValue


        public void SetInfo(IKSolverVR.VirtualBone rootBone, IKSolverVR.Spine spine, IKSolverVR.Leg leftLeg, IKSolverVR.Leg rightLeg, IKSolverVR.Arm leftArm, 
            IKSolverVR.Arm rightArm, int supportLegIndex,Vector3 leftFootPosition,  Vector3 rightFootPosition,  Quaternion leftFootRotation,  
            Quaternion rightFootRotation,  float leftFootOffset,  float rightFootOffset,  float leftHeelOffset,  float rightHeelOffset, float scale,
            float deltaTime,Vector3 rootVelocity)
        {
            this.rootBone = rootBone;
            this.spine = spine;
            this.leftLeg = leftLeg;
            this.rightLeg = rightLeg;
            this.leftArm = leftArm;
            this.rightArm = rightArm;
            this.supportLegIndex = supportLegIndex;
            this.leftFootPosition = leftFootPosition;
            this.rightFootPosition = rightFootPosition;
            this.leftFootRotation = leftFootRotation;
            this.rightFootRotation = rightFootRotation;
            this.leftFootOffset = leftFootOffset;
            this.rightFootOffset = rightFootOffset;
            this.leftHeelOffset = leftHeelOffset;
            this.rightHeelOffset = rightHeelOffset;
            this.scale = scale;
            this.deltaTime = deltaTime;
            this.rootVelocity = rootVelocity;
        }


        public void GetInfo(out Vector3 leftFootPosition, out Vector3 rightFootPosition, out Quaternion leftFootRotation, out Quaternion rightFootRotation, out float leftFootOffset, out float rightFootOffset, out float leftHeelOffset, out float rightHeelOffset)
        {
            leftFootPosition = this.leftFootPosition;
            rightFootPosition = this.rightFootPosition;
            leftFootRotation = this.rightFootRotation;
            rightFootRotation = this.rightFootRotation;
            leftFootOffset = this.leftFootOffset;
            rightFootOffset = this.rightFootOffset;
            leftHeelOffset = this.leftHeelOffset;
            rightHeelOffset = this.rightHeelOffset;
            
        }
    }
}
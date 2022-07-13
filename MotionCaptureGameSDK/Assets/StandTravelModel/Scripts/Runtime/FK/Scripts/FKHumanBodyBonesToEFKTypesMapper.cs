using MotionCaptureBasic.OSConnector;
using UnityEngine;

namespace StandTravelModel.Scripts.Runtime.FK.Scripts
{
    public static class FKHumanBodyBonesToEFKTypesMapper
    {
        private static HumanBodyBones[] humanBodyBones;

        public static HumanBodyBones GetHumanBodyBone(EFKType eFKType)
        {
            TryInit();
            return humanBodyBones[(int)eFKType];
        }

        private static void AddBone(EFKType eFKType, HumanBodyBones bodyBone)
        {
            humanBodyBones[(int)eFKType] = bodyBone;
        }

        private static void TryInit()
        {
            if(humanBodyBones == null)
            {
                var eFKTypes = System.Enum.GetNames(typeof(EFKType));
                humanBodyBones = new HumanBodyBones[eFKTypes.Length];
            }

            AddBone(EFKType.RShoulder,  HumanBodyBones.RightShoulder);
            AddBone(EFKType.RArm,       HumanBodyBones.RightUpperArm);
            AddBone(EFKType.RWrist,     HumanBodyBones.RightLowerArm);
            AddBone(EFKType.RHand,      HumanBodyBones.RightHand);

            AddBone(EFKType.LShoulder,  HumanBodyBones.LeftShoulder);
            AddBone(EFKType.LArm,       HumanBodyBones.LeftUpperArm);
            AddBone(EFKType.LWrist,     HumanBodyBones.LeftLowerArm);
            AddBone(EFKType.LHand,      HumanBodyBones.LeftHand);

            AddBone(EFKType.Head,       HumanBodyBones.Neck);
            AddBone(EFKType.Neck,       HumanBodyBones.Spine);
            AddBone(EFKType.CenterHip,  HumanBodyBones.Hips);

            AddBone(EFKType.RHip,       HumanBodyBones.Hips);
            AddBone(EFKType.RKnee,      HumanBodyBones.RightUpperLeg);
            AddBone(EFKType.RAnkle,     HumanBodyBones.RightLowerLeg);
            AddBone(EFKType.RFoot,      HumanBodyBones.RightFoot);

            AddBone(EFKType.LHip,       HumanBodyBones.Hips);
            AddBone(EFKType.LKnee,      HumanBodyBones.LeftUpperLeg);
            AddBone(EFKType.LAnkle,     HumanBodyBones.LeftLowerLeg);
            AddBone(EFKType.LFoot,      HumanBodyBones.LeftFoot);
        }
    }
}
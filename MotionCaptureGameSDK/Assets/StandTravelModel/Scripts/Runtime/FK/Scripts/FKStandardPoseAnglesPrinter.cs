using UnityEngine;
using MotionCaptureBasic.OSConnector;

namespace FK
{
    public class FKStandardPoseAnglesPrinter : MonoBehaviour
    {
        private EFKType[] eFKTypes = {
            EFKType.RShoulder,
            EFKType.RArm,
            EFKType.RWrist,
            EFKType.RHand,

            EFKType.LShoulder,
            EFKType.LArm,
            EFKType.LWrist,
            EFKType.LHand,

            EFKType.Head,
            EFKType.Neck,
            EFKType.CenterHip,

            EFKType.RHip,
            EFKType.RKnee,
            EFKType.RAnkle,
            EFKType.RFoot,

            EFKType.LHip,
            EFKType.LKnee,
            EFKType.LAnkle,
            EFKType.LFoot,
        };

        public void PrintPoseAngles()
        {
            Debug.Log("---------------------------");
            var anim = GetComponent<Animator>();
            foreach(var eFKType in eFKTypes)
            {
                PrintBoneAngle(eFKType, anim);
            }
        }

        public void PrintPosForwards()
        {
            var anim = GetComponent<Animator>();
            foreach(var eFKType in eFKTypes)
            {
                PrintBoneForward(eFKType, anim);
            }
        }

        private void PrintBoneAngle(EFKType eFKType, Animator animator)
        {
            var boneBody = FKHumanBodyBonesToEFKTypesMapper.GetHumanBodyBone(eFKType);
            var boneTran = animator.GetBoneTransform(boneBody);
            Debug.Log(eFKType + "|" + boneTran.eulerAngles);
        }

        private void PrintBoneForward(EFKType eFKType, Animator animator)
        {
            var boneBody = FKHumanBodyBonesToEFKTypesMapper.GetHumanBodyBone(eFKType);
            var boneTran = animator.GetBoneTransform(boneBody);
            Debug.Log(eFKType + "|" + boneTran.forward);
        }
    }
}